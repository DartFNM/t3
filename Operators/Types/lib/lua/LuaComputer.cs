using System;
using System.Diagnostics;
using T3;
using T3.Core;
using T3.Core.DataTypes;
using T3.Core.Logging;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Operators.Utils;
using System.Collections.Generic;
using Neo.IronLua;



namespace T3.Operators.Types.Id_5fa96817_4560_4362_8e75_63da634315fd
{
    public class LuaComputer : Instance<LuaComputer>
    {
        // OUTPUTS
        [Output(Guid = "00000004-0000-0000-0000-000000000000")]
        public readonly Slot<Command> Tick = new Slot<Command>();

        [Output(Guid = "00000001-0000-0000-0000-000000000000", DirtyFlagTrigger = DirtyFlagTrigger.Animated)]
        public readonly Slot<float> ResultFloat = new();

        [Output(Guid = "00000002-0000-0000-0000-000000000000", DirtyFlagTrigger = DirtyFlagTrigger.Animated)]
        public readonly Slot<System.Numerics.Vector2> ResultVec2 = new();

        [Output(Guid = "00000003-0000-0000-0000-000000000000", DirtyFlagTrigger = DirtyFlagTrigger.Animated)]
        public readonly Slot<System.Numerics.Vector3> ResultVec3 = new();

        [Output(Guid = "00000005-0000-0000-0000-000000000000", DirtyFlagTrigger = DirtyFlagTrigger.Animated)]
        public readonly Slot<LuaVM> OutputVM = new Slot<LuaVM>();

        // INPUTS
        [Input(Guid = "10000001-0000-0000-0000-000000000000")]
        public readonly InputSlot<string> Expression = new();

        [Input(Guid = "10000002-0000-0000-0000-000000000000")]
        public readonly InputSlot<bool> ResetVM = new();

        [Input(Guid = "10000003-0000-0000-0000-000000000000")]
        public readonly InputSlot<int> t_mode = new InputSlot<int>();

        [Input(Guid = "10000004-0000-0000-0000-000000000000")]
        public readonly InputSlot<LuaVM> InputVM = new InputSlot<LuaVM>();

        [Input(Guid = "10000005-0000-0000-0000-000000000000")]
        public readonly InputSlot<float> a0 = new InputSlot<float>();

        [Input(Guid = "10000005-0001-0000-0000-000000000000")]
        public readonly InputSlot<float> a1 = new InputSlot<float>();

        [Input(Guid = "10000005-0002-0000-0000-000000000000")]
        public readonly InputSlot<float> a2 = new InputSlot<float>();

        [Input(Guid = "10000005-0003-0000-0000-000000000000")]
        public readonly InputSlot<float> a3 = new InputSlot<float>();

        [Input(Guid = "10000005-0004-0000-0000-000000000000")]
        public readonly InputSlot<float> a4 = new InputSlot<float>();

        [Input(Guid = "10000005-0005-0000-0000-000000000000", MappedType = typeof(LuaUtils.TimeModes))]
        public readonly InputSlot<float> a5 = new InputSlot<float>();


        CompiledScript CompScript = new();
        private bool _reset = false;


        protected override void Dispose(bool disposing)
        {
            CompScript?.Dispose();
            CompScript = null;
            base.Dispose(disposing);
        }


        public LuaComputer()
        {
            Tick.UpdateAction = Update;
            OutputVM.UpdateAction = Update;

            ResultFloat.UpdateAction = Update;
            ResultVec2.UpdateAction = Update;
            ResultVec3.UpdateAction = Update;

        }


        private void Update(EvaluationContext context)
        {
            LuaUtils.TimeModes timeMode = (LuaUtils.TimeModes)this.t_mode.GetValue(context);
            double tVal = LuaUtils.GetContextTime(timeMode, context);

            LuaVM inVM = this.InputVM.GetValue(context);
            string strExpr = this.Expression.GetValue(context);

            bool reset = this.ResetVM.GetValue(context);
            if (reset)
            {
                CompScript.ResetLocalVM();
                this.ResetVM.Value = false;
            }

            string id = this.SymbolChildId.ToString();
            if (CompScript != null && CompScript.CompileScript(strExpr, id, inVM))
            {
                var sc = CompScript;
                sc.UpdateVar("t", tVal);
                sc.UpdateVar("a0", this.a0.GetValue(context));
                sc.UpdateVar("a1", this.a1.GetValue(context));
                sc.UpdateVar("a2", this.a2.GetValue(context));
                sc.UpdateVar("a3", this.a3.GetValue(context));
                sc.UpdateVar("a4", this.a4.GetValue(context));
                sc.UpdateVar("a5", this.a5.GetValue(context));

                // Fetch Context Variables to script
                foreach( var v in context.FloatVariables) {
                    sc.UpdateVar(v.Key, v.Value);
                }
                foreach (var v in context.IntVariables) {
                    sc.UpdateVar(v.Key, (float)v.Value);
                }

                sc.RunScript();

                if (sc.TryCallInitFunc())
                {
                    sc.CallUpdateFunc(tVal);
                }
            }
            float resFloat = float.NaN;
            CompScript?.ResultToFloat(ref resFloat);
            ResultFloat.Value = resFloat;

            System.Numerics.Vector2 resVec2 = new(float.NaN, float.NaN);
            CompScript?.ResultToVec2(ref resVec2);
            ResultVec2.Value = resVec2;

            System.Numerics.Vector3 resVec3 = new(float.NaN, float.NaN, float.NaN);
            CompScript?.ResultToVec3(ref resVec3);
            ResultVec3.Value = resVec3;


            OutputVM.Value = this.CompScript?.CurrentVM;
            InputVM.DirtyFlag.Clear();
            ResultFloat.DirtyFlag.Clear();

        }






    }



}