# Tridev Patcher
A simple unity patcher that targets Risk of Rain 2 using [MonoMod](https://http://github.com/0x0ade/MonoMod), based on the MiniInstaller for the [EverestAPI](https://github.com/EverestAPI/Everest/tree/master/MiniInstaller).

### How do I run it?
Tridev patcher relies on a specific folder layout to patch a game. The patcher itself should be stored in the root directory of the game that's being patched. 
- An installation of MonoMod should be placed in ```tools/monomod```
- MonoMod is configured to scan ```tools/patches``` for any patch files to apply.
- Any libraries required for the patches should be stored in ```mods/libraries```, ```mods/loader``` and ```mods```

### Where are the runtime hooks stored?
If the mod API you're using doesn't have an event for what you need to do then you can create your own patch by using a generated hook assembly. All hook assemblies are stored in ```mods/libraries/HOOKS_[Assembly].dll```

### How do I restore my vanilla installation?
If the platform you installed the game from doesn't support install validation then simply run the patcher with the argument ```restoreVanilla```

#### Credits
[0x0ade - For MonoMod](https://github.com/0x0ade/), [Everest API Team and Contributors - For MiniInstaller](https://github.com/EverestAPI)