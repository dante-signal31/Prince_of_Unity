using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component is placed in end levels specific configuration game object to trigger an EndGame event.
    ///
    /// This event will we caught by level loader to restart the game.
    /// </summary>
    public class GameEndedEventEmitter : MonoBehaviour
    {
        private EventBus _eventBus;

        private void Awake()
        {
            _eventBus = GameObject.Find("GameManagers").GetComponentInChildren<EventBus>();
        }

        public void EmitGameEndedEvent()
        {
            _eventBus.TriggerEvent(new GameEvents.GameEnded(), this);
        }
    }
}