using TridevLoader.Debug;
using TridevLoader.Mod.API.Event;
using TridevLoader.Mod.API.Item;

namespace TridevLoader.Core {
    public class ModLoader {
        private static ModList ModList;
        private static readonly EventBus.EventBus EventBus = new EventBus.EventBus();
        private static readonly ModDiscoverer Discoverer = new ModDiscoverer();
        private static readonly LibraryDiscoverer LibraryDiscoverer = new LibraryDiscoverer();

        public static void OnInject() {
            DebugScreen.Instantiate();
            DebugScreen.Log("Tridev Loader started!");

            LoadMods();
        }

        public static void LoadMods() {
            // Create a temp instance so the event bus is available during mod construction.
            TridevModHelper.realInstance = new TridevModHelper(ModList, EventBus);

            // Load mods.
            LibraryDiscoverer.LoadAssemblies();
            Discoverer.LoadAssemblies();

            // Create the mod list, and helper. Then fire the setup event.
            ModList = new ModList(Discoverer);
            TridevModHelper.realInstance = new TridevModHelper(ModList, EventBus);
            TridevModHelper.Instance.EventBus.FireEvent(new EventSetup());
            TridevModHelper.Instance.EventBus.FireEvent(new EventItemRegistration(ModItemRegistry.Instance));
        }
    }
}