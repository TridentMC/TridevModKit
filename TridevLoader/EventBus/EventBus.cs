using System;
using System.Collections.Generic;
using TridevLoader.Debug;

namespace TridevLoader.EventBus {
    public class EventBus {
        private readonly Dictionary<Type, EventSubscriptionList> subLists = new Dictionary<Type, EventSubscriptionList>();

        /// <summary>
        /// Subscribe to an event on the event bus.
        /// </summary>
        /// <param name="subscriber">The mod that is subscribing to this event.</param>
        /// <param name="priority">The event priority.</param>
        /// <param name="handler">The handler that's called when the event is fired.</param>
        /// <typeparam name="T">The type of event that the subscription listens for.</typeparam>
        /// <returns>An EventSubscription if the subscription was added, or null if it failed.</returns>
        public EventSubscription<T> AddSubscription<T>(Mod.Mod subscriber, EventSubscriptionPriority priority, Action<T> handler) where T : EventBase {
            var eventType = typeof(T);
            if (!this.subLists.ContainsKey(eventType)) {
                this.subLists.Add(eventType, new EventSubscriptionList(eventType));
            }

            var subscription = new EventSubscription<T>(eventType, priority, handler, subscriber);
            return this.subLists[eventType].AddSubscription(subscription) ? subscription : null;
        }

        /// <summary>
        /// Fires an event on the event bus.
        /// </summary>
        /// <param name="eventToFire">The event that should be fired.</param>
        /// <typeparam name="T">The type of event that's being fired.</typeparam>
        /// <returns>The event with any changes that may have occured.</returns>
        public T FireEvent<T>(T eventToFire) where T : EventBase {
            if (eventToFire != null)
                this.subLists[typeof(T)].HandleFiredEvent(eventToFire);
            return eventToFire;
        }
    }
}