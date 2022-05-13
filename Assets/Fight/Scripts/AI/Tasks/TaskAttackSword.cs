using BehaviorTree;
using DG.Tweening;
using Fight.AI.Agents;
using UnityEngine;

namespace Fight.AI.Tasks
{
	public class TaskAttackSword : Node
	{
		private Transform _swordAnchor;
		
		private bool _finished = false;

		protected override void OnInitialize()
		{
			BossSceneData sceneData = Agent.GetComponent<BossSceneData>();
			
			_swordAnchor = sceneData.SwordAnchor;
		}

		protected override NodeState OnStart()
		{
			DG.Tweening.Sequence attackSword = DOTween.Sequence();
			attackSword.Append(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(-20, 0, 0), 1.0f)
				.SetEase(Ease.OutQuad));
			attackSword.Append(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(130, 0, 30), 0.1f)
				.SetEase(Ease.InQuad));
			attackSword.Append(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.3f)
				.SetEase(Ease.OutQuad));
			attackSword.onComplete += () => {
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