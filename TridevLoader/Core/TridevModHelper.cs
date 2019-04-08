namespace TridevLoader.Core {
    /// <summary>
    /// Contains references to static instances of classes that can be used by mods to interact with each other or the game.
    /// </summary>
    public class TridevModHelper {
        protected internal static TridevModHelper realInstance;
        public static TridevModHelper Instance => realInstance;

        public TridevModHelper(ModList modList, EventBus.EventBus eventBus) {
            ModList = modList;
            EventBus = eventBus;
        }

        public ModList ModList { get; }
        public EventBus.EventBus EventBus { get; }
    }
}