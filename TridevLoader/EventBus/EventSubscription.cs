using System;

namespace TridevLoader.EventBus {
    /// <summary>
    /// Represents a subscription to an event on the event bus, contains the event type
    /// </summary>
    /// <typeparam name="T">The event that's been subscribed to.</typeparam>
    public class EventSubscription<T> where T : EventBase {
        public EventSubscription(Type subEventType, EventSubscriptionPriority subPriority,
            Action<T> subHandler, Mod.Mod subscriber) {
            SubEventType = subEventType;
            SubPriority = subPriority;
            SubHandler = subHandler;
            Subscriber = subscriber;
        }

        public Action<T> SubHandler { get; }
        public EventSubscriptionPriority SubPriority { get; }
        public Type SubEventType { get; }
        public Mod.Mod Subscriber { get; }

        /// <summary>
        /// Handles the fired event by passing it to the action defined by the subscription.
        /// </summary>
        /// <param name="eventBaseFired">The event that was fired.</param>
        /// <exception cref="ArgumentException">Thrown if the type of the fired event does not match that of the subscription.</exception>
        public void HandleFiredEvent(EventBase eventBaseFired) {
            if (eventBaseFired.GetType() == SubEventType) {
                SubHandler.Invoke((T) eventBaseFired);
            } else {
                throw new ArgumentException("Unable to handle fired event due to type mismatch." +
                                            $" {eventBaseFired.GetType()} != {SubEventType}");
            }
        }
    }
}