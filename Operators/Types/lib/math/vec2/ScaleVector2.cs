using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_4bbc6fac_789d_496e_9833_a0af78c31c98
{
    public class ScaleVector2 : Instance<ScaleVector2>
    {
        [Output(Guid = "b45d4180-d43a-4b0f-8218-6d4d7f5c56a8")]
        public readonly Slot<System.Numerics.Vector2> Result = new();

        
        public ScaleVector2()
        {
            Result.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            var a = A.GetValue(context);
            var b = B.GetValue(context);
            var u = UniformScale.GetValue(context);
            //Result.Value = new System.Numerics.Vector2(a.X * b.X, a.Y * b.Y);
            Result.Value = a * b * u;
        }
        
        [Input(Guid = "D7BF8B1C-B654-451B-AC94-A2F71DB3DA35")]
        public readonly InputSlot<System.Numerics.Vector2> A = new();

        [Input(Guid = "C34AE94C-228D-45E4-9E9A-EC2799953444")]
        public readonly InputSlot<System.Numerics.Vector2> B = new();
        
        [Input(Guid = "927443FE-4338-462A-BF78-90A08F77467B")]
        public readonly InputSlot<float> UniformScale = new();
        
    }
}
