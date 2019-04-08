namespace TridevLoader.EventBus {
    /// <summary>
    /// Base class for all events, defines cancellation behaviour and nothing else, all events must extend this class.
    /// </summary>
    public abstract class EventBase {
        /// <summary>
        /// Determines if this event is cancellable.
        /// </summary>
        /// <returns>true if the event can be cancelled, false otherwise.</returns>
        public abstract bool IsCancellable();

        /// <summary>
        /// Determines if the event has been cancelled.
        /// </summary>
        /// <returns>true if the event has been cancelled, false otherwise.</returns>
        public abstract bool IsCancelled();

        /// <summary>
        /// Cancels the event.
        /// </summary>
        public abstract void Cancel();
    }
}