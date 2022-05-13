using BehaviorTreeSerializer.Data;
using NodeReflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace VisualEditor.Editor {
    public class NodeView : Node {
        public NodeEditorInstanceMetadata Node;
        public Port Input;
        public Port Output;

        public NodeView(NodeEditorInstanceMetadata node) {
            Node = node;
            title = node.NodeTypeInternalName;
            viewDataKey = node.Id;
            style.left = node.PositionInEditor.x;
            style.top = node.PositionInEditor.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts() {
            if (Engine.Metadata[Node.NodeTypeInternalName].DisplayAsBlock) {
                Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else {
                Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            }

            if (Input != null) {
                Input.portName = "";
                inputContainer.Add(Input);
            }
        }

        private void CreateOutputPorts() {
            if (Engine.Metadata[Node.NodeTypeInternalName].DisplayAsBlock) {
                Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else {
                Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            
            if (Output != null) {
                Output.portName = "";
                outputContainer.Add(Output);
            }
        }

        public override void SetPosition(Rect newPos) {
            base.SetPosition(newPos);

            Node.PositionInEditor.x = newPos.xMin;
            Node.PositionInEditor.y = newPos.yMin;
        }
    }
}