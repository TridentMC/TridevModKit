namespace TridevLoader.EventBus {
    /// <summary>
    /// Defines event subscriptions from lowest to highest priority. Higher values run first, lower values run last.
    /// </summary>
    public enum EventSubscriptionPriority {
        Lowest,
        Low,
        Normal,
        High,
        Highest
    }
}