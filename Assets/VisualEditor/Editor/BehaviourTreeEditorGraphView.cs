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

        private Engine _nodeReflectionEngine;

        private List<VisualNode> _nodes;
        
        private VisualNode _selectedNode;
        private Vector2 _mousePosOnDown;

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

            _nodeReflectionEngine ??= new Engine();
            _nodeReflectionEngine.Update();
            
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
                var nodeMetaData = _nodeReflectionEngine.Metadata[nodeType];
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
            node.RegisterCallback<MouseDownEvent>(evt => {
                if (evt.button != (int)MouseButton.LeftMouse)
                    return;
                _mousePosOnDown = evt.originalMousePosition;
                _selectedNode = node;
            });
            node.RegisterCallback<MouseUpEvent>(evt => {
                if (evt.button != (int)MouseButton.RightMouse)
                    return;
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Delete Node"), false, () => {
                    node.Clear();
                    _behaviorTreeObject.RemoveNode(nodeEditorInstanceMetadata.Id);
                    EditorUtility.SetDirty(_behaviorTreeObject);
                    RepaintGraph();
                });
                menu.ShowAsContext();
            });

            contentViewContainer.Add(node);
            _nodes.Add(node);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
            foreach (var nodeMetadata in _nodeReflectionEngine.Metadata.Values) {
                evt.menu.AppendAction("Create Node/" + nodeMetadata.Name, action => {
                    Debug.Log(scale);
                    var mousePos = Vector2.zero;//action.eventInfo.mousePosition;
                    AddNode(nodeMetadata, mousePos, null);
                });
            }
        }

        private NodeEditorInstanceMetadata AddNode(NodeMetadata nodeMetadata, Vector2 position, string parentId) {
            Debug.Log("Drawing a node at position " + position);
            var node = _behaviorTreeObject.AddNode(nodeMetadata.InternalName,
                new SerializableDictionary<string, object>(), position, parentId);

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
        }


        private void UnregisterCallbacks() {
            UnregisterCallback<MouseMoveEvent>(OnMouseMoved);
            UnregisterCallback<MouseUpEvent>(OnMouseUp);
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
    }
}