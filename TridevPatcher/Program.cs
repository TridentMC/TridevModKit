using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace TridevPatcher {
    internal class Program {
        internal class PathNames {
            internal string Managed = Path.Combine("Risk of Rain 2_Data", "Managed");
            internal string Vanilla = Path.Combine("assemblies", "vanilla");
            internal string Modded = Path.Combine("assemblies", "modded");
            internal string MonoMod = Path.Combine("tools", "monomod");
            internal string PatchStorage = Path.Combine("tools", "patches");
            internal string Mods = Path.Combine("mods");
            internal string ModLibraries = Path.Combine("mods", "libraries");
            internal string ModLoaders = Path.Combine("mods", "loader");
        }

        internal class ActivePaths {
            internal string Installation;
            internal string GameLauncher;
            internal string Managed;
            internal string Vanilla;
            internal string Modded;
            internal string MonoMod;
            internal string PatchStorage;
            internal string Mods;
            internal string ModLibraries;
            internal string ModLoaders;
            internal string DepDirs;
        }

        public static readonly string GameName = "Risk of Rain 2";

        public static PathNames pathNames = new PathNames();
        public static ActivePaths activePaths = new ActivePaths();

        public static Dictionary<string, List<string>> TargetAssemblies = new Dictionary<string, List<string>>();

        public static Assembly AsmMonoMod;
        public static Assembly AsmHookGen;

        public static void Main(string[] args) {
            Console.WriteLine($"Tridev Patcher - Targeting {GameName}");

            var restoreVanilla = args.Any(x => x.ToUpper().Equals("RESTOREVANILLA"));

            SetupPaths();
            if (restoreVanilla) {
                Console.WriteLine($"Restoring vanilla install from {activePaths.Vanilla}");
                CopyAssemblies(activePaths.Vanilla, activePaths.Managed);
                LaunchGame();
                return;
            }

            try {
                GatherPatchTargets();

                if (AsmMonoMod == null || AsmHookGen == null) {
                    LoadMonoMod();
                }

                if (TargetAssemblies.Count > 0) {
                    foreach (var target in TargetAssemblies.Keys) {
                        try {
                            PatchAndStore(target);
                            CreateHookAssembly(target);
                        } catch (Exception e) {
                            Console.WriteLine("================================================================");
                            Console.WriteLine($"Failed to patch assembly, {target}");
                            Console.WriteLine(e.ToString());
                            Console.WriteLine("================================================================");
                        }
                    }
                } else {
                    Console.WriteLine("No assemblies were targeted during the discovery process, launching the game with modded assemblies.");
                }

                CopyAssemblies(activePaths.Modded, activePaths.Managed);
                LaunchGame();
            } catch (Exception e) {
                Console.WriteLine("");
                Console.WriteLine(e.ToString());
                Console.WriteLine("");
                Console.WriteLine("Failed to patch game, exiting.");
            }
        }

        /// <summary>
        /// Sets up all the paths that are used in patching and launching the game.
        /// Writes to the console if files are missing.
        /// </summary>
        public static void SetupPaths() {
            activePaths.Installation = Directory.GetCurrentDirectory();

            activePaths.GameLauncher = Path.Combine(activePaths.Installation, $"{GameName}.exe");
            activePaths.Managed = Path.Combine(activePaths.Installation, pathNames.Managed);
            activePaths.Vanilla = Path.Combine(activePaths.Installation, pathNames.Vanilla);
            activePaths.Modded = Path.Combine(activePaths.Installation, pathNames.Modded);
            activePaths.MonoMod = Path.Combine(activePaths.Installation, pathNames.MonoMod);
            activePaths.PatchStorage = Path.Combine(activePaths.Installation, pathNames.PatchStorage);
            activePaths.Mods = Path.Combine(activePaths.Installation, pathNames.Mods);
            activePaths.ModLibraries = Path.Combine(activePaths.Installation, pathNames.ModLibraries);
            activePaths.ModLoaders = Path.Combine(activePaths.Installation, pathNames.ModLoaders);

            activePaths.DepDirs = string.Join(Path.PathSeparator.ToString(),
                activePaths.Managed,
                activePaths.ModLoaders,
                activePaths.ModLibraries,
                activePaths.Mods,
                activePaths.MonoMod);

            if (!File.Exists(activePaths.GameLauncher)) {
                // Not strictly required, just means we won't launch the game after patching.
                Console.WriteLine("Failed to find game launcher, the launch step will be skipped.");
                Console.WriteLine($"Expected to find at {activePaths.GameLauncher}");
            }

            if (!Directory.Exists(activePaths.Managed)) {
                Console.WriteLine("Unable to find managed directory.");
                Console.WriteLine($"Expected to find at {activePaths.Managed}");
            }

            if (!Directory.Exists(activePaths.MonoMod)) {
                Console.WriteLine("Unable to find MonoMod directory, failure is expected.");
                Console.WriteLine($"Expected to find at {activePaths.MonoMod}");
            }

            if (!Directory.Exists(activePaths.Vanilla)) {
                Directory.CreateDirectory(activePaths.Vanilla);
                Console.WriteLine("No vanilla backup folder found, created one.");
            }

            if (!Directory.Exists(activePaths.Modded)) {
                Directory.CreateDirectory(activePaths.Modded);
                Console.WriteLine("No modded backup folder found, created one.");
            }

            if (!Directory.Exists(activePaths.PatchStorage)) {
                Directory.CreateDirectory(activePaths.PatchStorage);
                Console.WriteLine("Unable to find patch directory creating empty directory, nothing will happen.");
                Console.WriteLine($"Expected to find at {activePaths.PatchStorage}");
            }

            if (!Directory.Exists(activePaths.Mods)) {
                Directory.CreateDirectory(activePaths.Mods);
                Console.WriteLine("No mods folder was found, created one.");
            }

            if (!Directory.Exists(activePaths.ModLibraries)) {
                Directory.CreateDirectory(activePaths.ModLibraries);
                Console.WriteLine("No mod library folder was found, created one.");
            }

            if (!Directory.Exists(activePaths.ModLoaders)) {
                Directory.CreateDirectory(activePaths.ModLoaders);
                Console.WriteLine("No mod loader folder was found, created one.");
            }
        }

        /// <summary>
        /// Checks for patch files and their target assemblies, then creates assembly backups.
        /// </summary>
        public static void GatherPatchTargets() {
            var patches = Directory.GetFiles(activePaths.PatchStorage, "*.mm.dll");
            Console.WriteLine($"Found {patches.Length} patch files.");
            if (patches.Length == 0) {
                Console.WriteLine("If you expected patching to occur please check the following directory,");
                Console.WriteLine(activePaths.PatchStorage);
                return;
            }

            // I don't know how to regex, don't get mad.
            var regex = new Regex(@"^(.*)\..*\.mm\.dll");
            foreach (var patch in patches) {
                var patchName = Path.GetFileName(patch);
                var targetAssembly = regex.Match(patchName).Groups[1] + ".dll";

                BackupOrUpdateAssembly(targetAssembly);
                if (!TargetAssemblies.ContainsKey(targetAssembly)) {
                    TargetAssemblies.Add(targetAssembly, new List<string>());
                }

                TargetAssemblies[targetAssembly].Add(patchName);
            }
        }

        /// <summary>
        /// Checks if the current vanilla assembly backup is up to date and creates one if it's not.
        /// </summary>
        /// <param name="name">The name of the assembly to backup.</param>
        public static void BackupOrUpdateAssembly(string assemblyName) {
            var vanillaAssemblyName = Path.Combine(activePaths.Vanilla, assemblyName);
            var moddedAssemblyName = Path.Combine(activePaths.Modded, assemblyName);
            var installedAssemblyName = Path.Combine(activePaths.Managed, assemblyName);
            var doBackup = !File.Exists(vanillaAssemblyName);
            if (!doBackup) {
                // There's a vanilla asset already backed up
                // Check if we need a new backup by comparing the file hash of the currently installed version against our modded version.

                if (File.Exists(moddedAssemblyName)) {
                    var moddedHash = MD5.Create().ComputeHash(File.ReadAllBytes(moddedAssemblyName));
                    var installedHash = MD5.Create().ComputeHash(File.ReadAllBytes(installedAssemblyName));

                    // The current assembly is not the modded one, compare to the vanilla backup and update if needed.
                    if (moddedHash != installedHash) {
                        var vanillaHash = MD5.Create().ComputeHash(File.ReadAllBytes(vanillaAssemblyName));
                        if (vanillaHash != installedHash) {
                            // The installed assembly is not the same as the current vanilla backup, create a new backup.
                            doBackup = true;
                        }
                    }
                }
            }

            if (doBackup) {
                Console.WriteLine($"Created backup of {assemblyName}");
                File.Copy(installedAssemblyName, vanillaAssemblyName, true);
            } else {
                Console.WriteLine($"Skipping backup for {assemblyName}, current backup is already up to date.");
            }
        }

        /// <summary>
        /// Loads the mono mod assemblies from the ActivePath folder.
        /// </summary>
        public static void LoadMonoMod() {
            Console.WriteLine("Loading Mono.Cecil");
            LazyLoadAssembly(Path.Combine(activePaths.MonoMod, "Mono.Cecil.dll"));
            Console.WriteLine("Loading Mono.Cecil.Mdb");
            LazyLoadAssembly(Path.Combine(activePaths.MonoMod, "Mono.Cecil.Mdb.dll"));
            Console.WriteLine("Loading Mono.Cecil.Pdb");
            LazyLoadAssembly(Path.Combine(activePaths.MonoMod, "Mono.Cecil.Pdb.dll"));
            Console.WriteLine("Loading MonoMod.Utils.dll");
            LazyLoadAssembly(Path.Combine(activePaths.MonoMod, "MonoMod.Utils.dll"));
            Console.WriteLine("Loading MonoMod");
            AsmMonoMod = LazyLoadAssembly(Path.Combine(activePaths.MonoMod, "MonoMod.exe"));
            Console.WriteLine("Loading MonoMod.RuntimeDetour.dll");
            LazyLoadAssembly(Path.Combine(activePaths.MonoMod, "MonoMod.RuntimeDetour.dll"));
            Console.WriteLine("Loading MonoMod.RuntimeDetour.HookGen");
            AsmHookGen = LazyLoadAssembly(Path.Combine(activePaths.MonoMod, "MonoMod.RuntimeDetour.HookGen.exe"));
        }

        /// <summary>
        /// Patches the given assembly moves it to the modded folder and deletes any of the patch files that were used.
        /// </summary>
        /// <param name="targetAssembly">The name of the assembly to patch.</param>
        /// <exception cref="Exception">Throws an exception if MonoMod fails to patch the assembly.</exception>
        public static void PatchAndStore(string targetAssembly) {
            Console.WriteLine($"Running MonoMod for {targetAssembly}");
            var targetLocation = Path.Combine(activePaths.Vanilla, targetAssembly);
            var outputLocation = Path.Combine(activePaths.Modded, targetAssembly);

            // We're lazy.
            Environment.SetEnvironmentVariable("MONOMOD_DEPDIRS", activePaths.DepDirs);
            Environment.SetEnvironmentVariable("MONOMOD_MODS", activePaths.PatchStorage);
            Environment.SetEnvironmentVariable("MONOMOD_DEPENDENCY_MISSING_THROW", "0");
            AsmMonoMod.EntryPoint.Invoke(null, new object[] {new[] {targetLocation, outputLocation + ".tmp"}});

            if (!File.Exists(outputLocation + ".tmp"))
                throw new Exception("MonoMod failed creating a patched assembly!");

            if (File.Exists(outputLocation))
                File.Delete(outputLocation);

            foreach (var patch in TargetAssemblies[targetAssembly].Select(x => Path.Combine(activePaths.PatchStorage, x))) {
                if (File.Exists(patch)) {
                    Console.WriteLine($"Removing used patch file {patch}");
                    File.Delete(patch);
                }
            }

            File.Move(outputLocation + ".tmp", outputLocation);
        }

        /// <summary>
        /// Generates a mmhook assembly and places it in the mod library folder.
        /// </summary>
        /// <param name="targetAssembly">The name of the assembly to generate hooks for.</param>
        public static void CreateHookAssembly(string targetAssembly) {
            var targetLocation = Path.Combine(activePaths.Modded, targetAssembly);
            var outputLocation = Path.Combine(activePaths.ModLibraries, $"HOOKS_{targetAssembly}");
            Console.WriteLine($"Running MonoMod.RuntimeDetour.HookGen for {targetAssembly}");
            Environment.SetEnvironmentVariable("MONOMOD_DEPDIRS", activePaths.DepDirs);
            Environment.SetEnvironmentVariable("MONOMOD_DEPENDENCY_MISSING_THROW", "0");
            AsmHookGen.EntryPoint.Invoke(null, new object[] {new[] {"--private", targetLocation, outputLocation}});
        }

        /// <summary>
        /// Copies all the assemblies in the sourceDirectory to the targetDirectory.
        ///
        /// This does not change the assemblies already present in the sourceDirectory.
        /// </summary>
        /// <param name="sourceDirectory">The directory to copy all the assemblies from.</param>
        /// <param name="targetDirectory">The directory to copy all the assemblies to.</param>
        public static void CopyAssemblies(string sourceDirectory, string targetDirectory) {
            var sourceFiles = Directory.GetFiles(sourceDirectory);
            foreach (var sourceFile in sourceFiles) {
                var fileName = Path.GetFileName(sourceFile);
                var targetFile = Path.Combine(targetDirectory, fileName);
                if (File.Exists(targetFile)) {
                    File.Copy(sourceFile, targetFile, true);
                    Console.WriteLine($"Copied assembly to game install: {fileName}");
                } else {
                    Console.WriteLine($"Found no matching assembly in the game installation folder for {fileName}, it will be skipped.");
                }
            }
        }

        /// <summary>
        /// Launches the game after patching or restoration has occured.
        /// </summary>
        public static void LaunchGame() {
            Console.WriteLine($"Launching {GameName}");
            var game = new Process();
            // If the game was installed via Steam, it should restart in a Steam context on its own.
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX) {
                // The Linux and macOS versions come with a wrapping bash script.
                game.StartInfo.FileName = activePaths.GameLauncher.Substring(0, activePaths.GameLauncher.Length - 4);
                if (!File.Exists(game.StartInfo.FileName))
                    game.StartInfo.FileName = activePaths.GameLauncher.Substring(0, activePaths.GameLauncher.Length - 4);
            } else {
                game.StartInfo.FileName = activePaths.GameLauncher;
            }

            game.StartInfo.WorkingDirectory = activePaths.Installation;
            game.Start();
        }

        /// <summary>
        /// Loads the assembly at the provided path.
        /// </summary>
        /// <param name="assemblyPath">The path of the assembly.</param>
        /// <returns>The loaded assembly.</returns>
        private static Assembly LazyLoadAssembly(string assemblyPath) {
            Console.WriteLine($"Lazily loading {assemblyPath}");
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
            ResolveEventHandler resolveHandler = (sender, args) => {
                var asmPath = Path.Combine(assemblyDirectory, new AssemblyName(args.Name).Name + ".dll");
                if (!File.Exists(asmPath)) {
                    asmPath = Path.Combine(assemblyDirectory, new AssemblyName(args.Name).Name + ".exe");
                }

                return !File.Exists(asmPath) ? null : Assembly.LoadFrom(asmPath);
            };
            AppDomain.CurrentDomain.AssemblyResolve += resolveHandler;
            var asm = Assembly.Load(Path.GetFileNameWithoutExtension(assemblyPath));
            AppDomain.CurrentDomain.AssemblyResolve -= resolveHandler;
            AppDomain.CurrentDomain.TypeResolve += (s, e) => asm.GetType(e.Name) != null ? asm : null;
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) => e.Name == asm.FullName || e.Name == asm.GetName().Name ? asm : null;
            return asm;
        }
    }
}