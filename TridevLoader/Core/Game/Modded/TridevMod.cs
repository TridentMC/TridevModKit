namespace TridevLoader.Core.Game.Modded {
    [Mod.Mod(Id = "tridevmodapi", Name = "Tridev Mod API", Author = "darkevilmac", Version = "1.0.0")]
    public class TridevMod {
        public static readonly string Id = "tridevmodapi";
        public static readonly string Name = "Tridev Mod API";
        public static readonly string Author = "darkevilmac";
        public static readonly string Version = "1.0.0";

        public readonly Mod.Mod mod;
        
        public TridevMod(Mod.Mod mod) {
            this.mod = mod;
        }
    }
}