using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib;
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

        public InputAction CreateInputAction(string name, InputActionMap map, InputActionType type, string defaultBindings)
        {
            string expectedControlLayout = type switch
            {
                InputActionType.Value => "Value",
                InputActionType.Button => "Button",
                InputActionType.PassThrough => "PassThrough",
                _ => throw new NotImplementedException(),
            };

            InputAction action = map.AddAction(name, type, defaultBindings, null, null, _input.Actions.KeyboardMouseScheme.bindingGroup, expectedControlLayout);

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

            return action;
        }

        private PlayerInput _input = null;
    }

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

    private static void Reinit(PlayerInput input)
    {
        Manager = InputManager.Instance;

        input.Disable();
        ActionMaps.Initialize(input);
        CreateActions?.Invoke(new ActionCreator(input));
        input.Enable();
    }

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
}