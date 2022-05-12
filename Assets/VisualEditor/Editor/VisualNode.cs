using NodeReflection.Data;
using UnityEngine.UIElements;

namespace VisualEditor.Editor {
    public class VisualNode : VisualElement {
        private readonly TextElement _nodeText;

        public VisualNode(NodeMetadata data, StyleSheet nodeStyleSheet) : base(){
            AddToClassList("node");
            styleSheets.Add(nodeStyleSheet);
            
            _nodeText = new TextElement { text = data.Name };
            var buttonAdd = new Button(() => {
                
            }) {
                text = "+"
            };
            
            if (data.DisplayAsBlock) {
                AddToClassList("node-sequence");

                var vContainer = new VisualElement();
                vContainer.AddToClassList("v-container");
                vContainer.Add(_nodeText);

                var hContainer = new VisualElement();
                hContainer.AddToClassList("h-container");
                
                // add all child nodes here
                
                vContainer.Add(hContainer);
                Add(vContainer);
                
                Add(buttonAdd);
            }
            else {
                Add(_nodeText);
                Add(buttonAdd);
            }
        }

        public void SetText(string text) {
            _nodeText.text = text;
        }
    }
}