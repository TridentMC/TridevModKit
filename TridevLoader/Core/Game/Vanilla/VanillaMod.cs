using System;
using RoR2;
using TridevLoader.EventBus;
using TridevLoader.Mod.API;
using TridevLoader.Mod.API.Event;
using TridevLoader.Mod.API.Item;

namespace TridevLoader.Core.Game {
    [Mod.Mod(Id = "riskofrain2", Name = "Risk Of Rain 2 Game", Author = "Hopoo Games", Version = "1.0.0")]
    public class VanillaMod {
        public static readonly string Id = "riskofrain2";
        public static readonly string Name = "Risk Of Rain 2 Game";
        public static readonly string Author = "Hopoo Games";
        public static readonly string Version = "1.0.0";

        private static VanillaMod RealInstance;
        public static VanillaMod Instance => RealInstance;

        public readonly Mod.Mod mod;

        public VanillaMod(Mod.Mod mod) {
            RealInstance = this;
            this.mod = mod;

            TridevModHelper.Instance.EventBus.AddSubscription<EventSetup>(mod, EventSubscriptionPriority.Highest, e => OnSetup());
        }

        private void OnSetup() {
            TridevModHelper.Instance.EventBus.AddSubscription<EventItemRegistration>(this.mod, EventSubscriptionPriority.Highest, e => OnItemRegistration(e.registry));
        }

        private void OnItemRegistration(ModItemRegistry registry) {
            var itemIndices = Enum.GetValues(typeof(ItemIndex)) as ItemIndex[];
            var itemNames = Enum.GetNames(typeof(ItemIndex));
            for (var i = 0; i < itemIndices.Length; i++) {
                var itemIndex = itemIndices[i];
                var name = itemNames[i];
                var itemDef = ItemCatalog.GetItemDef(itemIndex);
                var itemTier = ModUtils.GetModItemTier(itemDef?.tier ?? ItemTier.NoTier);
                registry.RegisterItem(this.mod, new ItemStub(new ModObjectId(Id, name), itemTier));
            }
        }
    }
}