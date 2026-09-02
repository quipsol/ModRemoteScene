using Godot;

namespace ModRemoteScene.editor.GameRegistrationBootstrap;

/// <summary>
/// Must be the very first entry under <c>project.godot</c>'s [autoload]. <br/>
/// <c>Plugin.gd</c> with <c>EditorBootstrap</c> covers the editor.
/// </summary>
public partial class RuntimeBootstrap : Node
{
    public RuntimeBootstrap() => GameScriptRegistration.EnsureRegistered();
}