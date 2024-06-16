using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_0e1d5f4b_3ba0_4e71_aa26_7308b6df214d
{
    public class CountInt : Instance<CountInt>
    {
        [Output(Guid = "2E172F90-3995-4B16-AF33-9957BE07323B", DirtyFlagTrigger = DirtyFlagTrigger.Animated)]
        public readonly Slot<int> Result = new();

        public CountInt()
        {
            Result.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            if (!_initialized || TriggerReset.GetValue(context))
            {
                Result.Value = DefaultValue.GetValue(context);
                _initialized = true;
            }

            var triggeredIncrement = TriggerIncrement.GetValue(context);
            var triggeredDecrement = TriggerDecrement.GetValue(context);

            var notChanged = triggeredIncrement == _lastIncrementTrigger && triggeredDecrement == _lastDecrementTrigger;
            if (OnlyCountChanges.GetValue(context) && notChanged)
                return;

            _lastIncrementTrigger = triggeredIncrement;
            _lastDecrementTrigger = triggeredDecrement;

            var delta = Delta.GetValue(context);
            if (triggeredIncrement)
            {
                Result.Value += delta;
            }
            else if (triggeredDecrement)
            {
                Result.Value -= delta;
            }

            var modulo = Modulo.GetValue(context);
            if (modulo != 0)
            {
                Result.Value %= modulo;
            }
        }

        private bool _initialized;
        private bool _lastIncrementTrigger;
        private bool _lastDecrementTrigger;

        [Input(Guid = "bfd95809-61d2-49eb-85d4-ff9e36b2d158")]
        public readonly InputSlot<bool> TriggerIncrement = new();
        
        [Input(Guid = "6EBE2842-A8FC-4800-8296-C8664C804E3C")]
        public readonly InputSlot<bool> TriggerDecrement = new();
        
        [Input(Guid = "01027ce6-f4ca-44b6-a8ec-e4ab96280864")]
        public readonly InputSlot<bool> TriggerReset = new();

        [Input(Guid = "518A8BD6-D830-4F73-AC83-49BE2FD4B09D")]
        public readonly InputSlot<bool> OnlyCountChanges = new();

        [Input(Guid = "ABE64676-CCF7-4163-B4DA-26D8B7179AF4")]
        public readonly InputSlot<int> Delta = new();

        [Input(Guid = "11F9CDB5-84FC-4413-8CA7-77E12047F521")]
        public readonly InputSlot<int> DefaultValue = new();
        
        [Input(Guid = "2FF3D674-90D7-4C8F-8551-AAD9992540DB")]
        public readonly InputSlot<int> Modulo = new();

    }
}