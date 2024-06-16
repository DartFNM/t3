using System;
using System.Collections.Generic;
using System.Linq;
using T3.Core.Audio;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Utils;
using T3.Operators.Types.Id_03477b9a_860e_4887_81c3_5fe51621122c;

namespace T3.Operators.Types.Id_cda108a1_db4f_4a0a_ae4d_d50e9aade467
{
    public class PlaybackFFT : Instance<PlaybackFFT>
    {
        [Output(Guid = "2d0f5713-9620-4bc7-a792-a7b8e622554a", DirtyFlagTrigger = DirtyFlagTrigger.Animated)]
        public readonly Slot<List<float>> Result = new(new List<float>(256));

        public PlaybackFFT()
        {
            Result.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            List<float> bins = default;
            var mode = (AudioReaction.InputModes)InputBand.GetValue(context).Clamp(0, Enum.GetNames(typeof(AudioReaction.InputModes)).Length - 1);
            bins = mode switch
                       {
                           AudioReaction.InputModes.RawFft
                               => AudioAnalysis.FftGainBuffer == null ? EmptyList : AudioAnalysis.FftGainBuffer.ToList(),
                           AudioReaction.InputModes.NormalizedFft
                               => AudioAnalysis.FftNormalizedBuffer == null
                                      ? EmptyList
                                      : AudioAnalysis.FftNormalizedBuffer.ToList(),
                           AudioReaction.InputModes.FrequencyBands
                               => AudioAnalysis.FrequencyBands == null ? EmptyList : AudioAnalysis.FrequencyBands.ToList(),
                           AudioReaction.InputModes.FrequencyBandsPeaks
                               => AudioAnalysis.FrequencyBandPeaks == null
                                      ? EmptyList
                                      : AudioAnalysis.FrequencyBandPeaks.ToList(),
                           AudioReaction.InputModes.FrequencyBandsAttacks
                               => AudioAnalysis.FrequencyBandAttacks == null
                                      ? EmptyList
                                      : AudioAnalysis.FrequencyBandAttacks.ToList(),
                           _ => bins
                       };

            Result.Value = bins;
        }

        private static readonly List<float> EmptyList = new();

        [Input(Guid = "66CB16D6-1506-4783-925D-3CDF62602812", MappedType = typeof(AudioReaction.InputModes))]
        public readonly InputSlot<int> InputBand = new();
    }
}