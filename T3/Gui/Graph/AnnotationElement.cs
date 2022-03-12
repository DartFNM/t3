﻿using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using System.Numerics;
using T3.Core.IO;
using T3.Core.Operator;
using T3.Gui.Commands;
using T3.Gui.Graph.Interaction;
using T3.Gui.InputUi;
using T3.Gui.Selection;
using T3.Gui.Styling;
using T3.Gui.TypeColors;
using T3.Gui.UiHelpers;
using UiHelpers;

namespace T3.Gui.Graph
{
    /// <summary>
    /// Draws an AnnotationElement and handles its interaction
    /// </summary>
    static class AnnotationElement
    {
        private static Color _backgroundColor = new Color(0, 0, 0, 0.2f);
        private static Color _backgroundColorHover = new Color(0, 0, 0, 0.4f);
        
        internal static void Draw(Annotation annotation)
        {
            ImGui.PushID(annotation.Id.GetHashCode());
            {
                _lastScreenRect = GraphCanvas.Current.TransformRect(new ImRect(annotation.PosOnCanvas, annotation.PosOnCanvas + annotation.Size));
                var titleSize = annotation.Size;
                titleSize.Y = MathF.Min(titleSize.Y, 20);

                var lastClickableRect = GraphCanvas.Current.TransformRect(new ImRect(annotation.PosOnCanvas, annotation.PosOnCanvas + titleSize));

                _isVisible = ImGui.IsRectVisible(_lastScreenRect.Min, _lastScreenRect.Max);

                if (!_isVisible)
                    return;

                var drawList = GraphCanvas.Current.DrawList;
                
                // Resize indicator
                {
                    ImGui.SetMouseCursor(ImGuiMouseCursor.ResizeNWSE);
                    ImGui.SetCursorScreenPos(_lastScreenRect.Max - new Vector2(10, 10));
                    ImGui.Button("##resize", new Vector2(10, 10));
                    if (ImGui.IsItemActive() && ImGui.IsMouseDragging(ImGuiMouseButton.Left))
                    {
                        var delta = GraphCanvas.Current.InverseTransformDirection(ImGui.GetIO().MouseDelta);
                        annotation.Size += delta;
                    }

                    ImGui.SetMouseCursor(ImGuiMouseCursor.Arrow);
                }

                
                // Background
                drawList.AddRectFilled(_lastScreenRect.Min, _lastScreenRect.Max, _backgroundColor);


                
                // Interaction
                ImGui.SetCursorScreenPos(lastClickableRect.Min);
                ImGui.InvisibleButton("node", lastClickableRect.GetSize());

                THelpers.DebugItemRect();
                var hovered = ImGui.IsItemHovered();
                if (hovered)
                {
                    ImGui.SetMouseCursor(ImGuiMouseCursor.Hand);
                }
                
                // Header
                drawList.AddRectFilled(lastClickableRect.Min, lastClickableRect.Max,
                                       hovered
                                           ? _backgroundColorHover
                                           : _backgroundColor);
                

                HandleDragging(annotation);
                var shouldRename = ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left);
                Renaming.Draw(annotation, shouldRename);
                


                if (annotation.IsSelected)
                {
                    const float thickness = 1;
                    drawList.AddRect(_lastScreenRect.Min - Vector2.One * thickness,
                                     _lastScreenRect.Max + Vector2.One * thickness,
                                     Color.White, 0f, 0, thickness);
                }

                // Label
                {
                    var isScaledDown = GraphCanvas.Current.Scale.X < 1;
                    ImGui.PushFont(isScaledDown ? Fonts.FontSmall : Fonts.FontBold);

                    drawList.PushClipRect(_lastScreenRect.Min, _lastScreenRect.Max, true);
                    var labelPos = _lastScreenRect.Min + new Vector2(4, 4);

                    drawList.AddText(labelPos,
                                     ColorVariations.OperatorLabel.Apply(Color.White),
                                     annotation.Title);
                    ImGui.PopFont();
                    drawList.PopClipRect();
                }
            }
            ImGui.PopID();
        }

        private static void HandleDragging(Annotation annotation, Instance instance = null)
        {
            if (ImGui.IsItemActive())
            {
                if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
                {
                    var compositionSymbolId = GraphCanvas.Current.CompositionOp.Symbol.Id;
                    _draggedNodeId = annotation.Id;
                    if (annotation.IsSelected)
                    {
                        _draggedNodes = SelectionManager.GetSelectedNodes<ISelectableNode>().ToList();
                    }
                    else
                    {
                        var parentUi = SymbolUiRegistry.Entries[GraphCanvas.Current.CompositionOp.Symbol.Id];

                        if(!ImGui.GetIO().KeyCtrl)
                            _draggedNodes = FindAnnotatedOps(parentUi, annotation).ToList();
                        _draggedNodes.Add(annotation);
                    }

                    _moveCommand = new ChangeSelectableCommand(compositionSymbolId, _draggedNodes);
                }
                else if (_moveCommand != null)
                {
                }

                HandleNodeDragging(annotation);
            }
            else if (ImGui.IsMouseReleased(0) && _moveCommand != null)
            {
                if (_draggedNodeId != annotation.Id)
                    return;

                // var singleDraggedNode = (_draggedNodes.Count == 1) ? _draggedNodes[0] : null;
                _draggedNodeId = Guid.Empty;
                _draggedNodes.Clear();

                var wasDragging = ImGui.GetMouseDragDelta(ImGuiMouseButton.Left).LengthSquared() > UserSettings.Config.ClickTreshold;
                if (wasDragging)
                {
                    _moveCommand.StoreCurrentValues();
                    UndoRedoStack.Add(_moveCommand);
                }
                else
                {
                    if (!SelectionManager.IsNodeSelected(annotation))
                    {
                        if (!ImGui.GetIO().KeyShift)
                        {
                            SelectionManager.Clear();
                        }

                        SelectionManager.AddSelection(annotation);
                    }
                    else
                    {
                        if (ImGui.GetIO().KeyShift)
                        {
                            SelectionManager.DeselectNode(annotation, instance);
                        }
                    }
                }

                _moveCommand = null;
            }

            var wasDraggingRight = ImGui.GetMouseDragDelta(ImGuiMouseButton.Right).Length() > UserSettings.Config.ClickTreshold;
            if (ImGui.IsMouseReleased(ImGuiMouseButton.Right)
                && !wasDraggingRight
                && ImGui.IsItemHovered()
                && !SelectionManager.IsNodeSelected(annotation))
            {
                SelectionManager.SetSelection(annotation);
            }
        }

        private static List<ISelectableNode> FindAnnotatedOps(SymbolUi parentUi, Annotation annotation)
        {
            var matches = new List<ISelectableNode>();
            var aRect = new ImRect(annotation.PosOnCanvas, annotation.PosOnCanvas + annotation.Size);

            foreach (var n in parentUi.ChildUis)
            {
                var nRect = new ImRect(n.PosOnCanvas, n.PosOnCanvas + n.Size);
                if (aRect.Contains(nRect))
                    matches.Add(n);
            }

            return matches;
        }

        private static void HandleNodeDragging(ISelectableNode draggedNode)
        {
            if (!ImGui.IsMouseDragging(ImGuiMouseButton.Left))
            {
                _isDragging = false;
                return;
            }

            if (!_isDragging)
            {
                _dragStartDelta = ImGui.GetMousePos() - GraphCanvas.Current.TransformPosition(draggedNode.PosOnCanvas);
                _isDragging = true;
            }

            var newDragPos = ImGui.GetMousePos() - _dragStartDelta;
            var newDragPosInCanvas = GraphCanvas.Current.InverseTransformPosition(newDragPos);
            var moveDeltaOnCanvas = newDragPosInCanvas - draggedNode.PosOnCanvas;

            // Drag selection
            foreach (var e in _draggedNodes)
            {
                e.PosOnCanvas += moveDeltaOnCanvas;
            }
        }

        private static class Renaming
        {
            public static void Draw(Annotation annotation, bool shouldBeOpened)
            {
                var justOpened = false;
                if (_focusedAnnotationId == Guid.Empty)
                {
                    if (shouldBeOpened)
                    {
                        justOpened = true;
                        ImGui.SetKeyboardFocusHere();
                        _focusedAnnotationId = annotation.Id;
                    }
                }

                if (_focusedAnnotationId == Guid.Empty)
                    return;

                if (_focusedAnnotationId != annotation.Id)
                    return;
                
                var positionInScreen = _lastScreenRect.Min;
                ImGui.SetCursorScreenPos(positionInScreen);

                var text = annotation.Title;
                
                ImGui.SetNextItemWidth(150);
                ImGui.InputTextMultiline("##renameAnnotation", ref text, 256, _lastScreenRect.GetSize());
                if(!ImGui.IsItemDeactivated())
                    annotation.Title = text;

                if (!justOpened && (ImGui.IsItemDeactivated() || ImGui.IsKeyPressed((int)Key.Esc)))
                {
                    _focusedAnnotationId = Guid.Empty;
                }
            }

            private static Guid _focusedAnnotationId;
            public static bool IsOpen => _focusedAnnotationId != Guid.Empty;
        }

        private static bool _isDragging;
        private static Vector2 _dragStartDelta;
        private static ChangeSelectableCommand _moveCommand;

        private static Guid _draggedNodeId = Guid.Empty;
        private static List<ISelectableNode> _draggedNodes = new List<ISelectableNode>();

        private static bool _isVisible;
        private static ImRect _lastScreenRect;
    }
}