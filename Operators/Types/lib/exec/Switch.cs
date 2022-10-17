using System;
using System.Collections.Generic;
using SharpDX;
using T3.Core;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_e64f95e4_c045_400f_98ca_7c020ad46174
{
    public class Switch : Instance<Switch>
    {
        [Output(Guid = "9300b07e-977d-47b0-908e-c4b1e5e53a64", DirtyFlagTrigger = DirtyFlagTrigger.Always)]
        public readonly Slot<Command> Output = new();

        [Output(Guid = "044538A4-4499-4F8F-8843-D880677EE1E7")]
        public readonly Slot<int> Count = new();
        
        public Switch()
        {
            Output.UpdateAction = Update;
            Count.UpdateAction = UpdateCount;
        }

        private void UpdateCount(EvaluationContext context)
        {
            var commands = Commands.GetCollectedTypedInputs();
            if (commands == null)
            {
                Count.Value = 0;
                return;
            }

            Count.Value = commands.Count;
        }

        private void Update(EvaluationContext context)
        {
            var commands = Commands.GetCollectedTypedInputs();
            var index = Index.GetValue(context);

            if (commands.Count == 0 || index == -1)
            {
                Count.Value = 0;
                return;
            }
            
            // Do all
            _activeIndices.Clear();
            if (index == -2)
            {
                for (int i = 0; i < commands.Count; i++)
                {
                    _activeIndices.Add(i);
                    commands[i].GetValue(context); 
                }

                for (int i = 0; i < commands.Count; i++)
                {
                    _activeIndices.Add(i);
                    commands[i].Value?.RestoreAction?.Invoke(context);
                }
            }
            else
            {
                index %= commands.Count;
                if (index < 0)
                {
                    index += commands.Count;
                }
                
                _activeIndices.Add(index);
                commands[index].GetValue(context); 
                commands[index].Value?.RestoreAction?.Invoke(context);
            }

            Commands.LimitMultiInputInvalidationToIndices = OptimizeInvalidation.GetValue(context) 
                                                                ? _activeIndices 
                                                                : null;
            Count.Value = commands.Count;
        }

        private readonly List<int> _activeIndices = new();

        [Input(Guid = "988DD1B5-636D-4A78-9592-2C6601401CC1")]
        public readonly MultiInputSlot<Command> Commands = new();
        
        [Input(Guid = "00FD2794-567A-4F9B-A900-C2EBF9760764")]
        public readonly InputSlot<int> Index = new();
        
        [Input(Guid = "E896B269-D17E-417F-BE1F-2D6E9ADDAE91")]
        public readonly InputSlot<bool> OptimizeInvalidation = new();

        
    }
}