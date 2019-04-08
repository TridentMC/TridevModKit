using TridevLoader.Mod.API;
using TridevLoader.Mod.API.Item;

namespace TridevLoader.Core.Game {
    /// <summary>
    /// Stub item used to represent vanilla items, no actual behaviour.
    /// </summary>
    public class ItemStub : ModItem {
        public ItemStub(ModObjectId id, ModItemTier tier) : base(id, tier) {
        }
    }
}