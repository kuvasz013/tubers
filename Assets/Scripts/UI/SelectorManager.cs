using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class SelectorManager : MonoBehaviour
{
    private GameManager manager;
    private PlayerInputManager playerInputManager;
    private bool firstWASD = true;

    [SerializeField] private Transform emptySelectors;
    [SerializeField] private Transform playerSelectors;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    public void OnPlayerJoin(PlayerInput input)
    {
        if (input.currentControlScheme == "wasd" && firstWASD)
        {
            Destroy(input.gameObject);
            firstWASD = false;
            return;
        }

        AddNewPlayer(input);
    }

    public void AddNewPlayer(PlayerInput input)
    {
        if (input.currentControlScheme != ControlSchemes.Controller)
        {
            if (manager.PlayersConfigs.Any(pc => pc.controlScheme == input.currentControlScheme)) return;
        }

        var config = new PlayerConfig()
        {
            playerID = input.playerIndex,
            controlScheme = input.currentControlScheme,
            inputDevice = input.devices[0],
            tuberType = TuberType.Potato,
            playerInput = input,
            isDead = false,
        };

        manager.PlayersConfigs.Add(config);
        input.transform.SetParent(playerSelectors);

        var emptySelector = emptySelectors.GetChild(0);
        input.transform.position = emptySelector.transform.position;
        input.transform.localScale = emptySelector.transform.localScale;
        Destroy(emptySelector.gameObject);
    }
}
