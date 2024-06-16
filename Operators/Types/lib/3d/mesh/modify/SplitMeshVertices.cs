using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_3f0f0c40_482d_4d79_a201_b4651a0cd3a8
{
    public class SplitMeshVertices : Instance<SplitMeshVertices>
    {

        [Output(Guid = "7873a4c0-04ff-41b5-bf5e-66ae745c3918")]
        public readonly Slot<T3.Core.DataTypes.MeshBuffers> Result = new();

        [Input(Guid = "22370faa-8381-4878-8653-2fe9297400da")]
        public readonly InputSlot<T3.Core.DataTypes.MeshBuffers> InputMesh = new();

        [Input(Guid = "308f12dc-a308-472a-923e-20f0a20d54db")]
        public readonly InputSlot<float> ShadeFlat = new();

        
        private enum Spaces
        {
            PointSpace,
            ObjectSpace,
            WorldSpace,
        }
        
        private enum Directions
        {
            WorldSpace,
            SurfaceNormal,
        }
    }
}

