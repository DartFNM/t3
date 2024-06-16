using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_fcdc3089_2df5_467b_841b_7745efaf13db 
{
    public class StringBuilder : Instance<StringBuilder>
    {
        [Output(Guid = "DEB52613-01FC-428D-B2D1-4BE7B6767FAD")]
        public readonly Slot<System.Text.StringBuilder> Builder = new();

        
        public StringBuilder()
        {
            
            Builder.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            var initialString = InitialString.GetValue(context);
            var needsReset = ClearTrigger.GetValue(context);
            
            if (!_initialized)
            {
                needsReset = true;
                _initialized = true;

            }
            
            if (needsReset)
            {
                _builder.Clear();
                _builder.Append(initialString);
            }
            
            Builder.Value = _builder;
        }

        private bool _initialized = false;
        private readonly System.Text.StringBuilder _builder = new();

        [Input(Guid = "F5C0B04E-8C4E-43BA-AE19-79F80C0F830D")]
        public readonly InputSlot<bool> ClearTrigger = new();
        
        [Input(Guid = "85F3D00C-005D-4C2F-BF41-F5B5C672F286")]
        public readonly InputSlot<string> InitialString = new();

    }
}
