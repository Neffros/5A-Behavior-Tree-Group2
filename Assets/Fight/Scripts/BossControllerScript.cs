using DG.Tweening;
using UnityEngine;

namespace Fight
{
	public class BossControllerScript : MonoBehaviour
	{
		[SerializeField]
		private CharacterController _characterController;

		[SerializeField]
		private Transform _playerTransform;

		private Vector3 _velocity;
		public Vector3 Velocity
		{
			get => _velocity;
			set => _velocity = value;
		}

		private Sequence _attackSword;
		private Sequence _attackHammer;
		private Sequence _protect;
		
		private void Update()
		{
			Vector3 moveDirection = _playerTransform.position - transform.position;
			moveDirection.y = 0;
			moveDirection.Normalize();

			if (moveDirection != Vector3.zero)
			{
				transform.forward = moveDirection;
			}

			float gravity = Physics.gravity.y;

			if (_characterController.isGrounded)
			{
				_velocity.y = 0;
			}
			
			_velocity.y += gravity * Time.deltaTime;
			_characterController.Move(_velocity * Time.deltaTime);

			_velocity.x = 0;
			_velocity.z = 0;
		}
	}
}
