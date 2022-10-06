using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T3.Core.Logging;
using T3.Core.DataTypes;
using T3.Core.Operator;

using Neo.IronLua;


namespace T3.Operators.Utils
{

    public static class LuaUtils
    {
        public static void DeclareMathFuncs(LuaVM vm)
        {
            const string LibName = "T3Math.lua";
            if (vm.IsLibraryDeclared(LibName))
                return;
            string src = @"
const pi = 3.1415926535898
rad2deg = math.deg
deg2rad = math.rad

abs = math.abs
acos = math.acos
asin = math.asin
atan = math.atan
atan2 = math.atan2
ceil = math.ceil
cos = math.cos
exp = math.exp
floor = math.floor
log = math.log
log10 = math.log10
max = math.max
min = math.min
pow = math.pow
sin = math.sin
sqrt = math.sqrt
tan = math.tan
deg = math.deg
rad = math.rad
random = math.random
rand = math.random

--hypotenuse function: computes sqrt(a^2 + b^2) without underflow / overflow problems.
function hypot(a, b)
	if a == 0 and b == 0 then return 0 end
	a, b = abs(a), abs(b)
	a, b = max(a,b), min(a,b)
	return a * sqrt(1 + (b / a)^2)
end
--distance between two points. avoids underflow and overflow.
function distance(x1, y1, x2, y2)
	return hypot(x2-x1, y2-y1)
end

--distance between two points squared.
function distance2(x1, y1, x2, y2)
	return (x2-x1)^2 + (y2-y1)^2
end

function smoothstep(from, to, t)
    t = clamp01(t)
    t = -2 * t * t * t + 3 * t * t
    return to * t + from * (1 - t)
end

function clamp(value, min, max)
	if value < min then
		value = min
	elseif value > max then
		value = max    
	end
	return value
end

function clamp01(value)
	if value < 0 then
		return 0
	elseif value > 1 then
		return 1   
	end
	return value
end


function lerp(a, b, t)
  return a + (b - a) * t
end

function round(x)
	return floor(x + 0.5)
end

function sign(num)
	if num > 0 then
		num = 1
	elseif num < 0 then
		num = -1
	else 
		num = 0
	end
	return num
end


function mod(a, b)
	return a - floor(a / b) * b
end 
function modi(a, b)
	return floor(a - floor(a / b) * b)
end 

function fract(a)
	return a - floor(a)
end 

function mix(x, y, t)
    return (x+(y-x)*t)
end

function pingpong(t, length)
    t = mod(t, length * 2)
    return length - abs(t - length)
end
";

            vm.Env.DoChunk(src, LibName);
            vm.Libraries.Add(LibName);
        }

        public enum TimeModes
        {
            FxTimeInBars, //=context.LocalFxTime (timeline_current_time + AppTime)
            TimeLineTimeInBars, //=context.LocalTime (timeline_current_time)
            FxTimeInSec,
            TimeLineTimeInSec,
        }


        public static double GetContextTime(TimeModes timeMode, EvaluationContext context)
        {
            double tVal = 0;
            switch (timeMode)
            {
                default:
                case TimeModes.FxTimeInBars:
                    tVal = context.LocalFxTime; // timeline_current_time + AppTime
                    break;
                case TimeModes.TimeLineTimeInBars:
                    tVal = context.LocalTime; // timeline_current_time
                    break;
                case TimeModes.FxTimeInSec:
                    tVal = context.LocalFxTime * 240.0 / context.Playback.Bpm;
                    break;
                case TimeModes.TimeLineTimeInSec:
                    tVal = context.LocalTime * 240.0 / context.Playback.Bpm;
                    break;
            }
            return tVal;
        }

    }; // class LuaUtils
    ////////


    ///////////////////////////////////////////////////
    public class CompiledScript : IDisposable
    {
        private string PrevExprStr;
        public bool _WasRuntimeError = false;
        public bool WasCompileError = false;
        public LuaVM LocalVM = null; // VM owner
        public LuaVM CurrentVM = null; // last used
        private LuaChunk Compiled;
        double LastCompilationTIme = double.NaN;
        double LastRuntimeError = double.NaN;


        public void Dispose()
        {
            LocalVM?.Dispose();
            LocalVM = null;
        }


        public void CreateLocalVM()
        {
            if (LocalVM != null)
            {
                LocalVM.Dispose();
                LocalVM = null;
            }
            this.LocalVM = new LuaVM(); // create our own Virtual Machine
            LuaUtils.DeclareMathFuncs(this.CurrentVM);
        }



        public bool CompileScript(string strScript, string id, LuaVM overrideVM = null)
        {
            double curTime = T3.Core.Animation.Playback.RunTimeInSecs;
            double timeSinceLastCompilation = curTime - LastCompilationTIme;
            try
            {
                if (string.IsNullOrWhiteSpace(strScript))
                {
                    return false;
                }

                if (PrevExprStr == strScript && this.CurrentVM != null)
                {
                    if (!WasCompileError && Compiled != null)
                        return true;

                    if (overrideVM == null)
                    {
                        if (overrideVM == CurrentVM)
                        {
                            return true; // Complied script is valid
                        }
                    }
                    // Virtual Machine was changed
                }

               if (timeSinceLastCompilation < 0.5)
                    return false;

                // Recompile script
               this.PrevExprStr = strScript;
                WasCompileError = false;
                LastCompilationTIme = curTime;

                if (overrideVM == null && this.LocalVM == null)
                {
                    CreateLocalVM();
                }
                this.CurrentVM = (overrideVM != null) ? overrideVM : LocalVM;

                LuaCompileOptions opt = new();
                //opt.ClrEnabled = false;
                Compiled = CurrentVM.Lua.CompileChunk(strScript, id, opt);
                WasRuntimeError = false;
                return true;
            }
            catch (Exception ex)
            {
                if (!WasCompileError)
                {
                    WasCompileError = true;
                    Log.Error($"LuaScript compilation error: {ex.Message}");
                }
                return false;
            }
        }


        public bool WasRuntimeError
        {
            get => _WasRuntimeError;
            set
            {
                _WasRuntimeError = value;
                if (!value && this.CurrentVM != null)
                {
                    CurrentVM.LastResult = null;
                }
            }
        }


        public void UpdateVar(string name, double val)
        {
            if (CurrentVM != null)
                CurrentVM.Env[name] = val;
        }


        public LuaResult RunScript()
        {
            if (WasCompileError || CurrentVM == null)
                return null;

            double curTime = T3.Core.Animation.Playback.RunTimeInSecs;
            double timeSinceLastCompilation = curTime - LastRuntimeError;

            if (WasRuntimeError) {
                if (timeSinceLastCompilation < 0.5)
                    return null;
            }
            LastRuntimeError = curTime;
            WasRuntimeError = false;

            try
            {
                var luaRes = CurrentVM.Env.DoChunk(this.Compiled);
                this.CurrentVM.LastResult = luaRes;
                return luaRes;
            }
            catch (Exception ex)
            {
                if (!WasRuntimeError)
                {
                    WasRuntimeError = true;
                    Log.Error($"LuaScript runtime error: {ex.Message}");
                }
                return null;
            }
        }


        public bool ResultToFloat(ref float res)
        {
            res = float.NaN;
            LuaResult luaRes = CurrentVM?.LastResult;
            if (luaRes == null)
                return false;

            if (luaRes.Count < 1)
                return false;
            try
            {
                res = (float)Convert.ToDouble(luaRes[0]);
                return true;
            }
            catch (Exception ex)
            {
                if (!this.WasRuntimeError)
                {
                    //this.WasRuntimeError = true;
                    Log.Error($"LuaScript ResultFloat error: {ex.Message}");
                }
                return false;
            }
        }


        public bool ResultToVec2(ref System.Numerics.Vector2 res)
        {
            res = new (float.NaN, float.NaN);
            LuaResult luaRes = CurrentVM?.LastResult;
            if (luaRes == null)
                return false;

            if (luaRes.Count < 2)
                return false;
            try
            {
                res.X = (float)Convert.ToDouble(luaRes[0]);
                res.Y = (float)Convert.ToDouble(luaRes[1]);
                return true;
            }
            catch (Exception ex)
            {
                if (!this.WasRuntimeError)
                {
                    //this.WasRuntimeError = true;
                    Log.Error($"LuaScript ResultVec2 error: {ex.Message}");
                }
                return false;
            }
        }


        public bool ResultToVec3(ref System.Numerics.Vector3 res)
        {
            res = new(float.NaN, float.NaN, float.NaN);
            LuaResult luaRes = CurrentVM?.LastResult;
            if (luaRes == null)
                return false;

            if (luaRes.Count < 2)
                return false;
            try
            {
                res.X = (float)Convert.ToDouble(luaRes[0]);
                res.Y = (float)Convert.ToDouble(luaRes[1]);
                res.Z = (float)Convert.ToDouble(luaRes[2]);
                return true;
            }
            catch (Exception ex)
            {
                if (!this.WasRuntimeError)
                {
                    //this.WasRuntimeError = true;
                    Log.Error($"LuaScript ResultVec3 error: {ex.Message}");
                }
                return false;
            }
        }

        public void ResetLocalVM()
        {
            CreateLocalVM();
        }


        public bool TryCallInitFunc()
        {
            var vm = CurrentVM;
            if (vm == null)
                return false;

            if (vm.IsInitInvoked)
                return true;

            // tick virtual machine
            try
            {
                if (vm.Env.ContainsKey("init"))
                {
                    vm.Env.CallMember("init");
                    vm.IsInitInvoked = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                vm.IsInitInvoked = false;
                Log.Error($"Error in init() LuaFunction: {ex.Message}");
                return false;
            }
        }


        public void CallUpdateFunc(double tVal)
        {
            var vm = CurrentVM;
            if (vm == null)
                return;

            if (tVal != vm.PrevUpdateTime)
            {
                double timeDelta = tVal - vm.PrevUpdateTime;
                vm.PrevUpdateTime = tVal;
                if (timeDelta > 0 && vm.Env.ContainsKey("update") && !this.WasRuntimeError)
                {
                    try
                    {
                        vm.Env.CallMember("update", timeDelta);
                    }
                    catch (Exception ex)
                    {
                        this.WasRuntimeError = true;
                        Log.Error($"Error in update(dt) LuaFunction: {ex.Message}");
                    }
                }
            }

        }

    }


}; // namespace
