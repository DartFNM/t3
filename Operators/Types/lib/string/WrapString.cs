using System.Text;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;
using T3.Core.Utils;

namespace T3.Operators.Types.Id_96ccea19_c37f_4ee4_8dd2_5abdb347f5a1
{
    public class WrapString : Instance<WrapString>
    {
        [Output(Guid = "83571519-ade1-4508-bf4c-c3d734cf5603")]
        public readonly Slot<string> Result = new();

        public WrapString()
        {
            Result.UpdateAction = Update;
        }

        private void Update(EvaluationContext context)
        {
            var str = InputText.GetValue(context);
            var mode = Mode.GetEnumValue<WrapLinesModes>(context);
            var wrapColumn = WrapColumn.GetValue(context);

            _stringBuilder.Clear();
            _stringBuilder.Append(str);
            InsertLineWraps(mode, _stringBuilder, 0, 0, wrapColumn);
            Result.Value = _stringBuilder.ToString();
        }


        private static void InsertLineWraps(WrapLinesModes lineWrap, StringBuilder stringBuilder, int insertPos, int insertLength, int wrapColumn)
        {
            if (lineWrap == WrapLinesModes.WrapAtCharacters)
            {
                var lookBackIndex = insertPos;
                while (lookBackIndex > 0 && stringBuilder[lookBackIndex] != '\n')
                {
                    lookBackIndex--;
                }

                var lineLength = insertPos - lookBackIndex + insertLength;
                if (lineLength > wrapColumn && insertPos > 0 && insertPos < stringBuilder.Length)
                {
                    stringBuilder[insertPos - 1] = '\n';
                    return;
                }
                
                var lookForwardIndex = insertPos;
                while (lookForwardIndex < stringBuilder.Length && stringBuilder[lookForwardIndex] != '\n')
                {
                    lookForwardIndex++;
                }

                if (lookForwardIndex - lookBackIndex > wrapColumn)
                {
                    stringBuilder[insertPos + insertLength - 1] = '\n';
                }
            }
            else if (lineWrap == WrapLinesModes.WrapAtWords)
            {
                int pos = 0;
                int currentLineLength = 0;
                int lastValidBreakPos = -1;
                while (pos < stringBuilder.Length)
                {
                    var c = stringBuilder[pos];
                    if (c == '\n') 
                    {
                        currentLineLength = 0;
                        lastValidBreakPos = -1;
                    }
                    
                    else if (c == ' ' || c == '.' || c == ',' || c == '/')
                    {
                        lastValidBreakPos = pos;
                        currentLineLength++;
                    }
                    else
                    {
                        currentLineLength++;
                    }

                    if (currentLineLength > wrapColumn && lastValidBreakPos != -1)
                    {
                        stringBuilder[lastValidBreakPos] = '\n';
                        pos = lastValidBreakPos;
                        lastValidBreakPos = -1;
                        currentLineLength = 0;
                    }
                    pos++;
                }
            }
            else if (lineWrap == WrapLinesModes.WrapToFillBlock)
            {
                int pos = 0;
                int currentLineLength = 0;
                int lastValidBreakPos = -1;
                
                while (pos < stringBuilder.Length)
                {
                    var c = stringBuilder[pos];
                    if (c == '\n')
                    {
                        stringBuilder[pos] = ' ';
                        //currentLineLength = 0;
                        lastValidBreakPos = pos;
                        currentLineLength++;
                    }
                    
                    else if (c == ' ' || c == '.' || c == ',' || c == '/')
                    {
                        lastValidBreakPos = pos;
                        currentLineLength++;
                    }
                    else
                    {
                        currentLineLength++;
                    }

                    if (currentLineLength > wrapColumn && lastValidBreakPos != -1)
                    {
                        stringBuilder[lastValidBreakPos] = '\n';
                        pos = lastValidBreakPos;
                        lastValidBreakPos = -1;
                        currentLineLength = 0;
                    }
                    pos++;
                }
            }            
            else if (lineWrap == WrapLinesModes.SolidBlock)
            {
                int pos = 0;
                int currentLineLength = 0;
                while (pos < stringBuilder.Length)
                {
                    var c = stringBuilder[pos];
                    
                    if (c == '\n')
                    {
                        stringBuilder.Remove(pos, 1);
                        continue;
                    }
                    
                    currentLineLength++;
                    pos++;

                    if (currentLineLength == wrapColumn)
                    {
                        stringBuilder.Insert(pos, '\n');
                        pos++;
                        currentLineLength = 0;
                    }
                }
            }
        }
        
        private readonly StringBuilder _stringBuilder = new();

        
        private enum WrapLinesModes
        {
            DontWrap,
            WrapAtWords,
            WrapAtCharacters,
            WrapToFillBlock,
            SolidBlock,
        }
        
        [Input(Guid = "85ad6b56-45eb-484e-b7d9-a93d52f8e54d")]
        public readonly InputSlot<string> InputText = new();

        [Input(Guid = "545629C0-C027-4E7C-888C-8E0589940D9D")]
        public readonly InputSlot<int> WrapColumn = new();
        
        [Input(Guid = "9a474693-b2f2-4293-bb52-2f9401a0c928", MappedType = typeof(WrapLinesModes))]
        public readonly InputSlot<int> Mode = new ();

        
    }
}