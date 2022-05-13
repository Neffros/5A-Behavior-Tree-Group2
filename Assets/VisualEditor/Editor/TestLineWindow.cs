using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TestLineWindow : EditorWindow
{
    [MenuItem("Tools/OpenWindow")]
    public static void OpenWindow() {
        TestLineWindow wnd = GetWindow<TestLineWindow>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditorWindow");
    }

    private void CreateGUI() {
        rootVisualElement.generateVisualContent += GenerateVisualContent;
    }

    private void GenerateVisualContent(MeshGenerationContext obj) {
        Vector2 start = Vector2.zero;
        Vector2 end = Vector2.one * 500;
        Debug.Log("PAINTING LINE FROM " + start + " TO " + end);
        var painter = obj.painter2D;
        painter.strokeColor = Color.white;
        painter.lineWidth = 10.0f;
        painter.BeginPath();
        painter.MoveTo(start);
        painter.LineTo(end);
        painter.Stroke();
    }
}
