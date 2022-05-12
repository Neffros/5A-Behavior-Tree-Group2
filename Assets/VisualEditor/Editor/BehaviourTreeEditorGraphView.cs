using System.Collections.Generic;
using NodeReflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace VisualEditor.Editor {
    public class BehaviourTreeEditorGraphView : GraphView {
        private readonly StyleSheet _nodeStyleSheet;
        private List<VisualElement> _nodes;

        private Engine _nodeReflectionEngine;
        
        public BehaviourTreeEditorGraphView(StyleSheet nodeStyleSheet) {
            _nodeStyleSheet = nodeStyleSheet;
        }
        
        private class CustomGridBackground : GridBackground{}
        public void CreateGUI() {
            _nodes = new List<VisualElement>();
            var gridBackground = new CustomGridBackground {
                name = "GridBackground"
            };
            Insert(0, gridBackground);
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            viewTransform.scale = Vector3.one * 0.25f;
            
            contentViewContainer.transform.scale = Vector3.one * 2;
            this.StretchToParentSize();

            _nodeReflectionEngine = new Engine();
            _nodeReflectionEngine.Update();
        }

        public new void UpdateViewTransform(Vector3 newPosition, Vector3 newScale) {
             base.UpdateViewTransform(newPosition, newScale);
             foreach (var node in _nodes) {
                 node.transform.scale = newScale;
             }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
            evt.menu.AppendAction("Create Node/Selector", action => {
                
            });
            evt.menu.AppendAction("Create Node/Sequence", action => {
                
            });
            foreach (var nodeMetadata in _nodeReflectionEngine.Metadata) {
                evt.menu.AppendAction("Create Node/Custom Node/" + nodeMetadata.Value.Name, action => {
                    var node = new VisualElement();
                    node.AddToClassList("node");
                    node.styleSheets.Add(_nodeStyleSheet);
                    node.Add(new TextElement {text = nodeMetadata.Value.Name});
                    contentViewContainer.Add(node);
                    _nodes.Add(node);
                });
            }
                
        }
    }
}