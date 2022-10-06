using System;
using System.Diagnostics;
using T3;
using T3.Core;
using T3.Core.Logging;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using System.Collections.Generic;
using T3.Core.DataTypes;
using Neo.IronLua;
using T3.Operators.Utils;



namespace T3.Operators.Types.Id_2922d665_6d1c_459b_a7bb_b41a1a6fbbe2
{
    public class LuaMathExprFloat : Instance<LuaMathExprFloat>
    {
        [Output(Guid = "8505795d-7fe7-4cf3-b6eb-58e794709b5d", DirtyFlagTrigger = DirtyFlagTrigger.Animated)]
        public readonly Slot<float> Result = new Slot<float>();

        [Input(Guid = "72857c83-f68f-4675-8d56-9b3d37d8d506")]
        public readonly InputSlot<string> Expression = new();

        [Input(Guid = "3b2b4c06-20c8-46da-b33d-633f5d21e9b0", MappedType = typeof(TimeModes))]
        public readonly InputSlot<int> t_mode = new InputSlot<int>();

        [Input(Guid = "21268241-579f-454c-8e11-b64a78c5fc41")]
        public readonly InputSlot<float> a0 = new InputSlot<float>();

        [Input(Guid = "3e0be1b0-4d4d-4d9e-9cf3-e84e66dd71b7")]
        public readonly InputSlot<float> a1 = new InputSlot<float>();

        [Input(Guid = "edff0a8c-a0df-43ae-a29b-c98f2c17393c")]
        public readonly InputSlot<float> a2 = new InputSlot<float>();

        [Input(Guid = "8ec7b251-6467-4649-9fb7-512429e4614c")]
        public readonly InputSlot<float> a3 = new InputSlot<float>();

        [Input(Guid = "68625cb7-d671-400a-a203-37d109f8c995")]
        public readonly InputSlot<float> a4 = new InputSlot<float>();

        [Input(Guid = "1755dab6-4569-45fa-a322-713c4fc13851")]
        public readonly InputSlot<float> a5 = new InputSlot<float>();


        private string PrevExprStr;
        private bool WasError = false;
        public LuaVM LocalVM = null;
        private LuaChunk CompiledScript;

        protected override void Dispose(bool disposing)
        {
            LocalVM?.Dispose();
            base.Dispose(disposing);
        }


        public LuaMathExprFloat()
        {
            Result.UpdateAction = Update;
        }


        private void Update(EvaluationContext context)
        {
            double result = float.NaN;
            try
            {
                LuaVM vm = null;
                string strExpr = Expression.GetValue(context);
                vm = GetCompiledFunc(vm, strExpr);
                if (vm != null)
                {
                    TimeModes timeMode = (TimeModes)this.t_mode.GetValue(context);
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

                    //Dictionary<string, KeyValuePair<string, double>> kv = new();
                    //kv.Values

                    UpdateVar(vm, "t", tVal);
                    UpdateVar(vm, "a0", this.a0.GetValue(context));
                    UpdateVar(vm, "a1", this.a1.GetValue(context));
                    UpdateVar(vm, "a2", this.a2.GetValue(context));
                    UpdateVar(vm, "a3", this.a3.GetValue(context));
                    UpdateVar(vm, "a4", this.a4.GetValue(context));
                    UpdateVar(vm, "a5", this.a5.GetValue(context));

                    LuaResult r = vm.Env.DoChunk(this.CompiledScript);
                    if (r != null)
                    {
                        result = Convert.ToDouble(r[0]); // evaluate math expression
                    }
                }
            }
            catch (Exception ex)
            {
                if (!WasError)
                {
                    WasError = true;
                    Log.Error($"Math function compile error: {ex.Message}");
                }
            }
            Result.Value = (float)result;
            Result.DirtyFlag.Clear();
        }



        private LuaVM GetCompiledFunc( LuaVM in_vm, string mathStr)
        {
            if (PrevExprStr == mathStr && this.LocalVM != null)
            {
                if (WasError)
                    return null;
                return this.LocalVM;
            }

            PrevExprStr = mathStr;
            if (string.IsNullOrWhiteSpace(mathStr))
            {
                return null;
            }
            else
            {
                WasError = false;
                if (this.LocalVM == null)
                {
                    this.LocalVM = new LuaVM();
                    //MathParser.DefineConst("pi", Math.PI);
                    //MathParser.DefineConst("e", Math.E);
                }

                LuaUtils.DeclareMathFuncs(this.LocalVM);
                string ProgramSource = "return %expr%";
                mathStr = ProgramSource.Replace("%expr%", mathStr);

                LuaCompileOptions opt = new();
                opt.ClrEnabled = false;
                CompiledScript = LocalVM.Lua.CompileChunk(mathStr, "CurExpr.lua", opt);

            }
            return this.LocalVM;
        }


        public static void UpdateVar(LuaVM vm, string name, double val)
        {
            vm.Env[name] = val;
        }


        public enum TimeModes
        {
            FxTimeInBars, //=context.LocalFxTime (timeline_current_time + AppTime)
            TimeLineTimeInBars, //=context.LocalTime (timeline_current_time)
            FxTimeInSec,
            TimeLineTimeInSec,
        }


    }



}