using Infiltration;
using UnityEngine;

namespace Infiltration
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController controller;
        public Transform cam;

        [SerializeField] private float speed = 6f;
        [SerializeField] private float turnSmoothTime = .1f;
        [SerializeField] private float gravity = 9.81f;
        [SerializeField] private EventBool stateGameEvent;

        private Vector3 _velocity;

        private float _turnSmoothVelocity;

        public void Update()
        {
            if (UIManager.StateSet)
            {
                return;
            }
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            var direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= .1f)
            {
                var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                controller.Move(moveDir.normalized * (speed * Time.deltaTime));
            }

            _velocity.y -= gravity * Time.deltaTime;
            controller.Move(_velocity * Time.deltaTime);
        }

        public void GetHit()
        {
            stateGameEvent.Raise(false);
        }
    }
}