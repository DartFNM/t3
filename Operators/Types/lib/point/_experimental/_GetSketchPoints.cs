using System.Collections.Generic;
using T3.Core.DataTypes;
using T3.Core.Logging;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Operators.Types.Id_b238b288_6e9b_4b91_bac9_3d7566416028;

// ReSharper disable RedundantNameQualifier

namespace T3.Operators.Types.Id_271e397e_051c_473f_968f_a2251fed65d1
{
    public class _GetSketchPoints : Instance<_GetSketchPoints>
    {
        [Output(Guid = "9b1adfd0-2a94-438e-a0b9-6f6ae69f5690")]
        public readonly Slot<StructuredList> PointList = new();

        [Output(Guid = "FB4F4AC1-696C-4606-B479-ED8DADD166D3")]
        public readonly Slot<float> DistanceToCurrentTime = new();

        public _GetSketchPoints()
        {
            DistanceToCurrentTime.UpdateAction = Update;
            PointList.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            if (Pages.GetValue(context) is not List<_SketchImpl.Page> orderedPages)
            {
                Log.Warning("Can't get pages from sketch implementation", this);
                PointList.Value = _emptyList;
                DistanceToCurrentTime.Value = 10000;
                return;
            }

            var pageIndex = PageIndex.GetValue(context);
            var useGetOnionSkin = GetOnionSkin.GetValue(context);

            if (useGetOnionSkin)
            {
                var pageIndexOffset = pageIndex;
                if (pageIndexOffset == 0)
                {
                    PointList.Value = _emptyList;
                    return;
                }

                var found = false;
                for (var index = 0; index < orderedPages.Count; index++)
                {
                    var p = orderedPages[index];
                    if (p.Time > context.LocalTime - 0.05f)
                    {
                        if (pageIndexOffset < 0)
                        {
                            pageIndex = index + pageIndexOffset;
                        }
                        else
                        {
                            var index1 = (p.Time > context.LocalTime + 0.05f) ? 1 : 0;
                            pageIndex = index + pageIndexOffset- index1;
                        }

                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    pageIndex = orderedPages.Count + pageIndexOffset;
                }
            }

            var isValidIndex = pageIndex >= 0 && pageIndex < orderedPages.Count;
            if (!isValidIndex || orderedPages[pageIndex] == null)
            {
                PointList.Value = _emptyList;
                DistanceToCurrentTime.Value = 10000;
                return;
            }

            var activePage = orderedPages[pageIndex];
            PointList.Value = activePage.PointsList;
            DistanceToCurrentTime.Value = (float)(context.LocalTime - activePage.Time);
        }

        private static readonly StructuredList<Point> _emptyList = new(1);

        [Input(Guid = "C7C71DE4-A9A8-4194-B490-AB48004565D1")]
        public readonly InputSlot<object> Pages = new();

        [Input(Guid = "1b8d9a00-f337-431e-bab3-24d45ed9d191")]
        public readonly InputSlot<int> PageIndex = new();

        [Input(Guid = "DDBAC6AB-E547-4955-9346-680B2C5BF8A4")]
        public readonly InputSlot<bool> GetOnionSkin = new();
    }
}