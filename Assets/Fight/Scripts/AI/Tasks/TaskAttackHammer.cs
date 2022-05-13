using BehaviorTree;
using DG.Tweening;
using Fight.AI.Agents;
using UnityEngine;

namespace Fight.AI.Tasks
{
	public class TaskAttackHammer : Node
	{
		private Transform _hammerAnchor;
		
		private bool _finished = false;

		protected override void OnInitialize()
		{
			BossSceneData sceneData = Agent.GetComponent<BossSceneData>();
			
			_hammerAnchor = sceneData.HammerAnchor;
		}

		protected override NodeState OnStart()
		{
			DG.Tweening.Sequence attackHammer = DOTween.Sequence();
			attackHammer.Append(_hammerAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(-20, 0, 0), 1.0f)
				.SetEase(Ease.OutQuad));
			attackHammer.Append(_hammerAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(130, 0, -30), 0.1f)
				.SetEase(Ease.InQuad));
			attackHammer.Append(_hammerAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.3f)
				.SetEase(Ease.OutQuad));
			attackHammer.onComplete += () => {
				_finished = true;
			};

			return NodeState.Running;
		}

		protected override NodeState OnUpdate()
		{
			return _finished ? NodeState.Success : NodeState.Running;
		}

		protected override void OnReset()
		{
			_finished = false;
		}
	}
}