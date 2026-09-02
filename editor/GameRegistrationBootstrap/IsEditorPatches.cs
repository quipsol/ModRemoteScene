using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
// ReSharper disable UnusedType.Global
// ReSharper disable ArrangeTypeModifiers

namespace ModRemoteScene.editor.GameRegistrationBootstrap;

/// <summary>
/// The game makes a few deviations based on whether it runs in the Editor-Player or not. <br/>
/// These patches exist to work around those. They are likely incomplete and what needs patching depends on the mods you use.
/// </summary>
[HarmonyPatch]
static class IsEditorPatches
{
    /// <summary>
    /// The game skips ModelDb.Preload() in the editor-player as a startup optimization. <br/>
    /// BaseLib registers every scene-conversion from a postfix on that method. So it needs to run.
    /// </summary>
    [HarmonyPatch(typeof(OneTimeInitialization), nameof(OneTimeInitialization.ExecuteDeferred))]
    private static class EditorPreloadPatch
    {
        // Postfix is fine
        [HarmonyPostfix]
        private static void ForcePreloadInEditor()
        {
            if (OS.HasFeature("editor")) // Same check the game uses
                ModelDb.Preload();
        }
    }
}
