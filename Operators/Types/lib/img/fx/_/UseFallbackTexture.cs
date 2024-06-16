using SharpDX.Direct3D11;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_b470fdf9_ac0b_4eb9_9600_453b8c094e3f
{
    public class UseFallbackTexture : Instance<UseFallbackTexture>
    {
        [Output(Guid = "778f4eac-24ef-4e93-b864-39f150ab6cb2")]
        public readonly Slot<Texture2D> Output = new();

        public UseFallbackTexture()
        {
            Output.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            if (_fallback == null || Fallback.DirtyFlag.IsDirty)
            {
                _fallback = Fallback.GetValue(context);
            }
            else
            {
                Fallback.DirtyFlag.Clear();
            }
            
            var tex = TextureA.GetValue(context) ?? _fallback;

            Output.Value = tex;
        }

        private Texture2D _fallback=null;
        
        
        [Input(Guid = "91BFFBBA-B815-44D7-8F93-3238376935BF")]
        public readonly InputSlot<Texture2D> TextureA = new();
        
        [Input(Guid = "38B478FA-C431-4DC1-80EF-D6C53C90389E")]
        public readonly InputSlot<Texture2D> Fallback = new();
    }
}