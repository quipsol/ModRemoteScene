using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace ModRemoteScene;

//You're recommended but not required to keep all your code in this package and all your assets in the ModRemoteScene folder.
[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string MOD_ID = "ModRemoteScene"; //At the moment, this is used only for the Logger and harmony names.

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(MOD_ID, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        // TOOLS is defined when building with the Debug configuration (editor and editor player)
        // https://docs.godotengine.org/en/4.4/tutorials/scripting/c_sharp/c_sharp_features.html
#if !TOOLS
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
#endif
        
        Harmony harmony = new(MOD_ID);
        harmony.PatchAll();
    }
}