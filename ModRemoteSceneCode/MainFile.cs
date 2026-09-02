using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace ModRemoteScene;


[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string MOD_ID = "ModRemoteScene";
    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(MOD_ID, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        
        // We do not want to call this in the editor or editor-player, because they are already loaded.
        
// TOOLS is defined when building with the Debug configuration (editor and editor player)
// https://docs.godotengine.org/en/4.4/tutorials/scripting/c_sharp/c_sharp_features.html
#if !TOOLS 
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(System.Reflection.Assembly.GetExecutingAssembly());
#endif
        
        Harmony harmony = new(MOD_ID);
        harmony.PatchAll();
    }
}