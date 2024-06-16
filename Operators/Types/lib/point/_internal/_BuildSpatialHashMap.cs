using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_f61ceb9b_74f8_4883_88ea_7e6c35b63bbd
{
    public class _BuildSpatialHashMap : Instance<_BuildSpatialHashMap>
    {
        [Output(Guid = "59d09aa6-051c-4906-9f32-f65e66979c56")]
        public readonly Slot<T3.Core.DataTypes.Command> Update = new ();
        
        [Output(Guid = "b4505f1e-ab0e-45be-8e46-8e3b37ec59ec")]
        public readonly Slot<SharpDX.Direct3D11.ShaderResourceView> CellPointIndices = new();

        [Output(Guid = "6c026a5f-a724-4240-bb96-077d65266f66")]
        public readonly Slot<SharpDX.Direct3D11.ShaderResourceView> PointCellIndices = new();

        [Output(Guid = "fb96e3d2-9a0f-466a-9b1d-997a4aa3e625")]
        public readonly Slot<SharpDX.Direct3D11.ShaderResourceView> HashGridCells = new();

        [Output(Guid = "13f0d2c2-a18b-457b-a3cf-cfd0c755b9a9")]
        public readonly Slot<SharpDX.Direct3D11.ShaderResourceView> CellPointCounts = new();

        [Output(Guid = "eeb282ee-ad73-471c-89ab-ae7cc8de6b15")]
        public readonly Slot<SharpDX.Direct3D11.ShaderResourceView> CellRangeIndices = new();



        [Input(Guid = "4bae9eaa-42d8-4c1c-8776-3abebcce20e2")]
        public readonly InputSlot<T3.Core.DataTypes.BufferWithViews> PointsA_ = new();

        [Input(Guid = "22f9737b-b3b4-4455-a4ec-8d61ab7abc6c")]
        public readonly InputSlot<float> CellSize = new();
    }
}

