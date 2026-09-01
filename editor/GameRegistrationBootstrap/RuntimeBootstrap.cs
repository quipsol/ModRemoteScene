using Godot;

namespace ModRemoteScene.editor.GameRegistrationBootstrap;

/// <summary>
/// Must be the very first entry under <c>project.godot</c>'s [autoload] <br/>
/// 
/// Deliberately not [Tool]: In the editor Godot creates a placeholder instead of a managed instance and this never runs. <br/>
/// 
/// <c>Plugin.gd</c> with <c>EditorBootstrap</c> covers the editor.
/// </summary>
public partial class RuntimeBootstrap : Node
{
    public RuntimeBootstrap() => GameScriptRegistration.EnsureRegistered();
}