namespace TridevLoader.EventBus {
    /// <summary>
    /// Base class for cancellable events, just implements the default cancel behaviour.
    /// </summary>
    public class EventCancellable : EventBase {
        private bool isCancelled;

        public override bool IsCancellable() {
            return true;
        }

        public override bool IsCancelled() {
            return this.isCancelled;
        }

        public override void Cancel() {
            this.isCancelled = true;
        }
    }
}