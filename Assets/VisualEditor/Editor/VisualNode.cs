using System;
using NodeReflection.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace VisualEditor.Editor {
    public class VisualNode : VisualElement {
        private readonly TextElement _nodeText;
        private readonly string _guid;
        public string Guid => _guid;

        public VisualNode(NodeMetadata data, StyleSheet nodeStyleSheet, string guid, Action clickEvent) {
            _guid = guid;
            AddToClassList("node");
            styleSheets.Add(nodeStyleSheet);
            
            _nodeText = new TextElement { text = data.Name };
            var buttonAdd = new Button(clickEvent) {
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

        public void Move(Vector2 delta) {
            style.left = style.left.value.value + delta.x;
            style.top = style.top.value.value + delta.y;
        }

        public void OnMouseClick() {
            
        }
    }
}