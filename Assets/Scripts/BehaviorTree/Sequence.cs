namespace BehaviorTree
{
    public class Sequence : Node
    {
        public Sequence()
        {
        }
        
        public override NodeState Evaluate()
        {
            var anyChildIsRunning = false;

            foreach (var node in Children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        State = NodeState.FAILURE;
                        return State;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        break;
                    default:
                        State = NodeState.SUCCESS;
                        return State;
                }
            }

            State = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return State;
        }
    }
}