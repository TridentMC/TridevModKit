namespace TridevLoader.EventBus {
    /// <summary>
    /// Standard non-cancellable event implementation. Suitable for most use cases.
    /// </summary>
    public class EventStandard : EventBase {
        public override bool IsCancellable() {
            return false;
        }

        public override bool IsCancelled() {
            return false;
        }

        public override void Cancel() {
            // NO-OP
        }
    }
}