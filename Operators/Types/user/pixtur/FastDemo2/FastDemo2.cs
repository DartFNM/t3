using SharpDX.Direct3D11;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_20a10411_8a39_4ca3_85db_8f34537f66b8
{
    public class FastDemo2 : Instance<FastDemo2>
    {
        [Output(Guid = "7a5725b9-ba46-4e1b-a698-7ff78ae6cd1b")]
        public readonly Slot<Texture2D> ImgOutput = new();


    }
}

