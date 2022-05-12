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
            SetupZoom(ContentZoomer.DefaultMinScale, 2.0f);
            
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
            foreach (var nodeMetadata in _nodeReflectionEngine.Metadata) {
                evt.menu.AppendAction("Create Node/" + nodeMetadata.Value.Name, action => {
                    var node = new VisualElement();
                    node.AddToClassList("node");
                    node.styleSheets.Add(_nodeStyleSheet);
                    if (nodeMetadata.Value.DisplayAsBlock) {    // sequence
                        node.AddToClassList("node-sequence");

                        var vContainer = new VisualElement();
                        vContainer.AddToClassList("v-container");
                        vContainer.Add(new TextElement { text = nodeMetadata.Value.Name });

                        var hContainer = new VisualElement();
                        hContainer.AddToClassList("h-container");
                        
                        // add all nodes here
                        
                        vContainer.Add(hContainer);
                        node.Add(vContainer);
                        
                        node.Add(new Button(() => {
                        
                        }) {
                            text = "+",
                        });
                    }
                    else {
                        node.Add(new TextElement { text = nodeMetadata.Value.Name });
                        var buttonAdd = new Button(() => {
                        
                        }) {
                            text = "+",
                        };
                        node.Add(buttonAdd);
                    }
                    contentViewContainer.Add(node);
                    _nodes.Add(node);
                });
            }
                
        }
    }
}