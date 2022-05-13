using UnityEngine;

namespace Fight
{
	public class CameraRotator : MonoBehaviour
	{
		[SerializeField]
		[Range(0.0f, 10.0f)]
		private float _mouseSensitivity = 3;

		[SerializeField]
		private Transform _playerTransform;

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		private void Update()
		{
			Vector2 delta = new Vector2
			{
				x = Input.GetAxis("Mouse X"),
				y = Input.GetAxis("Mouse Y")
			};

			delta *= _mouseSensitivity;

			Vector3 yRotation = _playerTransform.localEulerAngles;
			yRotation.y += delta.x;
			_playerTransform.localEulerAngles = yRotation;

			Vector3 xRotation = transform.localEulerAngles;
			if (xRotation.x > 180)
			{
				xRotation.x -= 360;
			}

			xRotation.x = Mathf.Clamp(xRotation.x - delta.y, -89, 89);
			transform.localEulerAngles = xRotation;
		}
	}
}