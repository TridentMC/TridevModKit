using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TridevLoader.Core.Game;
using TridevLoader.Core.Game.Modded;
using TridevLoader.Debug;

namespace TridevLoader.Core {
    public class ModDiscoverer {
        protected internal List<ModContainer> modContainers = new List<ModContainer>();
        protected internal Dictionary<Type, Mod.Mod> modTypes = new Dictionary<Type, Mod.Mod>();

        public void LoadAssemblies() {
            var modsFolder = Path.Combine(AppContext.BaseDirectory, "mods");
            var modFiles = Directory.GetFiles(modsFolder).Where(x => x.ToUpper().EndsWith(".DLL")).ToList();
            var modAssemblies = modFiles.Select(x => Assembly.LoadFrom(x)).ToList();
            var discoveredMods = new Dictionary<string, List<ModData>>();
            foreach (var assembly in modAssemblies) {
                var validModTypes = assembly.GetTypes()
                    .Select(t => new ModData {assembly = assembly, type = t, mod = Mod.Mod.GetMod(t)})
                    .ToList();
                validModTypes
                    .Where(m => m.mod != null)
                    .ToList()
                    .ForEach(m => {
                        if (!discoveredMods.ContainsKey(m.mod.Id))
                            discoveredMods.Add(m.mod.Id, new List<ModData>());
                        discoveredMods[m.mod.Id].Add(m);
                    });
            }

            // We don't scan this assembly, so we load the vanilla and tridev mod manually.
            LoadVanillaMod();
            LoadModdedMod();

            foreach (var modDataList in discoveredMods.Values) {
                var selectedMod = modDataList.First();
                if (modDataList.Count > 1) {
                    foreach (var modData in modDataList) {
                        var selectedVersion = new Version(selectedMod.mod.Version);
                        var duplicateVersion = new Version(modData.mod.Version);
                        if (selectedVersion < duplicateVersion) {
                            selectedMod = modData;
                        } else {
                            DebugScreen.Log($"Rejected duplicate mod with version {duplicateVersion}," +
                                            $" using {selectedVersion}");
                        }
                    }
                }

                LoadMod(selectedMod);
            }
        }

        private void LoadMod(ModData mod) {
            var container = CreateContainer(mod);
            if (container != null) {
                this.modTypes.Add(mod.type, mod.mod);
                this.modContainers.Add(container);
                DebugScreen.Log($"Loaded mod {mod.mod.Name}" +
                                $" Version {mod.mod.Version}" +
                                $" by {mod.mod.Author}");
            } else {
                DebugScreen.Log($"Failed to load mod {mod.mod.Name}" +
                                $" Version {mod.mod.Version}" +
                                $" by {mod.mod.Author}");
            }
        }

        private ModContainer CreateContainer(ModData modData) {
            try {
                var instance = Activator.CreateInstance(modData.type, modData.mod);
                return new ModContainer(modData.assembly, modData.mod, instance);
            } catch (Exception e) {
                DebugScreen.Log($"Failed to create mod instance for mod {modData.mod.Id}");
                DebugScreen.Log(e.ToString());
            }

            return null;
        }

        private void LoadVanillaMod() {
            var vanillaType = typeof(VanillaMod);
            LoadMod(new ModData {
                assembly = vanillaType.Assembly,
                type = vanillaType,
                mod = Mod.Mod.GetMod(vanillaType)
            });
        }

        private void LoadModdedMod() {
            var moddedType = typeof(TridevMod);
            LoadMod(new ModData {
                assembly = moddedType.Assembly,
                type = moddedType,
                mod = Mod.Mod.GetMod(moddedType)
            });
        }

        private struct ModData {
            internal Assembly assembly;
            internal Type type;
            internal Mod.Mod mod;
        }
    }
}