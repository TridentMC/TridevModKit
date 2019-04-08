using RoR2;

namespace TridevLoader.Mod.API.Item {
    public class ModItem {
        public ModObjectId id;
        private ModItemTier tier;

        public ModItem(ModObjectId id, ModItemTier tier) {
            this.id = id;
            this.tier = tier;
        }

        public void OnAddedToPlayer(CharacterBody character, int count) {
        }

        public void OnRemovedFromPlayer(CharacterBody character, int count) {
        }

        public void OnPlayerStatCalculation(CharacterBody character, int count) {
        }

        public void OnPlayerLevelUp(CharacterBody character, int count) {
        }

        /// <summary>
        /// Determines if the item is hidden from the player's UI, defaults to false.
        /// </summary>
        /// <returns>true if the item is hidden, false otherwise.</returns>
        public bool IsHidden() {
            return false;
        }

        /// <summary>
        /// Determines if the item can be removed by the player, defaults to true.
        /// </summary>
        /// <returns>true if the item can be removed by the player, false otherwise.</returns>
        public bool CanRemove() {
            return true;
        }
    }
}