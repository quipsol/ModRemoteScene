using System.Reflection;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using MegaCrit.Sts2.Core.Nodes.Ftue;

namespace ModRemoteScene.editor.GameRegistrationBootstrap;


/// <summary>
/// Registers the game assembly if run in the Editor or Editor-player
/// </summary>
internal static class GameScriptRegistration
{
    private static bool _registered;
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    internal static void Initialize() => EnsureRegistered();
    
    internal static void EnsureRegistered()
    {
        if (_registered) return;
        _registered = true;

        // If this runs in the live game a duplicate key error will be thrown and the mod may be marked as error.
        // This shouldn't happen in a release build because "editor/**" is not included.
        // However, it may still happen due to attaching a debugger. So we check for that via !TOOLS just in case.
        // Note: "Engine.IsEditorHint() is false in editor-player!
        
// TOOLS is defined when building with the Debug configuration (editor and editor player)
// https://docs.godotengine.org/en/4.4/tutorials/scripting/c_sharp/c_sharp_features.html
#if !TOOLS 
        return;
#endif
        
        GD.Print("Looking up the Scripts in the game assembly");
        Register(typeof(NFtue).Assembly);
    }

    private static void Register(Assembly assembly)
    {
        try
        {
            ScriptManagerBridge.LookupScriptsInAssembly(assembly);
        }
        catch (Exception e)
        {
            GD.PushWarning($"[Game Script Registration] Could not register script paths for '{assembly.GetName().Name}':\n{e.Message}");
        }
    }
}