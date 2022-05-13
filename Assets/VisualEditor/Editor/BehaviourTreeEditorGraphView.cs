using System.Collections.Generic;
using BehaviorTreeSerializer.Data;
using NodeReflection;
using NodeReflection.Data;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace VisualEditor.Editor {
    public class BehaviourTreeEditorGraphView : GraphView {
        private readonly StyleSheet _nodeStyleSheet;
        private BehaviorTreeObject _behaviorTreeObject;

        private List<VisualNode> _nodes;
        
        private VisualNode _selectedNode;

        public BehaviourTreeEditorGraphView(StyleSheet nodeStyleSheet, BehaviorTreeObject behaviorTreeObject) {
            _nodeStyleSheet = nodeStyleSheet;
            _behaviorTreeObject = behaviorTreeObject;
            _nodes = new List<VisualNode>();
        }
        
        private class CustomGridBackground : GridBackground{}
        
        /// <summary>
        /// Graph instantiation and nodes loading
        /// </summary>
        public void CreateGUI() {
            CleanGUI();
            var gridBackground = new CustomGridBackground {
                name = "GridBackground"
            };
            Insert(0, gridBackground);
            SetupZoom(ContentZoomer.DefaultMinScale, 2.0f);

            viewTransform.scale = Vector3.one * 0.5f;

            contentViewContainer.transform.scale = Vector3.one * 2;
            this.StretchToParentSize();

            Engine.Update();

            RepaintGraph();
            
            RegisterCallbacks();
        }

        /// <summary>
        /// Cleans graph
        /// </summary>
        private void CleanGUI() {
            contentViewContainer.Clear();
            _nodes.Clear();
        }

        /// <summary>
        /// Updates graph repaint to redraw from data
        /// </summary>
        private void RepaintGraph() {
            CleanGUI();
            foreach (var nodeEditorInstanceMetadata in _behaviorTreeObject.IdToNode.Values) {
                var nodeType = nodeEditorInstanceMetadata.NodeTypeInternalName;
                var nodeMetaData = Engine.Metadata[nodeType];
                var nodePos = nodeEditorInstanceMetadata.PositionInEditor;

                DrawVisualNode(nodeMetaData, nodeEditorInstanceMetadata, nodePos);
            }
        }

        /// <summary>
        /// Add visual node on graph from node serialized data
        /// </summary>
        private void DrawVisualNode(NodeMetadata nodeMetaData, NodeEditorInstanceMetadata nodeEditorInstanceMetadata, Vector2 nodePos) {
            var node = new VisualNode(nodeMetaData, _nodeStyleSheet, nodeEditorInstanceMetadata.Id,
                () => {
                    AddNode(nodeMetaData, nodePos, nodeEditorInstanceMetadata.Id);
                }) {
                style = {
                    left = nodePos.x,
                    top = nodePos.y
                }
            };
            if (nodeEditorInstanceMetadata.Id == _behaviorTreeObject.RootId) {
                node.AddToClassList("node-root");
            }
            node.RegisterCallback<MouseDownEvent>(evt => {
                if (evt.button != (int)MouseButton.LeftMouse)
                    return;
                _selectedNode = node;
            });
            node.RegisterCallback<MouseUpEvent>(evt => {
                if (evt.button != (int)MouseButton.RightMouse)
                    return;
                var menu = new GenericMenu();
                if (nodeEditorInstanceMetadata.Id == _behaviorTreeObject.RootId) {
                    menu.AddDisabledItem(new GUIContent("Delete Node"), false);
                }
                else {
                    menu.AddItem(new GUIContent("Delete Node"), false, () => {
                        node.Clear();
                        _behaviorTreeObject.RemoveNode(nodeEditorInstanceMetadata.Id);
                        EditorUtility.SetDirty(_behaviorTreeObject);
                        RepaintGraph();
                    });
                }

                menu.ShowAsContext();
            });

            contentViewContainer.Add(node);
            _nodes.Add(node);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
            foreach (var nodeMetadata in Engine.Metadata.Values) {
                evt.menu.AppendAction("Create Node/" + nodeMetadata.Name, action => {
                    var mousePos = Vector2.zero;//action.eventInfo.mousePosition; todo tofix
                    AddNode(nodeMetadata, mousePos, null);
                });
            }
        }

        private NodeEditorInstanceMetadata AddNode(NodeMetadata nodeMetadata, Vector2 position, string parentId) {
            var node = _behaviorTreeObject.AddNode(nodeMetadata.InternalName,
                new SerializableDictionary<string, object>(Engine.GetProperties(nodeMetadata.InternalName)), position,
                parentId);

            EditorUtility.SetDirty(_behaviorTreeObject);
                    
            RepaintGraph();
            return node;
        }

        /// <summary>
        /// Registers callbacks for mouse events
        /// </summary>
        private void RegisterCallbacks() {
            UnregisterCallbacks();
            RegisterCallback<MouseMoveEvent>(OnMouseMoved);
            RegisterCallback<MouseUpEvent>(OnMouseUp);
            generateVisualContent += OnGenerateVisualContent;
        }
        
        private void UnregisterCallbacks() {
            UnregisterCallback<MouseMoveEvent>(OnMouseMoved);
            UnregisterCallback<MouseUpEvent>(OnMouseUp);
            generateVisualContent -= OnGenerateVisualContent;
        }

        private void OnMouseMoved(MouseMoveEvent evt) {
            if (_selectedNode == null) return;
            Vector2 movement = evt.mouseDelta / viewTransform.scale;
            MoveNode(_selectedNode, movement);
        }

        private void OnMouseUp(MouseUpEvent evt) {
            _selectedNode = null;
        }
        
        public VisualNode GetNodeAtPosition(Vector2 position) {
            foreach (var node in _nodes) {
                if (node.ContainsPoint(position)) return node;
            }
            return null;
        }

        private void MoveNode(VisualNode node, Vector2 delta) {
            node.Move(delta);
            _behaviorTreeObject.IdToNode[node.Guid].PositionInEditor += delta;
            EditorUtility.SetDirty(_behaviorTreeObject);
        }
        
        private void OnGenerateVisualContent(MeshGenerationContext obj) {
            foreach (var nodeEditorInstanceMetadata in _behaviorTreeObject.IdToNode.Values) {
                foreach(var childId in nodeEditorInstanceMetadata.ChildrenIds) {
                    Vector2 start = nodeEditorInstanceMetadata.PositionInEditor;
                    Vector2 end = _behaviorTreeObject.IdToNode[childId].PositionInEditor;
                }
            }
        }
    }
}