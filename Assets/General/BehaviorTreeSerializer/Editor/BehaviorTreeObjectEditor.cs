using System;
using BehaviorTreeSerializer.Data;
using UnityEditor;
using UnityEngine;
using VisualEditor.Editor;

namespace General.BehaviorTreeSerializer.Editor {
    [CustomEditor(typeof(BehaviorTreeObject))]
    public class BehaviorTreeObjectEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();
            if (GUILayout.Button("Open Editor")) {
                BehaviourTreeEditorWindow.OpenWindow((BehaviorTreeObject)target);
            }
            serializedObject.ApplyModifiedProperties();
            //base.OnInspectorGUI();
        }
    }
}