using T3.Core.DataTypes;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_4f0506ac_6a72_4e35_96f0_0f331d8f6cca
{
    public class _VisLenseFxZone : Instance<_VisLenseFxZone>
    {
        [Output(Guid = "0b911435-15e6-417c-bc27-390310c8e7ad")]
        public readonly Slot<Command> Output = new();

        [Input(Guid = "2aa0b998-c8a0-4017-9002-23bd7b29d042")]
        public readonly InputSlot<System.Numerics.Vector2> InnerFxZone = new();

        [Input(Guid = "5e718a12-e4d3-42e8-bc9b-4e6a849812dc")]
        public readonly InputSlot<System.Numerics.Vector2> EdgeFxZone = new();

        [Input(Guid = "b4aec340-9a7b-4f7b-b33c-e7ae2f61cf2b")]
        public readonly InputSlot<System.Numerics.Vector2> MatteBoxZone = new();


    }
}

