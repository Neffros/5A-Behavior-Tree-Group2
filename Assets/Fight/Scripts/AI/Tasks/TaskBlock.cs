using BehaviorTree;
using DG.Tweening;
using Fight.AI.Agents;
using UnityEngine;

namespace Fight.AI.Tasks
{
	public class TaskBlock : Node
	{
		private Transform _protectAnchor;
		private Transform _swordAnchor;
		
		private bool _finished = false;

		protected override void OnInitialize()
		{
			BossSceneData sceneData = Agent.GetComponent<BossSceneData>();
			
			_protectAnchor = sceneData.ProtectAnchor;
			_swordAnchor = sceneData.SwordAnchor;
		}

		protected override NodeState OnStart()
		{
			DG.Tweening.Sequence protect = DOTween.Sequence();
			protect.Append(_protectAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, -80, 0), 0.3f)
				.SetEase(Ease.InOutQuad));
			protect.Join(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(110, 0, 0), 0.3f)
				.SetEase(Ease.InOutQuad));
			protect.AppendInterval(3.0f);
			protect.Append(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.3f)
				.SetEase(Ease.InOutQuad));
			protect.Join(_protectAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.3f)
				.SetEase(Ease.InOutQuad));
			protect.onComplete += () => {
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