using T3.Core.DataTypes.Vector;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;


namespace T3.Operators.Types.Id_4238439e_a6b4_4390_9984_e6ebf19c3a69
{
    public class ReprojectToUV : Instance<ReprojectToUV>
    {
        [Output(Guid = "7d2fc5fe-0e1c-4132-9322-e08b3638bf83")]
        public readonly Slot<SharpDX.Direct3D11.Texture2D> TextureOutput = new();

        
        [Input(Guid = "5ba52f22-0fe6-4316-a512-7577fcdff091")]
        public readonly InputSlot<T3.Core.DataTypes.MeshBuffers> Mesh = new();

        [Input(Guid = "eb4da1b0-f9c6-480d-a1a3-ac875cbf1037")]
        public readonly InputSlot<System.Numerics.Vector4> Color = new();

        [Input(Guid = "04e2fd86-3dbc-4718-9f3b-361dff3e49c8")]
        public readonly InputSlot<SharpDX.Direct3D11.Texture2D> Texture = new();
        
        [Input(Guid = "CB1254AB-4D68-41DB-A326-C5E34BB5D2F4")]
        public readonly InputSlot<object> CameraReference = new();

        [Input(Guid = "6ff4c0bd-f47c-48f2-a2bc-ba13f7cff3ce")]
        public readonly InputSlot<Int2> Resolution = new();

        [Input(Guid = "c4fff7ca-02d3-4337-b4e8-9c3074f98eb5")]
        public readonly InputSlot<System.Numerics.Vector4> ClearColor = new();

        [Input(Guid = "e52b254e-e13b-4df8-81d4-35867aeb188e")]
        public readonly InputSlot<SharpDX.DXGI.Format> TextureFormat = new();


        
    }
}

