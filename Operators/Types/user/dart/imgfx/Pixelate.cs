using System.Numerics;
using SharpDX.Direct3D11;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_f906b3f6_515f_4856_beb8_a5b0bf0b8715
{
    public class Pixelate : Instance<Pixelate>
    {
        [Output(Guid = "2753ff7e-064e-4572-965a-52855b34bdf5")]
        public readonly Slot<Texture2D> Output = new Slot<Texture2D>();

        [Input(Guid = "fe87b984-d039-4b9c-8ef9-960c50fcc4e5")]
        public readonly InputSlot<SharpDX.Direct3D11.Texture2D> Texture2d = new InputSlot<SharpDX.Direct3D11.Texture2D>();

        [Input(Guid = "11bc5f73-a8fa-473c-bb55-d1472ea28712")]
        public readonly InputSlot<System.Numerics.Vector2> Size = new InputSlot<System.Numerics.Vector2>();

        [Input(Guid = "520bacd4-6d13-4625-b37e-58cd0e948e9e")]
        public readonly InputSlot<System.Numerics.Vector2> Shift = new InputSlot<System.Numerics.Vector2>();

    }
}