using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Utils;

namespace T3.Operators.Types.Id_acdd78b1_4e66_4fd0_a36b_5318670fefd4
{
    public class ChangeCase : Instance<ChangeCase>
    {
        [Output(Guid = "ecf66a1e-45e5-4e0c-ac9e-a784a9339153")]
        public readonly Slot<string> Result = new();

        public ChangeCase()
        {
            Result.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            var str = InputText.GetValue(context);
            var mode = Mode.GetEnumValue<Modes>(context);
            switch (mode)
            {
                case Modes.ToUpperCase:
                    Result.Value = str?.ToUpperInvariant();
                    break;
                case Modes.ToLowerCase:
                    Result.Value = str?.ToLowerInvariant();
                    break;
            }
        }

        [Input(Guid = "041C98B6-4450-46D7-9DAE-C9030C88B9E6")]
        public readonly InputSlot<string> InputText = new();

        [Input(Guid = "8BD38031-DE22-40A8-9B6D-A241B2FCD7F2", MappedType = typeof(Modes))]
        public readonly InputSlot<int> Mode = new ();


        
        private enum Modes
        {
            ToUpperCase,
            ToLowerCase,
        }
    }
}