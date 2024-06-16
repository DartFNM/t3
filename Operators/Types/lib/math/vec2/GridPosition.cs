using T3.Core.DataTypes.Vector;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Utils;

namespace T3.Operators.Types.Id_310e174e_ea52_4c54_90e6_72dc8320118a
{
    public class GridPosition : Instance<GridPosition>
    {
        [Output(Guid = "bf470007-4c7c-4915-a402-98bf6cead2dc")]
        public readonly Slot<System.Numerics.Vector2> Position = new();

        [Output(Guid = "D11A64D7-F8D3-4A97-AD51-2B80200A6E1A")]
        public readonly Slot<System.Numerics.Vector2> Size = new();
        
        public GridPosition()
        {
            Position.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            var index = Index.GetValue(context);
            var size = RasterSize.GetValue(context);
            var columns = size.Width.Clamp(1, 10000);
            var rows = size.Height.Clamp(1, 10000);
            
            var row = index / columns;
            var column = index - (row *  columns);
            
            var aspectRatio = (float)context.RequestedResolution.Width / context.RequestedResolution.Height;
            var sizeValue = new System.Numerics.Vector2( 1f/ columns,
                                                         1f / rows  );
            
            var x = ((float)column  / columns - 0.5f) * aspectRatio * 2 + sizeValue.X * aspectRatio;
            var y = (((float)(rows- row -1) / (float)rows) - 0.5f ) * 2 + sizeValue.Y;
            
            Size.Value = sizeValue;
            Position.Value = new System.Numerics.Vector2(x, y);
        }
        
        [Input(Guid = "854731fb-bbc9-48e7-9bcb-45bc53340945")]
        public readonly InputSlot<System.Numerics.Vector2> A = new();

        [Input(Guid = "2FA305B7-42FC-44FA-BFCF-219916F93EEF")]
        public readonly InputSlot<Int2> RasterSize = new();
        
        [Input(Guid = "938103EE-65BD-4F5E-AE5E-5635DC53E3E6")]
        public readonly InputSlot<int> Index = new();
    }
}
