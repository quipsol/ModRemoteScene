using System.Reflection;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using MegaCrit.Sts2.Core.Nodes.Ftue;

namespace ModRemoteScene.editor.GameRegistrationBootstrap;

internal static class GameScriptRegistration
{
    private static bool _registered;
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    internal static void Initialize() => EnsureRegistered();
    
    internal static void EnsureRegistered()
    {
        if (_registered) return; // safeguard 
        _registered = true;

        // If this runs in the live game a duplicate key error will be thrown and the mod will be marked as error.
        // This shouldn't happen because "editor/**" is not shipped to the live game dll/pck.
 
        // If it does happen, try to uncomment this line.
        // if (!Engine.IsEditorHint()) return;
        
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
            GD.PushWarning($"[Game Script Registration] Could not register script paths for '{assembly.GetName().Name}': {e.Message}");
        }
    }
}