﻿using System.Collections.Generic;
using System.Diagnostics;
using ImGuiNET;
using T3.Core.DataTypes;
using T3.Core.Operator;
using T3.Core.Operator.Slots;

namespace T3.Gui.OutputUi
{
    public class StructuredListOutputUi : OutputUi<StructuredList>
    {
        public override IOutputUi Clone()
        {
            return new StringListOutputUi()
                       {
                           OutputDefinition = OutputDefinition,
                           PosOnCanvas = PosOnCanvas,
                           Size = Size
                       };
        }
        
        protected override void DrawTypedValue(ISlot slot)
        {
            if (slot is Slot<StructuredList> typedSlot)
            {
                var list = typedSlot.Value;
                if (list == null)
                {
                    ImGui.Text("NULL?");
                }
                else
                {
                    TableView.TableList.Draw(typedSlot.Value);
                }
            }
            else
            {
                Debug.Assert(false);
            }
        }
    }
}