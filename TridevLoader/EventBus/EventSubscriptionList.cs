using System;
using System.Collections.Generic;
using TridevLoader.Debug;

namespace TridevLoader.EventBus {
    /// <summary>
    /// Internal class that defines a list of subscriptions for a given event and then sorts them by their priority.
    /// </summary>
    /// <typeparam name="T">The type of event that this list manages.</typeparam>
    public class EventSubscriptionList {
        private readonly Type type;
        private object sortedSubscriptions;

        public EventSubscriptionList(Type type) {
            this.type = type;
        }

        /// <summary>
        /// Get the set of sorted subscriptions that this list manages.
        /// 
        /// C# doesn't have type erasure, so what if I just pretend it does :^)
        /// </summary>
        /// <typeparam name="T">The type of event the set contains.</typeparam>
        /// <returns>All the event subscriptions managed by this list.</returns>
        private SortedSet<EventSubscription<T>> GetSortedSubscriptions<T>() where T : EventBase {
            if (typeof(T) != this.type) {
                throw new ArgumentException("Attempted to get sorted subscriptions on an event list with the incorrect type.");
            }

            if (this.sortedSubscriptions == null) {
                this.sortedSubscriptions = new SortedSet<EventSubscription<T>>(new SubscriptionComparer<T>());
            }

            return (SortedSet<EventSubscription<T>>) this.sortedSubscriptions;
        }

        /// <summary>
        /// Adds an event subscription to this list and then sorts it.
        /// </summary>
        /// <param name="subscription">The subscription to add to the list.</param>
        /// <returns>true if the subscription was added to the list, false otherwise.</returns>
        public bool AddSubscription<T>(EventSubscription<T> subscription) where T : EventBase {
            if (typeof(T) != this.type) {
                throw new ArgumentException("Attempted to add a subscription to an event list with the incorrect event type.");
            }

            return GetSortedSubscriptions<T>().Add(subscription);
        }

        /// <summary>
        /// Remove the given subscription from the list.
        /// </summary>
        /// <param name="subscription">The subscription to remove.</param>
        /// <returns>true if the subscription was found and removed, false otherwise.</returns>
        public bool RemoveSubscription<T>(EventSubscription<T> subscription) where T : EventBase {
            if (typeof(T) != this.type) {
                throw new ArgumentException("Attempted to remove a subscription from an event list with the incorrect event type.");
            }

            return GetSortedSubscriptions<T>().Remove(subscription);
        }

        /// <summary>
        /// Removes any event subscriptions matching the given predicate.
        /// </summary>
        /// <param name="match">The predicate that determines what subscriptions to remove.</param>
        /// <returns>The total amount of subscriptions that were removed.</returns>
        public int RemoveWhere<T>(Predicate<EventSubscription<T>> match) where T : EventBase {
            if (typeof(T) != this.type) {
                throw new ArgumentException("Attempted to remove subscriptions from an event list with the incorrect event type.");
            }

            return GetSortedSubscriptions<T>().RemoveWhere(match);
        }

        /// <summary>
        /// Tells all subscriptions managed by this list that an event has been fired.
        /// </summary>
        /// <param name="eventFired">The event that was fired.</param>
        public void HandleFiredEvent<T>(T eventFired) where T : EventBase {
            if (typeof(T) != this.type) {
                throw new ArgumentException("Attempted to handle a fired event on an event list with the incorrect event type.");
            }

            foreach (var sortedSubscription in GetSortedSubscriptions<T>()) {
                try {
                    sortedSubscription.HandleFiredEvent(eventFired);
                } catch (Exception e) {
                    DebugScreen.Log($"Failed to handle event {this.type}, caused by subscription by {sortedSubscription.Subscriber.Id}");
                    DebugScreen.Log(e.ToString());
                }
            }
        }

        /// <summary>
        /// Simple comparer that sorts event subscriptions based on their priority.
        /// </summary>
        private class SubscriptionComparer<T> : IComparer<EventSubscription<T>> where T : EventBase {
            public int Compare(EventSubscription<T> x, EventSubscription<T> y) {
                return x.SubPriority - y.SubPriority;
            }
        }
    }
}