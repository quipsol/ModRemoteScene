using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace ModRemoteScene.Examples;

/// <summary>
/// Example of logs showing in the Editor.
/// </summary>
[HarmonyPatch(typeof(NMainMenu), nameof(NMainMenu._Ready))]
public static class MainMenuLogMessage
{
    [HarmonyPrefix]
    public static void Log()
    {
        GD.Print("This message will appear in the logs!");
    }
}