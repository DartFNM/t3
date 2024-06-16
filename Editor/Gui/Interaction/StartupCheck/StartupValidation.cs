﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using T3.Core.Logging;
using T3.Editor.Gui.Windows.Layouts;
using T3.Editor.SystemUi;
using T3.SystemUi;

namespace T3.Editor.Gui.Interaction.StartupCheck
{
    /// <summary>
    /// Looks for required files and folders
    /// and shows a warning popup instead of an exception...
    /// </summary>
    public static class StartupValidation
    {
        public static void CheckInstallation()
        {
            
            var checks = new Check[]
                             {
                                 new()
                                     {
                                         RequiredFilePaths = new List<string>()
                                                                 {
                                                                     @"Resources\",
                                                                     @"Resources\t3-editor\images\t3-icons.png",
                                                                     @"Resources\t3-editor\images\t3.ico",
                                                                     @"Resources\t3-editor\images\t3-SplashScreen.png",
                                                                     @"Resources\t3-editor\fonts\Roboto-Regular.ttf",
                                                                 },
                                         Message = @"Please make sure to set the correct start up directory.\n ",
                                         URL = "https://github.com/tooll3/t3/wiki/installation#setting-the-startup-directory-in-visual-studio",
                                     },
                                 new()
                                     {
                                         RequiredFilePaths = new List<string>()
                                                                 {
                                                                     LayoutHandling.LayoutPath + "layout1.json",
                                                                     @"Editor\bin\Release\net6.0-windows\bass.dll",
                                                                     @"Editor\bin\Debug\net6.0-windows\bass.dll",
                                                                 },
                                         Message = "Please run Install/install.bat.",
                                         URL = "https://github.com/tooll3/t3/wiki/installation#setup-and-installation",
                                     },
                                 new()
                                     {
                                         RequiredFilePaths = new List<string>()
                                                                 {
                                                                     @"Player\bin\Release\net6.0-windows\Player.exe",
                                                                 },
                                         Message = "This will prevent you from exporting as executable.\nPlease rebuild your solution.",
                                         URL = "https://github.com/tooll3/t3/wiki/installation#setup-and-installation",
                                     }
                             };
            var _ = checks.Any(check => !check.Do());
        }

        public static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    try
                    {
                        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                    }
                    catch (Exception e)
                    {
                        Log.Warning($"Failed to open URL {url} " + e.Message);
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
        
        private struct Check
        {
            public List<string> RequiredFilePaths;
            public string Message;
            public string URL;

            public bool Do()
            {
                var missingPaths = new List<string>();
                foreach (var filepath in RequiredFilePaths)
                {
                    if (filepath.EndsWith(@"\"))
                    {
                        if (!Directory.Exists(filepath))
                        {
                            missingPaths.Add(filepath);
                        }
                    }
                    else if (!File.Exists(filepath))
                    {
                        missingPaths.Add(filepath);
                    }
                }

                if (missingPaths.Count <= 0)
                    return true;

                const string caption = "Tooll3 setup looks incomplete";
                
                var sb = new StringBuilder();

                var startupPath = Path.GetFullPath(".");
                sb.Append($"Startup folder is:\n{startupPath}\n\n");
                
                sb.Append($"We can't find the following files...\n\n  {string.Join("\n  ", missingPaths)}");
                sb.Append("\n\n");
                sb.Append(Message);
                if (!string.IsNullOrEmpty(URL))
                {
                    sb.Append("\n\n");
                    sb.Append("Click Yes to get help");
                }


                var result = EditorUi.Instance.ShowMessageBox(sb.ToString(), caption, PopUpButtons.YesNo);
                if (result == PopUpResult.Yes)
                {
                    OpenUrl(URL);
                }
                
                EditorUi.Instance.ExitApplication();
                EditorUi.Instance.ExitThread();
                return false;
            }
        }

        public static void ValidateNotRunningFromSystemFolder()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var specialFolders = new[]
                                     {
                                         Environment.SpecialFolder.ProgramFilesX86,
                                         Environment.SpecialFolder.ProgramFiles,
                                         Environment.SpecialFolder.System,
                                         Environment.SpecialFolder.Windows,
                                     };
            
            foreach (var p in specialFolders)
            {
                var folderPath = Environment.GetFolderPath(p);
                if (currentDir.IndexOf(folderPath, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                EditorUi.Instance
                        .ShowMessageBox($"Tooll can't be started from {folderPath}",
                                        @"Error", PopUpButtons.Ok);
                EditorUi.Instance.ExitApplication();
            }
            
            // Not writeable
            var directoryInfo = new DirectoryInfo(currentDir);
            if (!directoryInfo.Attributes.HasFlag(FileAttributes.ReadOnly))
                return;
            
            EditorUi.Instance
                    .ShowMessageBox($"Can't write to current working directory: {currentDir}.",
                                    @"Error", PopUpButtons.Ok);
            EditorUi.Instance.ExitApplication();
        }

        /// <summary>
        /// Validate that operators.dll has been updated to warn users if they started "T3Editor.exe"
        /// </summary>
        public static void ValidateCurrentStandAloneExecutable()
        {
            var fiveMinutes = new TimeSpan(0, 2, 0);
            const string operatorFilePath = "Operators.dll";
            if (File.Exists(operatorFilePath) && (DateTime.Now - File.GetLastWriteTime(operatorFilePath)) <= fiveMinutes)
                return;
            
            EditorUi.Instance
                    .ShowMessageBox($"Operators.dll is outdated.\nPlease use StartT3.exe to run Tooll.",
                                    @"Error", PopUpButtons.Ok);
            EditorUi.Instance.ExitApplication();
        }
    }
}