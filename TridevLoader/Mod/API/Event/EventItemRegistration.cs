using TridevLoader.EventBus;
using TridevLoader.Mod.API.Item;

namespace TridevLoader.Mod.API.Event {
    /// <summary>
    /// Called during the item registration phase, use the provided registry reference to add any mod items.
    /// </summary>
    public class EventItemRegistration : EventStandard {
        public readonly ModItemRegistry registry;

        public EventItemRegistration(ModItemRegistry registry) {
            this.registry = registry;
        }
    }
}