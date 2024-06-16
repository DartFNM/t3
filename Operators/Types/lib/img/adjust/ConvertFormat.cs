using SharpDX.Direct3D11;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_e1cd1cdf_3982_4bb3_b080_9f0a851566d7
{
    public class ConvertFormat : Instance<ConvertFormat>
    {
        [Output(Guid = "8acb5759-a93a-4f45-a19b-99e24792fe19")]
        public readonly Slot<Texture2D> Output = new();

        [Input(Guid = "33b6a702-2452-45d4-b5f7-7ff9f66940a6")]
        public readonly InputSlot<SharpDX.Direct3D11.Texture2D> Texture2d = new();

        [Input(Guid = "3f7b713d-2808-4312-87b4-707cb891b567")]
        public readonly InputSlot<SharpDX.DXGI.Format> Format = new();

        [Input(Guid = "88623684-a5e4-4415-8458-648761e834e1")]
        public readonly InputSlot<bool> GenerateMipMaps = new();

        [Input(Guid = "8686d1c3-c5a5-4b4a-b30f-95a1cfd0dc90")]
        public readonly InputSlot<bool> Enable = new();

        [Input(Guid = "7e308e6d-fcff-46b2-a6d7-460edb33ef80")]
        public readonly InputSlot<float> ScaleFactor = new();

    }
}