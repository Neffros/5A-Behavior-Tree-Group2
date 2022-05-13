using UnityEngine;

namespace Fight.AI.Agents
{
	public class BossSceneData : MonoBehaviour
	{
		public BossControllerScript BossController;
		public PlayerControllerScript PlayerController;

		public float MoveSpeed = 3;
		
		public float JumpHeight = 1;

		public Transform SwordAnchor;
		public Transform HammerAnchor;
		public Transform ProtectAnchor;
	}
}