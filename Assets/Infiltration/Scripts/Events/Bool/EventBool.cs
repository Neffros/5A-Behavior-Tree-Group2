using System.Collections.Generic;
using UnityEngine;

namespace Infiltration
{
    [CreateAssetMenu(fileName = "Event Bool", menuName = "Events/Event Bool", order = 1)]
    public class EventBool : ScriptableObject
    {
        private readonly List<GameEventBoolListener> _listeners = new List<GameEventBoolListener>();
        public void RegisterListener(GameEventBoolListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnregisterListener(GameEventBoolListener listener)
        {
            _listeners.Remove(listener);
        }

        public void Raise(bool responseType)
        {
            for (var i = _listeners.Count - 1; i >= 0; --i)
            {
                _listeners[i].RaiseEvent(responseType);
            }
        }
    }
}
