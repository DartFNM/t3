using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_87915d7b_f2aa_45da_80f9_bd1f6033d387
{
    public class SimCentricalOffset : Instance<SimCentricalOffset>
    {

        [Output(Guid = "22ac99e2-182d-4a14-b64d-2a27f39be88b")]
        public readonly Slot<T3.Core.DataTypes.BufferWithViews> OutBuffer = new();

        [Input(Guid = "9180eb49-efae-4305-b269-04314210e1f2")]
        public readonly InputSlot<T3.Core.DataTypes.BufferWithViews> GPoints = new();

        [Input(Guid = "129f21fb-3206-4b0a-b455-485f1c11bd83")]
        public readonly InputSlot<System.Numerics.Vector3> Center = new();

        [Input(Guid = "cbcfcceb-309b-4a25-856a-cb4eb3798cf4")]
        public readonly InputSlot<float> MaxAcceleration = new();

        [Input(Guid = "02f34a62-bd83-47ac-962a-f1af6a92f0b8")]
        public readonly InputSlot<float> Amount = new();

        [Input(Guid = "93cae017-8bf6-44c8-9b45-8b9a4e64b9bd")]
        public readonly InputSlot<float> DecayExponent = new();

        [Input(Guid = "894b2978-63f5-4f0f-8e15-03fc21c9532b")]
        public readonly InputSlot<T3.Core.Operator.GizmoVisibility> ShowGizmo = new();
        
        private enum Modes {
            Legacy,
            EncodeInRotation,
        }
    }
}

