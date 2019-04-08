using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TridevLoader.Debug;

namespace TridevLoader.Core {
    /// <summary>
    /// Loads library assemblies from mods/libraries folder, these are simply loaded into memory. Mod discovery is skipped for these assemblies.
    /// </summary>
    public class LibraryDiscoverer {
        public void LoadAssemblies() {
            // Get the directory assemblies are stored in, then gather all the files with a .DLL extension.
            var librariesFolder = Path.Combine(AppContext.BaseDirectory, "mods", "libraries");
            // Create the libraries folder if it doesn't exist.
            if (!Directory.Exists(librariesFolder))
                Directory.CreateDirectory(librariesFolder);
            var libraryFiles = Directory.GetFiles(librariesFolder).Where(x => x.ToUpper().EndsWith(".DLL")).ToList();
            var libraryAssemblies = libraryFiles.Select(x => Assembly.LoadFrom(x)).ToList();

            // Store assemblies based on their name, used to remove duplicate assemblies by looking at their version.
            var duplicateAssemblies = new Dictionary<string, List<Assembly>>();
            foreach (var assembly in libraryAssemblies) {
                var name = assembly.GetName().Name;
                if (!duplicateAssemblies.ContainsKey(name)) {
                    duplicateAssemblies.Add(name, new List<Assembly>());
                }

                duplicateAssemblies[name].Add(assembly);
            }

            // Unloads any duplicate assemblies.
            foreach (var keyValuePair in duplicateAssemblies) {
                Assembly selectedAssembly = null;
                foreach (var assembly in keyValuePair.Value) {
                    if (selectedAssembly == null) {
                        selectedAssembly = assembly;
                    } else {
                        var selectedVersion = selectedAssembly.GetName().Version;
                        var duplicateVersion = assembly.GetName().Version;
                        if (selectedVersion < duplicateVersion) {
                            selectedAssembly = assembly;
                        } else {
                            DebugScreen.Log($"Rejected duplicate library with version {duplicateVersion}," +
                                            $" using {selectedVersion}");
                        }
                    }
                }

                if (selectedAssembly != null) {
                    DebugScreen.Log($"Loaded external library {selectedAssembly.GetName().Name}");
                }
            }
        }
    }
}