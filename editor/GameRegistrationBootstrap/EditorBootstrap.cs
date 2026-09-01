using Godot;

namespace ModRemoteScene.editor.GameRegistrationBootstrap;

/// <summary>
/// <para>
/// Instantiating this from <c>plugin.gd</c> forces the mods DLL to load. This will
/// run <see cref="GameScriptRegistration"/>'s <c>[ModuleInitializer] Initialize</c> and registers the game
/// assembly's [ScriptPath] entries before any other editor plugin loads a game script.
/// </para>
/// <para>
/// The game calls Module.GetTypes() on the mod assembly at load time, which throws <c>ReflectionTypeLoadException</c>
/// if any type fails to resolve. -> Do not use anything from <c>GodotSharpEditor.dll</c>
/// </para>
/// </summary>
[Tool]
public partial class EditorBootstrap : RefCounted { }
// The editor folder is now excluded on build so "GodotSharpEditor.dll" should in theory not be an issue anymore.