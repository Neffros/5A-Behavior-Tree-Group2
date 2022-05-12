using UnityEngine;

namespace Infiltration
{
    public class GameEventBoolListener : MonoBehaviour
    {
        // The game event instance to register to.
        public EventBool gameEvent;
        // The unity event response created for the event.
        public CustomUnityEventBool response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void RaiseEvent(bool responseType)
        {
            response.Invoke(responseType);
        }
    }
}
