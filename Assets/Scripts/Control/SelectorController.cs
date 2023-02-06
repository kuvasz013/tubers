using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[RequireComponent(typeof(PlayerInput))]
public class SelectorController : MonoBehaviour
{
    private PlayerInput input;
    private GameManager manager;
    private SelectorManager selectorManager;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        manager = FindObjectOfType<GameManager>();
        selectorManager= FindObjectOfType<SelectorManager>();
    }

    public void OnInput(InputAction.CallbackContext context)
    {
        if (input == null) return;

        if (input.currentControlScheme == "wasd" || input.currentControlScheme == "arrows")
        {
            Debug.Log(context.control.path);
            //CheckToAddKeyboard();
        }
    }

    private void CheckToAddKeyboard()
    {
        if (input.currentControlScheme == "wasd" && !manager.PlayersConfigs.Any(pc => pc.playerInput.currentControlScheme == "arrows"))
        {
            var prefab = selectorManager.GetComponent<PlayerInputManager>().playerPrefab;
            var input = PlayerInput.Instantiate(prefab, controlScheme: "arrows", pairWithDevice: Keyboard.current);
            selectorManager.AddNewPlayer(input);
        } else if(input.currentControlScheme == "arrows" && !manager.PlayersConfigs.Any(pc => pc.playerInput.currentControlScheme == "wasd")) {
            var prefab = selectorManager.GetComponent<PlayerInputManager>().playerPrefab;
            var input = PlayerInput.Instantiate(prefab, controlScheme: "wasd", pairWithDevice: Keyboard.current);
            selectorManager.AddNewPlayer(input);
        }
    }
}
