using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Operators.Types.Id_3af25959_fd3f_4608_b521_5860d82554df;

namespace T3.Operators.Types.Id_dd7fa7ee_266a_43c8_b29f_3357488b26be
{
    public class RandomString : Instance<RandomString>
    {
        [Output(Guid = "3a769380-1586-4d7f-a881-e509d5c14c1b")]
        public readonly Slot<string> Fragments = new();

        [Input(Guid = "4e36b984-43ff-447b-8e1d-a099fefd4d74")]
        public readonly InputSlot<float> Rate = new();

        [Input(Guid = "9b705b88-a644-498a-b831-5d0243e01c41", MappedType = typeof(MockStrings.Categories))]
        public readonly InputSlot<int> Category = new();


    }
}

