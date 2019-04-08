using System.Collections.Generic;

namespace TridevLoader.Core {
    public class ModList {
        private readonly Dictionary<string, Mod.Mod> modsById = new Dictionary<string, Mod.Mod>();
        private readonly Dictionary<Mod.Mod, ModContainer> modContainers = new Dictionary<Mod.Mod, ModContainer>();

        protected internal ModList(ModDiscoverer modDiscoverer) {
            foreach (var container in modDiscoverer.modContainers) {
                this.modsById.Add(container.ModDetails.Id, container.ModDetails);
                this.modContainers.Add(container.ModDetails, container);
            }
        }

        /// <summary>
        /// Checks if a mod with the given id is currently loaded.
        /// </summary>
        /// <param name="id">The id of the mod to check for.</param>
        /// <returns>true if the mod is loaded, false otherwise.</returns>
        public bool IsModLoaded(string id) {
            return this.modsById.ContainsKey(id);
        }

        /// <summary>
        /// Gets the mod info if a given mod is present.
        /// </summary>
        /// <param name="id">The id of the mod to get.</param>
        /// <returns>The mod if it's loaded, or null if none was found.</returns>
        public Mod.Mod GetMod(string id) {
            return IsModLoaded(id) ? this.modsById[id] : null;
        }

        /// <summary>
        /// Gets the mod container associated with the given mod id if the mod is currently loaded.
        /// </summary>
        /// <param name="id">The id of the mod to get.</param>
        /// <returns>The mod container if it's loaded, or null if none was found.</returns>
        public ModContainer GetModContainer(string id) {
            var mod = GetMod(id);
            return mod != null ? GetModContainer(mod) : null;
        }

        /// <summary>
        /// Gets the mod container associated with the given mod if the mod is currently loaded.
        /// </summary>
        /// <param name="mod">The mod to get.</param>
        /// <returns>The mod container if it's loaded, or null if none was found.</returns>
        public ModContainer GetModContainer(Mod.Mod mod) {
            return this.modContainers.ContainsKey(mod) ? this.modContainers[mod] : null;
        }
    }
}