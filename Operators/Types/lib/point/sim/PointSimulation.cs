using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_10507c42_1240_47cc_9569_5e3f1c733e99
{
    public class PointSimulation : Instance<PointSimulation>
    {

        [Output(Guid = "5bc395fd-1e77-402f-88da-b9727f3c1b98")]
        public readonly Slot<T3.Core.DataTypes.BufferWithViews> OutBuffer = new();

        [Input(Guid = "76263857-27ea-40f5-856f-983c6ddcbfe8")]
        public readonly InputSlot<T3.Core.DataTypes.BufferWithViews> GPoints = new();

        [Input(Guid = "79080698-1097-4178-b7da-7d10fd86be28")]
        public readonly InputSlot<float> MixOriginal = new();

        [Input(Guid = "0954b214-dd1f-40fd-bebe-29f74a8f5585")]
        public readonly InputSlot<bool> Reset = new();

        [Input(Guid = "5b9dcd2e-36b6-46f3-bded-0cba148cf628")]
        public readonly InputSlot<bool> Update = new();

        [Input(Guid = "2f883ce2-421a-45f5-8de2-9e05d984b551")]
        public readonly InputSlot<int> MinCapacity = new();
    }
}

