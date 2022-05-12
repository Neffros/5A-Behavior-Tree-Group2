using DG.Tweening;
using UnityEngine;

namespace Fight
{
	public class BossControllerScript : MonoBehaviour
	{
		[SerializeField]
		private CharacterController _characterController;

		[SerializeField]
		private float _moveSpeed = 3;
		
		[SerializeField]
		private float _jumpHeight = 1;

		[SerializeField]
		private Transform _playerTransform;

		private Vector3 _velocity;

		[SerializeField]
		private Transform _swordAnchor;
		[SerializeField]
		private Transform _hammerAnchor;
		[SerializeField]
		private Transform _protectAnchor;

		private Sequence _attackSword;
		private Sequence _attackHammer;
		private Sequence _protect;
		
		private void Update()
		{
			Vector3 moveDirection = _playerTransform.position - transform.position;
			moveDirection.y = 0;
			moveDirection.Normalize();

			transform.forward = moveDirection;

			moveDirection *= _moveSpeed;
			
			_velocity.x = moveDirection.x;
			_velocity.z = moveDirection.z;

			float gravity = Physics.gravity.y;

			if (_characterController.isGrounded)
			{
				_velocity.y = 0;
				
				if (Input.GetKey(KeyCode.M))
				{
					_velocity.y = Mathf.Sqrt(2.0f * -gravity * _jumpHeight);
				}
			}
			
			_velocity.y += gravity * Time.deltaTime;
			_characterController.Move(_velocity * Time.deltaTime);
			
			if (Input.GetKeyDown(KeyCode.O))
			{
				AttackSword();
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				AttackHammer();
			}

			if (Input.GetKeyDown(KeyCode.L))
			{
				EnableProtect();
			}
			if (Input.GetKeyUp(KeyCode.L))
			{
				DisableProtect();
			}
		}

		private void AttackSword()
		{
			if (_attackSword != null)
				return;

			_attackSword = DOTween.Sequence();
			_attackSword.Append(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(-20, 0, 0), 1.0f)
				.SetEase(Ease.OutQuad));
			_attackSword.Append(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(130, 0, 30), 0.1f)
				.SetEase(Ease.InQuad));
			_attackSword.Append(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.3f)
				.SetEase(Ease.OutQuad));
			_attackSword.onComplete += () => _attackSword = null;
		}
		
		private void AttackHammer()
		{
			if (_attackHammer != null)
				return;

			_attackHammer = DOTween.Sequence();
			_attackHammer.Append(_hammerAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(-20, 0, 0), 1.0f)
				.SetEase(Ease.OutQuad));
			_attackHammer.Append(_hammerAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(130, 0, -30), 0.1f)
				.SetEase(Ease.InQuad));
			_attackHammer.Append(_hammerAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.3f)
				.SetEase(Ease.OutQuad));
			_attackHammer.onComplete += () => _attackHammer = null;
		}
		
		private void EnableProtect()
		{
			if (_protect != null)
			{
				_protect.Kill();
				_protect = null;
			}
			
			_protect = DOTween.Sequence();
			_protect.Append(_protectAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, -80, 0), 0.3f)
				.SetEase(Ease.InOutQuad));
			_protect.Join(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(110, 0, 0), 0.3f)
				.SetEase(Ease.InOutQuad));
			_protect.onComplete += () => _protect = null;
		}

		private void DisableProtect()
		{
			if (_protect != null)
			{
				_protect.Kill();
				_protect = null;
			}
			
			_protect = DOTween.Sequence();
			_protect.Append(_swordAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.3f)
				.SetEase(Ease.InOutQuad));
			_protect.Join(_protectAnchor
				.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.3f)
				.SetEase(Ease.InOutQuad));
			_protect.onComplete += () => _protect = null;
		}
	}
}
