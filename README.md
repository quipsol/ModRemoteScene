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
4. Manually copy the compiler generated classes (`--**.cs`) into your root.
5. Copy `default_bus_layout.tres` into your root.
6. This example depends on BaseLib. Open your Godot installation and next to the exe add a new `mods` directory. Copy the BaseLib mod into it.
7. Publish once so Godot generates the .godot folder. (The first time will take quite long)
8. Copy all `spatlas` and `spskel` files from the decomps `.godot\imported\` folder into yours.



You should now be able to open the godot editor and run the game. 

For debugging with the editor, make sure in the godot editor `Debug->Keep Debug Server Open` is checked. <br/>
Add a new Configuration: <br/>
Set the Godot executable as the `Exe path` (not the games exe!) <br/>
Copy `--path "./" --remote-debug tcp://127.0.0.1:6007` into the `Program arguments`
Set the working directory as this mods root `..\ModRemoteScene`

Open the Godot editor and then press the Debug Icon next to the Configuration in Rider. You should now be able to get both, breakpoints; and the remote scene view inside the Godot Editor.

The Godot Editor will now also show the logs inside of it. <br/>
Please note that as long as the editor is open: Due to the above set Debug setting, publishing will fill the logs with the publish logs. This includes the games log output `godot.log` file.

---

### Setting up your own Project

Setting up your own project is very similar:

1. Use [GDRE Tools](https://github.com/GDRETools/gdsdecomp/releases) to extract the games assets.
2. Copy both the `build` and `editor` folders into your project. <br/>
    I recommend reading through all the files. (even if you just skim over them)
3. In your mods `csproj` import `mod.build.props` and `mod.build.targets`. At the bottom of it, copy the Sentry addon logic from the example mod. <br/>
   Besides the Sentry fuckery, the example project only has a single `PropertryGroup`and `ItemGroup` in it.
4. Replace your `project.godot` file with the one from the example or the decomp. <br/>
   If you copied the file from the example, change the following: <br/>
   1. Under `[application]` change `config/name` to your mod name.
   2. Under `[dotnet]` change the `project/assembly_name` to your mods. <br/>
   
   If you instead copied the file from the decomp, also change this:
   1. Add this at the top of `[autoload]`: `RuntimeBootstrap="*res://editor/GameRegistrationBootstrap/RuntimeBootstrap.cs"`
   2. Under `[dotnet]` remove `project/solution_directory`.
   3. Under `[editor_plugins]` edit `enabled` by adding `"res://editor/GameRegistrationBootstrap/plugin.cfg" as the **first** entry.
   4. Under `[Sentry]` remove `config/dsn` and set `config/disable_in_editor` to `true`

5. Create a `local.prop` file in your root and fill it in similar to the example file.
6. Follow from step 3 above.