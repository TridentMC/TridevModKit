using System.Reflection;

namespace TridevLoader.Core {
    public class ModContainer {
        private readonly Assembly modAssembly;
        private readonly Mod.Mod modDetails;
        private readonly object modInstance;

        public ModContainer(Assembly modAssembly, Mod.Mod modDetails, object modInstance) {
            this.modAssembly = modAssembly;
            this.modDetails = modDetails;
            this.modInstance = modInstance;
        }

        public Assembly ModAssembly => this.modAssembly;

        public Mod.Mod ModDetails => this.modDetails;

        public object ModInstance => this.modInstance;
    }
}