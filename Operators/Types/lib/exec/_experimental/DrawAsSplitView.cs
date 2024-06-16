using T3.Core.DataTypes;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_f19a9234_cd23_4229_a794_aa9d97ad8027
{
    public class DrawAsSplitView : Instance<DrawAsSplitView>
    {
        [Output(Guid = "65456554-355b-41a3-893e-960d28113f53")]
        public readonly Slot<Command> Output = new();
        
        [Input(Guid = "a3929303-170b-496a-b8e0-fc5f604a0ec7")]
        public readonly MultiInputSlot<Command> Commands = new();

        [Input(Guid = "987bda72-6a6b-4216-9ecf-d87b7299553d")]
        public readonly InputSlot<string> Labels = new();

        [Input(Guid = "3cb0dfab-deaa-4ed4-ba45-ac63e886e212")]
        public readonly InputSlot<System.Numerics.Vector2> Stretch = new();

        [Input(Guid = "b1cdb551-0045-42d4-a6ba-fa8aa0f1f98f", MappedType = typeof(ViewModes))]
        public readonly InputSlot<int> Mode = new();
        
        
        private enum ViewModes
        {
            RepeatView,
            SliceView,
        }
    }
    
}

