using System.Numerics;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_4af1080c_3133_4070_a440_4cf2f4cc10a1
{
    public class Vec3Distance : Instance<Vec3Distance>
    {
        [Output(Guid = "14D4FD70-153B-4AD2-B068-71A29427FBF4")]
        public readonly Slot<float> Result = new();

        public Vec3Distance()
        {
            Result.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            Result.Value = Vector3.Distance(Input1.GetValue(context), Input2.GetValue(context));
        }


        [Input(Guid = "eecb6054-ecb8-4b22-a685-6740ed1cfe5c")]
        public readonly InputSlot<Vector3> Input1 = new();

        [Input(Guid = "c4094f11-d93d-4497-b1ce-1faa8bc1d1b0")]
        public readonly InputSlot<Vector3> Input2 = new();
    }
}
