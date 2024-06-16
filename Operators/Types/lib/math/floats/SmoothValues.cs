using System.Collections.Generic;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Utils;

namespace T3.Operators.Types.Id_50bf9e01_6912_4ab7_b233_660ce67bca25
{
    public class SmoothValues : Instance<SmoothValues>
    {
        [Output(Guid = "38FBB674-1EF9-47D0-BDEC-9E1E728F6D92")]
        public readonly Slot<List<float>> Result = new();

        public SmoothValues()
        {
            Result.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            var windowSize = WindowSize.GetValue(context).Clamp(1, 10);
            
            var list = Input.GetValue(context);
            if (list == null || list.Count == 0)
            {
                return;
            }

            if (Result.Value == null || Result.Value.Count != list.Count)
            {
                Result.Value = new List<float>();
            }
            
            Result.Value.Clear();

            for (var index = 0; index < list.Count; index++)
            {
                float sum=0;
                float count=0;
                SampleAtIndex(index, ref sum, ref count);
                
                for (var windowIndex = 0; windowIndex < windowSize; windowIndex++)
                {
                    SampleAtIndex(index + windowIndex, ref sum, ref count);
                }

                Result.Value.Add(count == 0 ? float.NaN : sum/count);
            }
            
            void SampleAtIndex(int index, ref float sum, ref float count)
            {
                if (index < 0 || index >= list.Count)
                    return;

                sum += list[index];
                count++;
            }
        }
        

        
        
        [Input(Guid = "1f88c433-fd51-4076-8f46-34b586f723f9")]
        public readonly InputSlot<List<float>> Input = new(new List<float>(20));

        [Input(Guid = "da418887-bb4d-4912-b448-13129078fc4c")]
        public readonly InputSlot<int> WindowSize = new ();
        
    }
}