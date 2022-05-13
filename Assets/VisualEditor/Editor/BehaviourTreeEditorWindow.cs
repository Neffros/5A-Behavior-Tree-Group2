using System;
using BehaviorTreeSerializer.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VisualEditor.Editor {
    public class BehaviourTreeEditorWindow : EditorWindow {
        private BehaviourTreeEditorGraphView _graphView;
        private BehaviourTreeEditorInspectorView _inspectorView;
        private static BehaviorTreeObject _targetObject;

        private static bool _guiCreated;
        
        /// <summary>
        /// Opens visual editor and loads targetObject
        /// </summary>
        public static void OpenWindow(BehaviorTreeObject targetObject) {
            _targetObject = targetObject;
            BehaviourTreeEditorWindow wnd = GetWindow<BehaviourTreeEditorWindow>();
            wnd.titleContent = new GUIContent("BehaviourTreeEditorWindow");
            if (!_guiCreated) {
                wnd.CreateGUI();
                _guiCreated = true;
            }
        }

        public void CreateGUI() {
            rootVisualElement.Clear();
            if (_targetObject == null) {
                return;
            }
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/VisualEditor/Editor/Layouts/BehaviourTreeEditorWindow.uxml");
            VisualElement uxmlLayout = visualTree.Instantiate();
            for (int i = 0; i < uxmlLayout.childCount; ++i) {
                root.Add(uxmlLayout.ElementAt(i));
            }
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/VisualEditor/Editor/Style/Style.uss");
            root.styleSheets.Add(styleSheet);

            _graphView = root.Q<BehaviourTreeEditorGraphView>();
            _inspectorView = root.Q<BehaviourTreeEditorInspectorView>();

            _graphView.PopulateView(_targetObject);
        }

        private void OnGUI() {
            rootVisualElement.MarkDirtyRepaint();
        }
    }
}