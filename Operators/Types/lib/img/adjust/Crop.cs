using SharpDX.Direct3D11;
using T3.Core.DataTypes.Vector;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_a29cf1c8_d9cd_4a5d_b06c_597cbeb5b33d
{
    public class Crop : Instance<Crop>
    {
        [Output(Guid = "0f7a4421-97d7-48d0-8a99-a7cc84356be2")]
        public readonly Slot<Texture2D> Output = new();

        [Input(Guid = "a7fff052-42c2-40fb-938e-9fb9b6cfa591")]
        public readonly InputSlot<SharpDX.Direct3D11.Texture2D> Texture2d = new();

        [Input(Guid = "eff02108-2335-46b5-95ba-4235b9a26349")]
        public readonly InputSlot<Int2> LeftRight = new();

        [Input(Guid = "db4c2d41-c369-4182-83fd-cb04aa04ec76")]
        public readonly InputSlot<Int2> TopBottom = new();

        [Input(Guid = "3b62379a-de94-4be1-8471-357710ba14c3")]
        public readonly InputSlot<System.Numerics.Vector4> PaddingColor = new();

    }
}