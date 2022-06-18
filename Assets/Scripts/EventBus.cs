using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Event bus component.
    ///
    /// This component should be placed as a level manager so game objects can register their events and listeners here. Events should be triggered using this components method TriggerEvent.
    ///
    /// Actually events are internally indexed using their EventArgs types.
    /// </summary>
    public class EventBus : MonoBehaviour
    {
        /// <summary>
        /// Exception thrown if anyone tries to add a listener to a non existing event.
        /// </summary>
        public class NotExistingEvent : Exception
        {
            public string TriedEvent { get; private set; }
            
            public NotExistingEvent(string triedEvent) : base ($"Tried a not existing event: {triedEvent}")
            {
                TriedEvent = triedEvent;
            }
        }
        
        private Dictionary<Type, Delegate> _eventTable = new Dictionary<Type, Delegate>();

        /// <summary>
        /// Add a new event to event table.
        /// </summary>
        /// <param name="_event">Delegate for this event.</param>
        /// <typeparam name="T">EventArgs concrete type.</typeparam>
        public void RegisterEvent<T>(EventHandler<T> _event)
        {
            _eventTable[typeof(T)] = null;
        }

        /// <summary>
        /// Add a listener to an existing event.
        /// </summary>
        /// <param name="listener">Callback for this event.</param>
        /// <typeparam name="T">EventArgs concrete type.</typeparam>
        /// <exception cref="NotExistingEvent">Thrown if event is not registered yet.</exception>
        public void AddListener<T>(EventHandler<T> listener)
        {
            if (!_eventTable.ContainsKey(typeof(T)))
            {
                throw new NotExistingEvent(typeof(T).Name);
            }
            _eventTable[typeof(T)] = (EventHandler<T>) _eventTable[typeof(T)] + listener;
        }

        /// <summary>
        /// Remove a listener from an existing event.
        /// </summary>
        /// <param name="listener">Callback for this event.</param>
        /// <typeparam name="T">EventArgs concrete type.</typeparam>
        /// <exception cref="NotExistingEvent">Thrown if event is not registered yet.</exception>
        public void RemoveListener<T>(EventHandler<T> listener)
        {
            if (!_eventTable.ContainsKey(typeof(T)))
            {
                throw new NotExistingEvent(typeof(T).Name);
            }
            _eventTable[typeof(T)] = (EventHandler<T>) _eventTable[typeof(T)] - listener;
        }

        /// <summary>
        /// Trigger a registered event.
        /// </summary>
        /// <param name="_event">Event to trigger.</param>
        /// <param name="args">Concrete EventArgs for this event.</param>
        /// <param name="sender">Instance that is raising this event.</param>
        /// <typeparam name="T">EventArgs concrete type.</typeparam>
        /// <exception cref="NotExistingEvent">Thrown if event is not registered yet.</exception>
        public void TriggerEvent<T>(EventHandler<T> _event, T args, object sender)
        {
            if (!_eventTable.ContainsKey(typeof(T)))
            {
                throw new NotExistingEvent(typeof(T).Name);
            }
            ((EventHandler<T>) _eventTable[typeof(T)])?.Invoke(sender, args);
        }
        
    }
}