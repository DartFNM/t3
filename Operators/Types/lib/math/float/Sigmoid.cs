using System;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_fc56f945_ba04_4d10_a516_68a479147016 
{
    public class Sigmoid : Instance<Sigmoid>
    {
        [Output(Guid = "00b2d450-f19f-4deb-b88a-7c75972c0962")]
        public readonly Slot<float> Result = new();

        public Sigmoid()
        {
            Result.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            var v = Value.GetValue(context);
            var pow = Stretch.GetValue(context);
            Result.Value =  1f/(1+ MathF.Pow(MathF.E,pow * v));
        }
        
        [Input(Guid = "276596b1-9a24-48f9-9202-4658efe33d25")]
        public readonly InputSlot<float> Value = new();

        [Input(Guid = "35957f60-7f91-47cd-8ff1-e0079c31f295")]
        public readonly InputSlot<float> Stretch = new();
    }
}
