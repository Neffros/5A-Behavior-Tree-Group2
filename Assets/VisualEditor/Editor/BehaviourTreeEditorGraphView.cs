using System.Collections.Generic;
using System.Linq;
using BehaviorTreeSerializer.Data;
using NodeReflection;
using NodeReflection.Data;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace VisualEditor.Editor {
    public class BehaviourTreeEditorGraphView : GraphView{
        public new class UxmlFactory : UxmlFactory<BehaviourTreeEditorGraphView, GraphView.UxmlTraits> {}

        private BehaviorTreeObject _behaviorTreeObject;
        
        public BehaviourTreeEditorGraphView() {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/VisualEditor/Editor/Style/Style.uss");
            styleSheets.Add(styleSheet);
        }

        NodeView FindNodeView(NodeEditorInstanceMetadata node) {
            return GetNodeByGuid(node.Id) as NodeView;
        }

        public void PopulateView(BehaviorTreeObject behaviorTreeObject) {
            _behaviorTreeObject = behaviorTreeObject;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
            
            // creates node view
            foreach (var nodeEditorInstanceMetadata in behaviorTreeObject.IdToNode.Values) {
                CreateNodeView(nodeEditorInstanceMetadata);
            }
            
            // create edges
            foreach (var nodeEditorInstanceMetadata in behaviorTreeObject.IdToNode.Values) {
                var children = nodeEditorInstanceMetadata.ChildrenIds.Select(id => behaviorTreeObject.IdToNode[id]);
                foreach (var childEditorInstanceMetadata in children) {
                    NodeView parentView = FindNodeView(nodeEditorInstanceMetadata);
                    NodeView childView = FindNodeView(childEditorInstanceMetadata);

                    Edge edge = parentView.Output.ConnectTo(childView.Input);
                    AddElement(edge);
                }
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
            return ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewchange) {
            if (graphviewchange.elementsToRemove != null) {
                graphviewchange.elementsToRemove.ForEach(e => {
                    NodeView nodeView = e as NodeView;
                    if (nodeView != null) {
                        _behaviorTreeObject.RemoveNode(nodeView.Node.Id);
                        EditorUtility.SetDirty(_behaviorTreeObject);
                    }

                    Edge edge = e as Edge;
                    if (edge != null) {
                        NodeView parentView = edge.output.node as NodeView;
                        NodeView childView = edge.input.node as NodeView;
                        _behaviorTreeObject.RemoveChild(parentView.Node.Id, childView.Node.Id);
                    }
                });
            }

            if (graphviewchange.edgesToCreate != null) {
                graphviewchange.edgesToCreate.ForEach(edge => {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    _behaviorTreeObject.AddChild(parentView.Node.Id, childView.Node.Id);
                });
            }
            
            return graphviewchange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
            foreach (var nodeMetadata in Engine.Metadata.Values) {
                evt.menu.AppendAction("Create Node/" + nodeMetadata.Name, action => {
                    CreateNode(nodeMetadata, null);
                });
            }
        }
        
        private void CreateNode(NodeMetadata nodeMetadata, string parentId) {
            Vector2 position = Vector2.zero;
            var node = _behaviorTreeObject.AddNode(nodeMetadata.InternalName, position, parentId);
            EditorUtility.SetDirty(_behaviorTreeObject);
            CreateNodeView(node);
        }

        private void CreateNodeView(NodeEditorInstanceMetadata nodeData) {
            NodeView nodeView = new NodeView(nodeData);
            AddElement(nodeView);
        }
    }
}