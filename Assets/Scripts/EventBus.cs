using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Event bus component.
    ///
    /// This component should be placed as a level manager so game objects can register their events and listeners here.
    /// Events should be triggered using this component's method TriggerEvent.
    ///
    /// Actually events are internally indexed using their EventArgs types. Multiple objects can send the same event
    /// because EventHandler<T> delegates includes a sender object reference next to T data in arguments passed to
    /// callbacks. 
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
        ///
        /// Should be run at awake() of registering game object.
        /// </summary>
        /// <param name="eventArgsType">EventArgs concrete type.</param>
        public void RegisterEvent<T>()
        {
            if (!_eventTable.ContainsKey(typeof(T))) _eventTable[typeof(T)] = null;
        }

        /// <summary>
        /// Add a listener to an existing event.
        ///
        /// Should be run at start() of listening game objects. If awake() is used to register listeners then
        /// race conditions may happen if a listener tries to add itself to event bus before events are registered.
        /// </summary>
        /// <param name="listener">Callback for this event.</param>
        /// <typeparam name="T">EventArgs concrete type.</typeparam>
        /// <exception cref="NotExistingEvent">Thrown if event is not registered yet.</exception>
        public void AddListener<T>(EventHandler<T> listener)
        {
            if (_eventTable.ContainsKey(typeof(T)))
            {
                _eventTable[typeof(T)] = (EventHandler<T>) _eventTable[typeof(T)] + listener;
            }
            else
            {
                throw new NotExistingEvent(typeof(T).Name);
            }
        }

        /// <summary>
        /// Remove a listener from an existing event.
        /// </summary>
        /// <param name="listener">Callback for this event.</param>
        /// <typeparam name="T">EventArgs concrete type.</typeparam>
        /// <exception cref="NotExistingEvent">Thrown if event is not registered yet.</exception>
        public void RemoveListener<T>(EventHandler<T> listener)
        {
            if (_eventTable.ContainsKey(typeof(T)))
            {
                _eventTable[typeof(T)] = (EventHandler<T>) _eventTable[typeof(T)] - listener;
            }
            else
            {
                throw new NotExistingEvent(typeof(T).Name);
            }
        }

        /// <summary>
        /// Trigger a registered event.
        /// </summary>
        /// <param name="args">Concrete EventArgs for this event.</param>
        /// <param name="sender">Instance that is raising this event.</param>
        /// <typeparam name="T">EventArgs concrete type.</typeparam>
        /// <exception cref="NotExistingEvent">Thrown if event is not registered yet.</exception>
        public void TriggerEvent<T>(T args, object sender)
        {
            if (_eventTable.ContainsKey(typeof(T)))
            {
                ((EventHandler<T>) _eventTable[typeof(T)])?.Invoke(sender, args);
            }
            else
            {
                throw new NotExistingEvent(typeof(T).Name);
            }
        }

        /// <summary>
        /// Whether event bus has already registered this event.
        /// </summary>
        /// <typeparam name="T">Event we are asking for.</typeparam>
        /// <returns>True is this event has been registered and we can add listeners to it.</returns>
        public bool HasRegisteredEvent<T>()
        {
            return _eventTable.ContainsKey(typeof(T));
        }
        
    }
}