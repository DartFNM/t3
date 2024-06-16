using System;
using T3.Core.Animation;
using T3.Core.Logging;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Interfaces;
using T3.Core.Operator.Slots;
using T3.Core.Utils;

namespace T3.Operators.Types.Id_0cd18cbb_e138_4b4b_a800_175fc39c61bf
{
    public class ConvertTime : Instance<ConvertTime>, IStatusProvider
    {
        [Output(Guid = "2ccb0dad-0d47-4169-9351-97b96e55ad26")]
        public readonly Slot<float> Result = new();
        
        public ConvertTime()
        {
            Result.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {

            var time = Time.GetValue(context);
            
            if (Playback.Current == null)
            {
                _lastErrorMessage = "Can't get BPM rate without value playback";
                return;
            }

            _lastErrorMessage = null;
            
            Result.Value = Mode.GetEnumValue<Modes>(context) switch
                               {
                                   Modes.BarsToSeconds => (float)Playback.Current.SecondsFromBars(time),
                                   Modes.SecondsToBars => (float)Playback.Current.BarsFromSeconds(time),
                                   _                   => throw new ArgumentOutOfRangeException()
                               };
        }

        private enum Modes
        {
            BarsToSeconds,
            SecondsToBars,
        }
        
        [Input(Guid = "DD9B7590-9D1F-4A3A-AFE7-FA37FEFD5798")]
        public readonly InputSlot<float> Time = new();
        
        [Input(Guid = "3AD320B9-D1BC-4BDB-B5C8-20CAA721621B", MappedType = typeof(Modes))]
        public readonly InputSlot<int> Mode = new();

        public IStatusProvider.StatusLevel GetStatusLevel()
        {
            return string.IsNullOrEmpty(_lastErrorMessage) ? IStatusProvider.StatusLevel.Success : IStatusProvider.StatusLevel.Warning;
        }

        public string GetStatusMessage()
        {
            return _lastErrorMessage;
        }

        private string _lastErrorMessage;
    }
}