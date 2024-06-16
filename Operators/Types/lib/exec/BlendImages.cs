using T3.Core.DataTypes.Vector;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_48781d5a_d67f_4b9f_8554_35185ddb6c5c
{
    public class BlendImages : Instance<BlendImages>
    {

        [Output(Guid = "83ad8874-210d-461f-b7ce-dfd7ff6338f9")]
        public readonly Slot<SharpDX.Direct3D11.Texture2D> OutputImage = new();

        [Input(Guid = "f1888e3b-edf6-409c-9cda-8c97fb18c38e")]
        public readonly InputSlot<float> BlendFraction = new();

        [Input(Guid = "12a875e2-89c8-4a16-91ec-9f9ac431f10c")]
        public readonly MultiInputSlot<SharpDX.Direct3D11.Texture2D> Input = new();

        [Input(Guid = "48e4a15f-7806-4f10-b7b5-bb383e480d59")]
        public readonly InputSlot<Int2> Resolution = new();


    }
}

