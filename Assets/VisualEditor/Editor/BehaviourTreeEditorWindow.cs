using BehaviorTreeSerializer.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VisualEditor.Editor {
    public class BehaviourTreeEditorWindow : EditorWindow {
        private BehaviourTreeEditorGraphView _graphView;
        private static BehaviorTreeObject _targetObject;
        
        /// <summary>
        /// Opens visual editor and loads targetObject
        /// </summary>
        public static void OpenWindow(BehaviorTreeObject targetObject) {
            _targetObject = targetObject;
            BehaviourTreeEditorWindow wnd = GetWindow<BehaviourTreeEditorWindow>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditorWindow");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/VisualEditor/Editor/Layouts/BehaviourTreeEditorWindow.uxml");
            var nodeStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/VisualEditor/Editor/Style/NodeStyle.uss");
            VisualElement uxmlLayout = visualTree.Instantiate();
            for (int i = 0; i < uxmlLayout.childCount; ++i) {
                root.Add(uxmlLayout.ElementAt(i));
            }

            var viewPortContent = root.Query("ViewportContent").First();
            if (_graphView == null) {
                _graphView = new BehaviourTreeEditorGraphView(nodeStyle, _targetObject);
            }
            viewPortContent.Add(_graphView);
            _graphView.CreateGUI();
        }
    }
}