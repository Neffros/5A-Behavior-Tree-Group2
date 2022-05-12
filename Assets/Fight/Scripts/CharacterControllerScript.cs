using DG.Tweening;
using UnityEngine;

namespace Fight
{
	public class CharacterControllerScript : MonoBehaviour
	{
		[SerializeField]
		private CharacterController _characterController;

		[SerializeField]
		private float _moveSpeed = 3;
		
		[SerializeField]
		private float _jumpHeight = 1;

		private Vector3 _velocity;

		[SerializeField]
		private Transform _swordAnchor;
		[SerializeField]
		private Transform _shieldAnchor;

		private Sequence _attack;
		private Sequence _shield;
		
		private void Update()
		{
			Vector3 forward = transform.forward;
			Vector3 right = transform.right;
			
			Vector3 moveDirection = Vector3.zero;
			if (Input.GetKey(KeyCode.Z))
			{
				moveDirection += forward;
			}
			if (Input.GetKey(KeyCode.S))
			{
				moveDirection -= forward;
			}
			if (Input.GetKey(KeyCode.D))
			{
				moveDirection += right;
			}
			if (Input.GetKey(KeyCode.Q))
			{
				moveDirection -= right;
			}

			moveDirection.Normalize();
			moveDirection *= _moveSpeed;
			
			_velocity.x = moveDirection.x;
			_velocity.z = moveDirection.z;

			float gravity = Physics.gravity.y;

			if (_characterController.isGrounded)
			{
				_velocity.y = 0;
				
				if (Input.GetKey(KeyCode.Space))
				{
					_velocity.y = Mathf.Sqrt(2.0f * -gravity * _jumpHeight);
				}
			}
			
			_velocity.y += gravity * Time.deltaTime;
			_characterController.Move(_velocity * Time.deltaTime);
			
			if (Input.GetMouseButton(0))
			{
				Attack();
			}
			
			if (Input.GetMouseButtonDown(1))
			{
				EnableShield();
			}
			
			if (Input.GetMouseButtonUp(1))
			{
				DisableShield();
			}
		}

		private void Attack()
		{
			if (_attack != null)
				return;

			_attack = DOTween.Sequence();
			_attack.Append(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(-130, 0, 0), 0.3f)
				.SetEase(Ease.OutQuad));
			_attack.Append(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f)
				.SetEase(Ease.InQuad));
			_attack.Append(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(-110, 0, 0), 0.3f)
				.SetEase(Ease.OutQuad));
			_attack.onComplete += () => _attack = null;
		}

		private void EnableShield()
		{
			if (_shield != null)
			{
				_shield.Kill();
				_shield = null;
			}
			
			_shield = DOTween.Sequence();
			_shield.Append(_shieldAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, 80, 0), 0.3f)
				.SetEase(Ease.InOutQuad));
			_shield.onComplete += () => _shield = null;
		}

		private void DisableShield()
		{
			if (_shield != null)
			{
				_shield.Kill();
				_shield = null;
			}
			
			_shield = DOTween.Sequence();
			_shield.Append(_shieldAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.3f)
				.SetEase(Ease.InOutQuad));
			_shield.onComplete += () => _shield = null;
		}
	}
}
