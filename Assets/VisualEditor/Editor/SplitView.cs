using UnityEngine.UIElements;

namespace VisualEditor.Editor {
    public class SplitView : TwoPaneSplitView {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> {}
        
    }
}