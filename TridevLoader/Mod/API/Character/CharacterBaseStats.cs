using RoR2;

namespace TridevLoader.Mod.API.Character {
    /// <summary>
    /// Represents the Character base stats, used in place of direct CharacterBody references.
    /// </summary>
    public class CharacterBaseStats {
        public CharacterBaseStats(CharacterBody characterBody) {
            BaseMaxHealth = characterBody.baseMaxHealth;
            BaseRegen = characterBody.baseRegen;
            BaseMaxShield = characterBody.baseMaxShield;
            BaseMoveSpeed = characterBody.baseMoveSpeed;
            BaseAcceleration = characterBody.baseAcceleration;
            BaseJumpPower = characterBody.baseJumpPower;
            BaseJumpCount = characterBody.baseJumpCount;
            BaseDamage = characterBody.baseDamage;
            BaseAttackSpeed = characterBody.baseAttackSpeed;
            BaseCrit = characterBody.baseCrit;
            BaseArmor = characterBody.baseArmor;
        }

        public float BaseMaxHealth { get; set; }
        public float BaseRegen { get; set; }
        public float BaseMaxShield { get; set; }
        public float BaseMoveSpeed { get; set; }
        public float BaseAcceleration { get; set; }
        public float BaseJumpPower { get; set; }
        public float BaseJumpCount { get; set; }
        public float BaseDamage { get; set; }
        public float BaseAttackSpeed { get; set; }
        public float BaseCrit { get; set; }
        public float BaseArmor { get; set; }
    }
}