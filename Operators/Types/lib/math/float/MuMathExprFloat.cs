using System;
using System.Diagnostics;
using T3;
using T3.Core;
using T3.Core.Logging;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using System.Collections.Generic;
using muParserNET;




namespace T3.Operators.Types.Id_4ebfa0e6_bfbc_499d_9dec_20ec455b3178
{
    public class MuMathExprFloat : Instance<MuMathExprFloat>
    {
        [Output(Guid = "482041bd-0577-4d91-8d57-ec7b70c52ce1", DirtyFlagTrigger = DirtyFlagTrigger.Animated)]
        public readonly Slot<float> Result = new Slot<float>();

        [Input(Guid = "0da599f1-fe10-45ce-a467-644df1120945")]
        public readonly InputSlot<string> Expression = new();

        [Input(Guid = "af6f1bb3-3b93-4ae1-bf54-f64b306dd52e")]
        public readonly InputSlot<float> a0 = new InputSlot<float>();

        [Input(Guid = "1da9814e-924e-4f98-8783-d603e86fc60f")]
        public readonly InputSlot<float> a1 = new InputSlot<float>();

        [Input(Guid = "1159c710-93f6-4fd2-97b8-aaca96862f1d")]
        public readonly InputSlot<float> a2 = new InputSlot<float>();

        [Input(Guid = "1a08fcda-604f-4eca-822a-892338689eee")]
        public readonly InputSlot<float> a3 = new InputSlot<float>();

        [Input(Guid = "2539a5f5-8487-4b0c-8599-313586c25804")]
        public readonly InputSlot<float> a4 = new InputSlot<float>();

        [Input(Guid = "e57544f7-6735-438f-80e9-19624304a07f")]
        public readonly InputSlot<float> a5 = new InputSlot<float>();

        [Input(Guid = "7c7ad7dc-bcfa-4fea-b02d-658c12a4ddad", MappedType = typeof(TimeModes))]
        public readonly InputSlot<int> t = new InputSlot<int>();

        private string PrevExprStr;
        private Parser MathParser;
        private bool WasError = false;


        public MuMathExprFloat()
        {
            Result.UpdateAction = Update;
        }


        private void Update(EvaluationContext context)
        {
            double result = float.NaN;
            try
            {
                Parser mathExpr = null;
                string strExpr = Expression.GetValue(context);
                mathExpr = GetCompiledFunc(strExpr);
                if (mathExpr != null)
                {
                    Parser p = MathParser;

                    TimeModes timeMode = (TimeModes)this.t.GetValue(context);
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

                    UpdateVar(p, "t", tVal); 

                    UpdateVar(p, "a0", this.a0.GetValue(context));
                    UpdateVar(p, "a1", this.a1.GetValue(context));
                    UpdateVar(p, "a2", this.a2.GetValue(context));
                    UpdateVar(p, "a3", this.a3.GetValue(context));
                    UpdateVar(p, "a4", this.a4.GetValue(context));
                    UpdateVar(p, "a5", this.a5.GetValue(context));

                    result = p.Eval(); // evaluate math expression
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


        private Parser GetCompiledFunc(string mathStr)
        {
            if (PrevExprStr == mathStr && MathParser != null)
            {
                if (WasError)
                    return null;
                return MathParser;
            }

            PrevExprStr = mathStr;
            if (string.IsNullOrWhiteSpace(mathStr))
            {
                return null;
            }
            else
            {
                WasError = false;
                if (MathParser == null)
                {
                    MathParser = new Parser();
                    MathParser.DefineConst("pi", Math.PI);
                    MathParser.DefineConst("e", Math.E);
                }

                MathParser.Expr = mathStr;
            }
            return MathParser;
        }


        public static void UpdateVar(Parser parser, string name, double val)
        {
            ParserVariable pvar;
            if (!parser.Vars.TryGetValue(name, out pvar))
            {
                pvar = parser.DefineVar(name, 0);
            }
            pvar.Value = val;
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