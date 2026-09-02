## Example Project for running your Mod via the Godot Editor-Player

Running your mod in the Editor-Player can help visualize and understand the relation of your Scenes, as well as Nodes you added to base game scenes.

Unfortunately this is not a project that runs out of the box, as it requires direct access to the games files which I won't upload.

*This is based on [Lamali](https://github.com/lamali292)'s Symlink setup (see [Downfall](https://github.com/lamali292/Downfall) as an example)*

---

### Getting this Project to run

To run this example project you need to do the following:
1. Use [GDRE Tools](https://github.com/GDRETools/gdsdecomp/releases) to extract the games assets.
2. Rename `local.props.example` to `local.props` and fill out the paths.
3. Run `build\link-assets.ps1` to symlink the decompiles folders.
4. Manually copy over the compiler generated classes (`--**.cs`) into your root.
5. Copy over `default_bus_layout.tres` into your root.
6. Copy over all `spatlas` and `spskel` files from the decomps `.godot\imported\` folder into yours.

You should now be able to open the godot editor and run the game. 

For debugging with the editor, make sure `Debug->Keep Debug Server Open` is checked.


---

### Setting up your own Project
