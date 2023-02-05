using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("DO NOT CHANGE THE ORDER HERE")]
    [SerializeField] private List<GameObject> playerPrefabs;

    [HideInInspector]
    public IList<PlayerConfig> Players { get; set; } = new List<PlayerConfig>() {
        new PlayerConfig()
        {
            playerID = 0,
            controlScheme = "wasd",
            inputDevice = Keyboard.current,
            tuberType = TuberType.Carrot,
        },
        new PlayerConfig()
        {
            playerID = 1,
            controlScheme = "arrows",
            inputDevice = Keyboard.current,
            tuberType = TuberType.Beet,
        },
    };


    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length > 0)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level")) SpawnAll();
    }

    public void SetInputEnabled(bool enabled)
    {
        foreach (var p in Players) { p.playerInput.enabled = enabled; }
        Debug.Log($"Set all input to enabled: {enabled}");
    }

    private void SpawnAll()
    {
        if (Players.Count == 0) throw new Exception("There are no PlayerConfigs in the GameManager!");
        var positions = new List<Vector3>();

        for (var index = 0; index < Players.Count; index++)
        {
            positions.Add(new Vector3(0, 0, -10 + 20 / (Players.Count + 1) * (index + 1)));
        }

        var rnd = new System.Random();
        var randomizedPositions = positions.OrderBy(x => rnd.Next()).ToList();

        for (var index = 0; index < Players.Count; index++)
        {
            var player = Players[index];
            Spawn(player, randomizedPositions[index]);
        }
    }

    private void Spawn(PlayerConfig config, Vector3 position)
    {
        var player = PlayerInput.Instantiate(playerPrefabs[(int)config.tuberType], controlScheme: config.controlScheme, pairWithDevice: config.inputDevice);
        player.transform.SetPositionAndRotation(position, Quaternion.LookRotation(Vector3.forward));
        config.playerInput = player;
        Debug.Log($"Spawn Player{config.playerID}, TuberType: {config.tuberType}, ControlScheme: {config.controlScheme}");
    }
}