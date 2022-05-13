using System;
using NodeReflection;

namespace BehaviorTree
{
	/// <summary>
	/// Repeat mode
	/// </summary>
	public enum RepeatMode
	{
		RepeatIfSuccess,
		RepeatIfFailure
	}
	
	/// <summary>
	/// Repeats the child while its state does not match the RepeatMode
	/// </summary>
	[VisualNode]
	public class Repeater : Node
	{
		/// <summary>
		/// Repeat mode
		/// </summary>
		[ExposedInVisualEditor(defaultValue: RepeatMode.RepeatIfSuccess)]
		public RepeatMode RepeatMode { get; set; }
		
		/// <summary>
		/// Evaluates the node
		/// </summary>
		/// <returns>
		/// If RepeatMode is RepeatIfSuccess:
		///		Return Success if the child state is Success, Running otherwise.
		/// If RepeatMode is RepeatIfFailure:
		///		Return Failure if the child state is Failure, Running otherwise.
		/// </returns>
		/// <exception cref="InvalidOperationException">If the repeater does not have exactly one child.</exception>
		protected override NodeState OnUpdate()
		{
			if (Children.Count != 1)
			{
				throw new InvalidOperationException("A Repeater must have exactly one child.");
			}

			Node child = Children[0];
			
			child.Update();
			switch (child.State)
			{
				case NodeState.Failure:
					switch (RepeatMode)
					{
						case RepeatMode.RepeatIfSuccess:
							return NodeState.Failure;
						case RepeatMode.RepeatIfFailure:
							child.Reset();
							return NodeState.Running;
					}
					break;
				case NodeState.Success:
					switch (RepeatMode)
					{
						case RepeatMode.RepeatIfSuccess:
							child.Reset();
							return NodeState.Running;
						case RepeatMode.RepeatIfFailure:
							return NodeState.Success;
					}
					break;
				case NodeState.Running:
					return NodeState.Running;
			}
			
			throw new InvalidOperationException("");
		}
	}
}