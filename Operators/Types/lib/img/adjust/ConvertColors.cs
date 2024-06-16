using SharpDX.Direct3D11;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_d5516087_f7dd_44d4_a7e1_3c18767de921
{
    public class ConvertColors : Instance<ConvertColors>
    {
        [Output(Guid = "49e4972f-e360-4bc3-b780-032d5e985540")]
        public readonly Slot<Texture2D> Output = new();

        [Input(Guid = "dd2f08a9-a539-41e9-aa1e-f8ad64fb8d29")]
        public readonly InputSlot<SharpDX.Direct3D11.Texture2D> Texture2d = new();

        [Input(Guid = "ffcba423-04e2-4bc6-b3bf-fbc3b15c84b8", MappedType = typeof(Modes))]
        public readonly InputSlot<int> Mode = new();

        [Input(Guid = "c11ad183-0f0c-46cb-b543-8f39cc707427")]
        public readonly InputSlot<bool> GenerateMipmaps = new();

        [Input(Guid = "caa374cf-d07a-4af9-89a0-42597e11a6ff")]
        public readonly InputSlot<SharpDX.DXGI.Format> OutputFormat = new();

        private enum Modes
        {
            RgbToOKLab,
            OKLabToRgb,
            RgbToLCh,
            LChToRgb,
        }
    }
}