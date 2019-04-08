using TridevLoader.Core;
using TridevLoader.Core.Game;

namespace TridevLoader.Mod.API {
    public class ModObjectId {
        private readonly string modId;
        private readonly string name;

        public string ModId => this.modId;

        public Mod Mod => TridevModHelper.Instance.ModList.GetMod(this.modId);

        public string Name => this.name;

        public ModObjectId(string modId, string name) {
            this.modId = modId;
            this.name = name;
        }

        public ModObjectId(string id) {
            if (!id.Contains(":")) {
                // No id specified, default to the vanilla mod.
                this.modId = VanillaMod.Id;
                this.name = id;
            } else {
                var splitId = id.Split(':');
                this.modId = splitId[0];
                this.name = splitId[1];
            }
        }

        public override string ToString() {
            return $"{this.modId}:{this.name}";
        }

        protected bool Equals(ModObjectId other) {
            return string.Equals(this.modId, other.modId) && string.Equals(this.name, other.name);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ModObjectId) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((this.modId != null ? this.modId.GetHashCode() : 0) * 397) ^ (this.name != null ? this.name.GetHashCode() : 0);
            }
        }
    }
}