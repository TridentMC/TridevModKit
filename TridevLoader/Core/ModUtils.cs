using RoR2;
using TridevLoader.Mod.API.Item;

namespace TridevLoader.Core {
    public class ModUtils {
        /// <summary>
        /// Gets the mod item tier associated with the game item tier.
        /// </summary>
        /// <param name="tier">The game tier.</param>
        /// <returns>The mod tier associated with the game tier.</returns>
        public static ModItemTier GetModItemTier(ItemTier tier) {
            switch (tier) {
                case ItemTier.Tier1: return ModItemTier.Tier1;
                case ItemTier.Tier2: return ModItemTier.Tier2;
                case ItemTier.Tier3: return ModItemTier.Tier3;
                case ItemTier.Lunar: return ModItemTier.Lunar;
                case ItemTier.Boss: return ModItemTier.Boss;
                case ItemTier.NoTier: return ModItemTier.NoTier;
                default: return ModItemTier.NoTier;
            }
        }
    }
}