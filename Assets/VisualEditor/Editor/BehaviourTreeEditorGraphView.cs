using System.Collections.Generic;
using BehaviorTreeSerializer.Data;
using NodeReflection;
using NodeReflection.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace VisualEditor.Editor {
    public class BehaviourTreeEditorGraphView : GraphView {
        private readonly StyleSheet _nodeStyleSheet;
        private BehaviorTreeObject _behaviorTreeObject;

        private Engine _nodeReflectionEngine;
        
        public BehaviourTreeEditorGraphView(StyleSheet nodeStyleSheet, BehaviorTreeObject behaviorTreeObject) {
            _nodeStyleSheet = nodeStyleSheet;
            _behaviorTreeObject = behaviorTreeObject;
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
        }

        /// <summary>
        /// Cleans graph
        /// </summary>
        private void CleanGUI() {
            contentViewContainer.Clear();
        }

        private void RepaintGraph() {
            CleanGUI();
            foreach (var nodeEditorInstanceMetadata in _behaviorTreeObject.IdToNode.Values) {
                var nodeType = nodeEditorInstanceMetadata.NodeTypeInternalName;
                var nodeMetaData = _nodeReflectionEngine.Metadata[nodeType];
                var node = new VisualNode(nodeMetaData, _nodeStyleSheet) {
                    style = {
                        top = nodeEditorInstanceMetadata.PositionInEditor.x,
                        left = nodeEditorInstanceMetadata.PositionInEditor.y
                    }
                };

                contentViewContainer.Add(node);
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
            foreach (var nodeMetadata in _nodeReflectionEngine.Metadata.Values) {
                evt.menu.AppendAction("Create Node/" + nodeMetadata.Name, action => {
                    /*var node = new VisualNode(nodeMetadata, _nodeStyleSheet);
                    contentViewContainer.Add(node);*/

                    var node = _behaviorTreeObject.AddNode(nodeMetadata.InternalName, new SerializableDictionary<string, object>(),
                        evt.mousePosition, null);
                    
                    EditorUtility.SetDirty(_behaviorTreeObject);
                    
                    RepaintGraph();
                });
            }
        }
    }
}