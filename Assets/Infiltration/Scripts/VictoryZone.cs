using UnityEngine;

namespace Infiltration
{
    public class VictoryZone : MonoBehaviour
    {
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private EventBool stateGameEvent;

        private void OnTriggerEnter(Collider other)
        {
            if ((playerMask.value & 1 << other.gameObject.layer) > 0)
            {
                stateGameEvent.Raise(true);
            }
        }
    }
}