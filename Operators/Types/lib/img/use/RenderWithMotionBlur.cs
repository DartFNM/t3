using SharpDX.Direct3D11;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_0989e44b_f543_4b9a_a08e_5058d0032259
{
    public class RenderWithMotionBlur : Instance<RenderWithMotionBlur>
    {
        [Output(Guid = "351ecd6a-1b91-49c1-8497-16e115941e63")]
        public readonly Slot<Texture2D> Output = new();


        [Input(Guid = "a341c7d8-b324-4b5e-82e4-b9fae958e913")]
        public readonly InputSlot<Texture2D> Texture = new();

        [Input(Guid = "27f2bbbd-a1d6-4489-9bb4-f856872ff5de")]
        public readonly InputSlot<int> Passes = new();

        [Input(Guid = "5305a99b-c9ee-41d8-b3e8-918b17ed2107")]
        public readonly InputSlot<float> Strength = new();

    }
}

