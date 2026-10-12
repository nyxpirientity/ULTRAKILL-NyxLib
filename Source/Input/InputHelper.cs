using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BepInEx;
using HarmonyLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nyxpiri.ULTRAKILL.NyxLib;


public static class Input
{
    public delegate void CreateActionsEventHandler(ActionCreator creator);
    public static event CreateActionsEventHandler CreateActions;

    public static InputManager Manager { get; private set; } = null;

    public class ActionCreator
    {
        internal ActionCreator(PlayerInput input)
        {
            _input = input;
        }
        public InputAction CreateInputAction(string name, InputActionMap map, InputActionType type)
        {
            return CreateInputAction(name, map, type, null, null);
        }

        public InputAction CreateInputAction(string name, InputActionMap map, InputActionType type, string defaultBindings)
        {
            return CreateInputAction(name, map, type, defaultBindings, string.IsNullOrEmpty(defaultBindings) ? null : ControlSchemes.KeyboardAndMouse);
        }

        public InputAction CreateInputAction(string name, InputActionMap map, InputActionType type, string defaultBindings, InputControlScheme? controlScheme)
        {
            string expectedControlLayout = type switch
            {
                InputActionType.Value => "Value",
                InputActionType.Button => "Button",
                InputActionType.PassThrough => "PassThrough",
                _ => throw new NotImplementedException(),
            };

            InputAction action = map.AddAction(name, type, defaultBindings, null, null, controlScheme?.bindingGroup, expectedControlLayout);
            Input.RegisterAction(action);

            return action;
        }

        public InputAction CreateInputAction(string name, InputActionMap map, InputActionType type, KeyCode defaultBinding)
        {
            string expectedControlLayout = type switch
            {
                InputActionType.Value => "Value",
                InputActionType.Button => "Button",
                InputActionType.PassThrough => "PassThrough",
                _ => throw new NotImplementedException(),
            };

            string binding = $"<Keyboard>/{defaultBinding.ToString().ToLower()}";

            InputAction action = map.AddAction(name, InputActionType.Button, binding, null, null, _input.Actions.KeyboardMouseScheme.bindingGroup, expectedControlLayout);
            Input.RegisterAction(action);

            return action;
        }

        private PlayerInput _input = null;
    }

    private static void RegisterAction(InputAction action)
    {
        _actions.Add(action.name, action);
    }

    private static Dictionary<string, InputAction> _actions = [];
    private static Dictionary<string, List<JsonBinding>> _rememberedBindings = [];

    public static class ActionMaps
    {
        public static InputActionMap Fist { get; private set; }
        public static InputActionMap UI { get; private set; }
        public static InputActionMap Movement { get; private set; }
        public static InputActionMap Weapon { get; private set; }
        public static InputActionMap HUD { get; private set; }

        internal static void Initialize(PlayerInput input)
        {
            Fist = input.Actions.Fist.Get();
            UI = input.Actions.UI.Get();
            Movement = input.Actions.Movement.Get();
            Weapon = input.Actions.Weapon.Get();
            HUD = input.Actions.HUD.Get();
        }
    }

    public static class ControlSchemes
    {
        public static InputControlScheme KeyboardAndMouse { get; private set; }
        public static InputControlScheme Gamepad { get; private set; }

        internal static void Initialize(PlayerInput input)
        {
            KeyboardAndMouse = input.Actions.KeyboardMouseScheme;
            Gamepad = input.Actions.GamepadScheme;
        }
    }

    private static void Reinit(PlayerInput input)
    {
        Manager = InputManager.Instance;

        input.Disable();
        ActionMaps.Initialize(input);
        ControlSchemes.Initialize(input);
        CreateActions?.Invoke(new ActionCreator(input));
        input.Enable();
    }

    private static void LoadActions()
    {
        if (!File.Exists(BindingsFilePath))
        {
            return;
        }

        string jsonText = File.ReadAllText(BindingsFilePath);
        Dictionary<string, List<JsonBinding>> json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<JsonBinding>>>(jsonText);

        if (json == null)
        {
            Log.Warning($"Failed to read input bindings at {BindingsFilePath} due to first DeserializeObject failure");
            return;
        }

        foreach (var entry in json)
        {
            var bindings = entry.Value;

            if (bindings == null)
            {
                continue;
            }

            _rememberedBindings.Add(entry.Key, bindings);

            if (!_actions.ContainsKey(entry.Key))
            {
                continue;
            }

            var action = _actions[entry.Key];

            if (bindings == null)
            {
                continue;
            }

            action.WipeAction(ControlSchemes.KeyboardAndMouse.bindingGroup);

            foreach (var binding in bindings)
            {
                if (binding.isComposite)
                {
                    if (binding.parts.Count == 0)
                    {
                        continue;
                    }

                    var syntax = action.AddCompositeBinding(binding.path);

                    foreach (var part in binding.parts)
                    {
                        syntax.With(part.Key, part.Value, ControlSchemes.KeyboardAndMouse.bindingGroup);
                    }

                    action.ChangeBinding(syntax.bindingIndex).WithGroup(ControlSchemes.KeyboardAndMouse.bindingGroup);
                }
                else
                {
                    action.AddBinding(binding.path, null, null, ControlSchemes.KeyboardAndMouse.bindingGroup);
                }
            }
        }
    }

    private static void SaveActions()
    {
        using StreamWriter file = File.CreateText(BindingsFilePath);

        Dictionary<string, List<JsonBinding>> json = [];
        HashSet<string> saved = [];

        foreach (var action in _actions.Values)
        {
            Assert.IsNotNull(ControlSchemes.KeyboardAndMouse.bindingGroup);
            json.Add(action.name, JsonBinding.FromAction(action, ControlSchemes.KeyboardAndMouse.bindingGroup));
            saved.Add(action.name);
        }

        foreach (var bindings in _rememberedBindings)
        {
            string actionName = bindings.Key;

            if (saved.Contains(actionName))
            {
                continue;
            }

            json.Add(actionName, bindings.Value);
        }

        file.Write(JsonConvert.SerializeObject(json, Formatting.Indented));
    }

    public static string BindingsFilePath => Path.Combine(BepInEx.Paths.ConfigPath, "nyxlib_input_action_bindings.json");

    [HarmonyPatch(typeof(PlayerInput), "RebuildActions")]
    public static class PlayerInputAwakePatch
    {
        public static bool Prefix(PlayerInput __instance)
        {
            return true;
        }

        public static void Postfix(PlayerInput __instance)
        {
            Reinit(__instance);
        }
    }

    [HarmonyPatch(typeof(PlayerInput), "Enable")]
    public static class PlayerInputEnablePatch
    {
        public static bool Prefix(PlayerInput __instance)
        {
            return true;
        }

        public static void Postfix(PlayerInput __instance)
        {
            ControlSchemes.Initialize(__instance);
            ActionMaps.Initialize(__instance);
        }
    }

    [HarmonyPatch(typeof(InputManager), "Awake")]
    public static class InputManagerAwakePatch
    {
        public static bool Prefix(InputManager __instance)
        {
            return true;
        }

        public static void Postfix(InputManager __instance)
        {
            try
            {
                LoadActions();
            }
            catch (System.Exception e)
            {
                Log.Error($"LoadActions failed {e}");
            }
        }
    }

    [HarmonyPatch(typeof(InputManager), "SaveBindings")]
    public static class InputManagerSaveBindingsPatch
    {
        public static bool Prefix(InputManager __instance)
        {

            return true;
        }

        public static void Postfix(InputManager __instance)
        {
            try
            {
                SaveActions();
            }
            catch (System.Exception e)
            {
                Log.Error($"SaveActions failed {e}");
            }
        }
    }
}