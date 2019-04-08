using RoR2;

namespace TridevLoader.Mod.API.Character {
    /// <summary>
    /// Represents the Character level stats, used in place of direct CharacterBody references.
    /// </summary>
    public class CharacterLevelStats {
        public CharacterLevelStats(CharacterBody characterBody) {
            LevelMaxHealth = characterBody.levelMaxHealth;
            LevelRegen = characterBody.levelRegen;
            LevelMaxShield = characterBody.levelMaxShield;
            LevelMoveSpeed = characterBody.levelMoveSpeed;
            LevelJumpPower = characterBody.levelJumpPower;
            LevelDamage = characterBody.levelDamage;
            LevelAttackSpeed = characterBody.levelAttackSpeed;
            LevelCrit = characterBody.levelCrit;
            LevelArmor = characterBody.levelArmor;
        }

        public float LevelMaxHealth { get; set; }
        public float LevelRegen { get; set; }
        public float LevelMaxShield { get; set; }
        public float LevelMoveSpeed { get; set; }
        public float LevelJumpPower { get; set; }
        public float LevelDamage { get; set; }
        public float LevelAttackSpeed { get; set; }
        public float LevelCrit { get; set; }
        public float LevelArmor { get; set; }
    }
}