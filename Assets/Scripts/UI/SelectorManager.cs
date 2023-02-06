using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class SelectorManager : MonoBehaviour
{
    private GameManager manager;
    private PlayerInputManager playerInputManager;
    private bool firstWASD = true;

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
    }
}
