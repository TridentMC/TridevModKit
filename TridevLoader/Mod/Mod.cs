using System;

namespace TridevLoader.Mod {
    /// <summary>
    /// Used to define a given class as a mod, requires that the class has a constructor containing only a reference to this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class Mod : Attribute {
        /// <summary>
        /// Defines the mod id, must be all lower case.
        ///
        /// For example: "riskofrain2"
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Defines the mod name in a user-friendly format.
        ///
        /// For example: "Risk Of Rain 2"
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Defines the mod version in a format that can be converted to a Version object.
        ///
        /// For example: "1.0.0"
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// Defines the mod author(s) in a user-friendly format.
        ///
        /// For example: "Hopoo Games"
        /// </summary>
        public virtual string Author { get; set; }

        /// <summary>
        /// Finds and gets the mod attribute on the given type. 
        /// </summary>
        /// <param name="t">The type to get the mod attribute from.</param>
        /// <returns>The mod attribute on the given class, or null if none is found.</returns>
        public static Mod GetMod(Type t) {
            return (Mod) GetCustomAttribute(t, typeof(Mod));
        }
    }
}