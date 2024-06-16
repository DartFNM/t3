using System.Numerics;
using SharpDX.Direct3D11;
using T3.Core.DataTypes;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Rendering;
using T3.Core.Resource;
using T3.Core.Utils;

namespace T3.Operators.Types.Id_74cbfce0_f8b8_46a1_b5d6_38477d4eec99
{
    public class SetFog : Instance<SetFog>
    {
        [Output(Guid = "7c2134d1-45c6-4dc7-b591-a4a5113f5747")]
        public readonly Slot<Command> Output = new();

        public SetFog()
        {
            Output.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            Utilities.Dispose(ref _parameterBuffer);
            ResourceManager.SetupConstBuffer(new FogSettings.FogParameters
                                                 {
                                                     Bias = Bias.GetValue(context),
                                                     Distance = Distance.GetValue(context),
                                                     Color = Color.GetValue(context)
                                                 }, ref _parameterBuffer);
            
            var previousParameters = context.FogParameters;
            context.FogParameters = _parameterBuffer;

            // Evaluate sub tree
            Command.GetValue(context);

            context.FogParameters = previousParameters;
        }

        private Buffer _parameterBuffer;

        [Input(Guid = "AFADB109-37B2-49AA-8C32-627CC35FD574")]
        public readonly InputSlot<Command> Command = new();

        [Input(Guid = "7A331539-33EA-48B9-8930-DAF93DD33D7C")]
        public readonly InputSlot<float> Distance = new();

        [Input(Guid = "53479387-1C04-4FBC-B093-043075495E10")]
        public readonly InputSlot<float> Bias = new();

        [Input(Guid = "EF8C86EE-16C0-446E-9CE0-C6342ADBF32A")]
        public readonly InputSlot<Vector4> Color = new();
    }
}