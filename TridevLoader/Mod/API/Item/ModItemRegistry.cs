using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TridevLoader.Debug;

namespace TridevLoader.Mod.API.Item {
    public class ModItemRegistry {
        public static readonly ModItemRegistry Instance = new ModItemRegistry();
        private List<ModItemDefinition> registeredItems = new List<ModItemDefinition>();
        private Dictionary<ModObjectId, int> itemIds = new Dictionary<ModObjectId, int>();
        private List<ModObjectId> itemIdList = new List<ModObjectId>();

        /// <summary>
        /// Adds the given item to the item registry.
        /// </summary>
        /// <param name="owner">The mod associated with the registered item.</param>
        /// <param name="item">The item to register.</param>
        /// <returns>The item that was registered, or null if registration failed.</returns>
        public T RegisterItem<T>(Mod owner, T item) where T : ModItem {
            var itemDef = new ModItemDefinition(owner, item.id, item);
            if (IsItemPresent(itemDef.Id)) {
                throw new Exception($"Attempted to register an item with the id '${itemDef.Id}' but it was already registered.");
            }

            this.itemIds.Add(itemDef.Id, this.itemIds.Count);
            this.itemIdList.Insert(this.itemIds.Count - 1, itemDef.Id);
            this.registeredItems.Insert(this.itemIds.Count - 1, itemDef);

            DebugScreen.Log($"Registered new item with id {itemDef.Id}");

            return item;
        }

        /// <summary>
        /// Checks if an item with the given id is present in the registry.
        /// </summary>
        /// <param name="id">The id of the item to search for.</param>
        /// <returns>true if the item was found in the registry, false otherwise.</returns>
        public bool IsItemPresent(ModObjectId id) {
            return this.itemIds.ContainsKey(id);
        }

        /// <summary>
        /// Gets an item with the given id in the registry, or null if none is found.
        /// </summary>
        /// <param name="id">The id of the item to search for.</param>
        /// <returns>The item with the given id, or null if none was found.</returns>
        public ModItem GetItem(ModObjectId id) {
            var itemId = this.itemIds.GetValueOrDefault(id, -1);
            return itemId == -1 ? null : this.registeredItems[itemId].Item;
        }

        /// <summary>
        /// Gets a read only collection of all the registered item ids.
        /// </summary>
        /// <returns>A read only collection of all the registered item ids, sorted by their effective index.</returns>
        public ReadOnlyCollection<ModObjectId> GetItemIds() {
            return new ReadOnlyCollection<ModObjectId>(this.itemIdList);
        }

        private struct ModItemDefinition {
            public Mod Owner { get; }
            public ModObjectId Id { get; }
            public ModItem Item { get; }

            public ModItemDefinition(Mod owner, ModObjectId id, ModItem item) {
                Owner = owner;
                Id = id;
                Item = item;
            }
        }
    }
}