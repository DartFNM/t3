﻿using System.Numerics;
using ImGuiNET;
using T3.Gui.Interaction;

namespace T3.Gui.InputUi
{
    public class IntInputUi : SingleControlInputUi<int>
    {
        protected override bool DrawSingleEditControl(string name, ref int value)
        {
            var result= SingleValueEdit.Draw(ref value, new Vector2(-1, 0));
            return result == InputEditStateFlags.Modified;
        }

        protected override string GetSlotValueAsString(ref int intValue)
        {
            // This is a stub of value editing. Sadly it's very hard to get
            // under control because of styling issues and because in GraphNodes
            // The op body captures the mouse event first.
            //SingleValueEdit.Draw(ref floatValue,  -Vector2.UnitX);
            
            return intValue.ToString();
        }
        
        protected override void DrawValueDisplay(string name, ref int value)
        {
            ImGui.InputInt(name, ref value, 0, 0, ImGuiInputTextFlags.ReadOnly);
        }
    }
}