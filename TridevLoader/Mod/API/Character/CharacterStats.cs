using RoR2;

namespace TridevLoader.Mod.API.Character {
    /// <summary>
    /// Represents the Character stats, used in place of direct CharacterBody references.
    /// </summary>
    public class CharacterStats {
        public CharacterStats(CharacterBody characterBody) {
            Experience = characterBody.experience;
            Level = characterBody.level;
            MaxHealth = characterBody.maxHealth;
            Regen = characterBody.regen;
            MaxShield = characterBody.maxShield;
            MoveSpeed = characterBody.moveSpeed;
            Acceleration = characterBody.acceleration;
            JumpPower = characterBody.jumpPower;
            MaxJumpCount = characterBody.maxJumpCount;
            MaxJumpHeight = characterBody.maxJumpHeight;
            Damage = characterBody.damage;
            AttackSpeed = characterBody.attackSpeed;
            Crit = characterBody.crit;
            Armor = characterBody.armor;
            CritHeal = characterBody.critHeal;
        }

        public float Experience { get; set; }

        public float Level { get; set; }

        public float MaxHealth { get; set; }

        public float Regen { get; set; }

        public float MaxShield { get; set; }

        public float MoveSpeed { get; set; }

        public float Acceleration { get; set; }

        public float JumpPower { get; set; }

        public int MaxJumpCount { get; set; }

        public float MaxJumpHeight { get; set; }

        public float Damage { get; set; }

        public float AttackSpeed { get; set; }

        public float Crit { get; set; }

        public float Armor { get; set; }

        public float CritHeal { get; set; }
    }
}