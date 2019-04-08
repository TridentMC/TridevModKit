using TridevLoader.EventBus;

namespace TridevLoader.Mod.API.Event {
    /// <summary>
    /// Called after all mods have been injected, used for anything that can't be done directly in the mod class constructor.
    /// </summary>
    public class EventSetup : EventStandard {
    }
}