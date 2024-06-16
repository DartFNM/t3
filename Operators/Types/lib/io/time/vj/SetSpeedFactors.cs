using SharpDX.Direct3D11;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_61dc8ba5_8aa6_4003_8d8c_8bce27f56a67
{
    public class SetSpeedFactors : Instance<SetSpeedFactors>
    {
        [Output(Guid = "8d7b0260-85d4-4c76-938f-4e027cc0e337")]
        public readonly Slot<T3.Core.DataTypes.Command> OutputCommand = new();

        [Output(Guid = "d23fcc88-cd97-437b-b0a4-0b17c087b563")]
        public readonly Slot<Texture2D> Output = new();

        
        [Input(Guid = "038fb60f-0ef9-45ec-839f-c533a93ad124")]
        public readonly MultiInputSlot<T3.Core.DataTypes.Command> Commands = new();
        
        [Input(Guid = "3ba30914-444c-43a9-b936-87d5d98898cd")]
        public readonly InputSlot<Texture2D> Texture = new();

        [Input(Guid = "c1d45c3a-1940-4c1a-aaf7-bd2e69fb2c9d")]
        public readonly InputSlot<float> SpeedFactorA = new();

        [Input(Guid = "c6cc12b7-96e6-406c-8e91-56fa70a241a2")]
        public readonly InputSlot<float> SpeedFactorB = new();

        [Input(Guid = "14a0ee21-309a-44cc-bc00-3d7d5594de01", MappedType = typeof(Modes))]
        public readonly InputSlot<int> ApplyAs = new();

        private enum Modes
        {
            NormalizedRates,
            Factor,
        }
    }
}

