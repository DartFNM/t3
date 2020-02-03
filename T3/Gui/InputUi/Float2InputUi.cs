﻿using System.Linq;
using System.Numerics;
using ImGuiNET;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using T3.Core;
using T3.Core.Animation;
using T3.Core.Operator;
using T3.Gui.Interaction;


namespace T3.Gui.InputUi
{
    public class Float2InputUi : InputValueUi<Vector2>
    {
        public override bool IsAnimatable => true;

        public override IInputUi Clone()
        {
            return new Float2InputUi()
                   {
                       _max = _max,
                       _min = _min,
                       _scale = _scale,
                       InputDefinition = InputDefinition,
                       Parent = Parent,
                       PosOnCanvas = PosOnCanvas,
                       Relevancy = Relevancy,
                       Size = Size
                   };
        }

        protected override InputEditStateFlags DrawEditControl(string name, ref Vector2 float2Value)
        {
            float2Value.CopyTo(_components);
            var inputEditState = VectorValueEdit.Draw(_components, _min, _max, _scale);
            float2Value = new Vector2(_components[0], _components[1]);

            return inputEditState;
        }
        
        protected override void DrawReadOnlyControl(string name, ref Vector2 float2Value)
        {
            ImGui.InputFloat2(name, ref float2Value, "%f", flags: ImGuiInputTextFlags.ReadOnly);
        }

        protected override string GetSlotValueAsString(ref Vector2 float2Value)
        {
            // This is a stub of value editing. Sadly it's very hard to get
            // under control because of styling issues and because in GraphNodes
            // The op body captures the mouse event first.
            //
            //SingleValueEdit.Draw(ref floatValue,  -Vector2.UnitX);

            return string.Format(T3Ui.FloatNumberFormat, float2Value);
        }

        protected override void DrawAnimatedValue(string name, InputSlot<Vector2> inputSlot, Animator animator)
        {
            double time = EvaluationContext.GlobalTime;
            var curves = animator.GetCurvesForInput(inputSlot).ToArray();
            if (curves.Length < _components.Length)
            {
                DrawReadOnlyControl(name, ref inputSlot.Value);
                return;
            }

            for (var index = 0; index < _components.Length; index++)
            {
                _components[index] = (float)curves[index].GetSampledValue(time);
            }

            var inputEditState = VectorValueEdit.Draw(_components, _min, _max, _scale);
            if (inputEditState == InputEditStateFlags.Nothing)
                return;

            for (var index = 0; index < _components.Length; index++)
            {
                var key = curves[index].GetV(time) ?? new VDefinition { U = time };
                key.Value = _components[index];
                curves[index].AddOrUpdateV(time, key);
            }
        }

        public override void DrawSettings()
        {
            base.DrawSettings();

            ImGui.DragFloat("Min", ref _min);
            ImGui.DragFloat("Max", ref _max);
            ImGui.DragFloat("Scale", ref _scale);
        }

        public override void Write(JsonTextWriter writer)
        {
            base.Write(writer);

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (_min != DefaultMin)
                writer.WriteValue("Min", _min);

            if (_max != DefaultMax)
                writer.WriteValue("Max", _max);

            if (_scale != DefaultScale)
                writer.WriteValue("Scale", _scale);
            // ReSharper enable CompareOfFloatsByEqualityOperator
        }

        public override void Read(JToken inputToken)
        {
            base.Read(inputToken);

            _min = inputToken["Min"]?.Value<float>() ?? DefaultMin;
            _max = inputToken["Max"]?.Value<float>() ?? DefaultMax;
            _scale = inputToken["Scale"]?.Value<float>() ?? DefaultScale;
        }

        private float _min = DefaultMin;
        private float _max = DefaultMax;
        private float _scale = DefaultScale;

        private const float DefaultScale = 0.01f;
        private const float DefaultMin = -9999999f;
        private const float DefaultMax = 9999999f;
        
        private static float[] _components = new float[2];
    }
}