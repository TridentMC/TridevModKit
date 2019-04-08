using System.IO;
using System.Reflection;
using TridevLoader.Core;
using TridevLoader.Debug;
using TridevLoader.Mod.API.Event;

namespace RoR2 {
    class patch_RoR2Application : RoR2Application {
        private static bool CalledInjection;

        private extern void orig_Awake();

        private void Awake() {
            if (!CalledInjection) {
                var loaderDll = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "mods", "loader", "Tridev-Loader.dll");
                var loaderAssembly = Assembly.LoadFrom(loaderDll);
                var methodInfo = loaderAssembly.GetType("TridevLoader.Core.Injector").GetMethod("OnInject", BindingFlags.Public | BindingFlags.Static);
                methodInfo.Invoke(null, null);
                CalledInjection = true;
            }

            orig_Awake();
        }

        private extern void orig_Start();

        private void Start() {
            DebugScreen.Log("Firing setup.");
            TridevModHelper.Instance.EventBus.FireEvent(new EventSetup());
            orig_Start();
        }
    }
}