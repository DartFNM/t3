using T3.Core.DataTypes;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_037c89b0_f5d3_4509_b574_c34fa8ec21f3
{
    public class CrossLinePoints : Instance<CrossLinePoints>
    {
        [Output(Guid = "8dfd5a8b-ee85-4e95-ab54-622e2478d0ab")]
        public readonly Slot<BufferWithViews> OutBuffer = new();

        [Input(Guid = "b4ba04f3-f09b-4b73-9499-b4bbed0eaf01")]
        public readonly InputSlot<float> LengthFactor = new();


    }
}

