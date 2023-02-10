using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static Controls;

[RequireComponent(typeof(PlayerInput))]
public class SelectorController : MonoBehaviour, SelectorControls.ISelectorActions
{
    private PlayerInput input;
    private GameManager manager;
    private SelectorManager selectorManager;

    private readonly List<Key> WASDKeys = new() { Key.W, Key.A, Key.S, Key.D, Key.Space };
    private readonly List<Key> ArrowsKeys = new() { Key.LeftArrow, Key.RightArrow, Key.DownArrow, Key.UpArrow, Key.RightShift };

    static readonly object SpawnLock = new object();

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        manager = FindObjectOfType<GameManager>();
        selectorManager = FindObjectOfType<SelectorManager>();
    }

    private void CheckToAddKeyboardPlayer(InputControl control)
    {
        if (input == null || input.currentControlScheme == ControlSchemes.Controller) return;
        var kControl = (KeyControl) control;

        lock (SpawnLock)
        {
            if (input.currentControlScheme == ControlSchemes.Arrows)
            {
                if (ArrowsKeys.Contains(kControl.keyCode) && !manager.PlayersConfigs.Any(pc => pc.controlScheme == ControlSchemes.Arrows))
                {
                    selectorManager.AddNewPlayer(input);
                }
                else if (WASDKeys.Contains(kControl.keyCode) && !manager.PlayersConfigs.Any(pc => pc.controlScheme == ControlSchemes.WASD))
                {
                    var prefab = selectorManager.GetComponent<PlayerInputManager>().playerPrefab;
                    var input = PlayerInput.Instantiate(prefab, controlScheme: ControlSchemes.WASD, pairWithDevice: Keyboard.current);
                    selectorManager.AddNewPlayer(input);
                }
            }
            else if (input.currentControlScheme == ControlSchemes.WASD)
            {
                if (WASDKeys.Contains(kControl.keyCode) && !manager.PlayersConfigs.Any(pc => pc.controlScheme == ControlSchemes.WASD))
                {
                    selectorManager.AddNewPlayer(input);
                }
                else if (ArrowsKeys.Contains(kControl.keyCode) && !manager.PlayersConfigs.Any(pc => pc.controlScheme == ControlSchemes.Arrows))
                {
                    var prefab = selectorManager.GetComponent<PlayerInputManager>().playerPrefab;
                    var input = PlayerInput.Instantiate(prefab, controlScheme: ControlSchemes.Arrows, pairWithDevice: Keyboard.current);
                    selectorManager.AddNewPlayer(input);
                }
            }
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
            CheckToAddKeyboardPlayer(context.control);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
            CheckToAddKeyboardPlayer(context.control);
    }
}
