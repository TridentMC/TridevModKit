# Tridev Modkit
A dead simple patcher, loader, and API for [Risk of Rain 2 by Hopoo Games](http://hopoogames.com/)

### For Users
Download the latest version of the modkit from the releases page and extract the archive in your game folder, install mods by adding them to the mods folder inside the game installation. That's it, you're done!

### For Developers
**To create a development workspace you must already own a copy of Risk of Rain 2**, you'll need to create a new project in your IDE of choice, preferably an IDE with support for the Unity Engine. First, download the latest version of the modkit from the releases page and extract it into your game installation. Then run the patcher to generate the patched game assembly, the patched assembly will be stored in ```assemblies/modded```, add any assemblies in that folder as a reference on your project. If you need any other assemblies besides the patched ones they can be found in the ```Risk of Rain 2_Data/Managed``` folder.

Great, now you have a project to work on your mod from, the TridevLoader will scan any DLLs placed in the ```mods``` folder inside the game installation for a class with the ```Mod``` attribute. Once it finds one it will instantiate an instance of that class with the info provided from the ```Mod``` attribute. Use your constructor to prepare any variables you may need, instantiate any persistent game objects, as well as register with the event bus.

If your mod uses any references that aren't provided by the vanilla game then they must be placed in the ```mods/libraries``` folder in the game installation. The TridevLoader will load any assemblies placed in that directory without scanning for mod classes.

For more information on how to use the various APIs provided by the Tridev Modkit please visit the [wiki](https://github.com/TridentMC/TridevModKit/wiki)