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
    public IList<PlayerConfig> PlayersConfigs { get; set; } = new List<PlayerConfig>();

    public IList<PlayerConfig> Players { get; set; } = new List<PlayerConfig>();

    [HideInInspector] public TuberType winner;
    private bool isLoaded = false;
    static private TuberType[] tuberTypes = new TuberType[] { TuberType.Potato, TuberType.Carrot, TuberType.Beet, TuberType.Scallion };



    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length > 1)
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
        if (scene.name.StartsWith("Level") && !isLoaded)
        {
            isLoaded = true;
            if (PlayersConfigs == null || PlayersConfigs.Count == 0)
            {
                Debug.Log("PlayerConfigs empty, faling back");
                PlayersConfigs = new List<PlayerConfig>()
                {
                    new PlayerConfig()
                    {
                        playerID = 0,
                        controlScheme = "arrows",
                        inputDevice = Keyboard.current,
                        tuberType = tuberTypes[(int) Mathf.Floor(UnityEngine.Random.value * 4)],
                    },
                    new PlayerConfig()
                    {
                        playerID = 1,
                        controlScheme = "wasd",
                        inputDevice = Keyboard.current,
                        tuberType = tuberTypes[(int) Mathf.Floor(UnityEngine.Random.value * 4)],
                    },
                };

                if (Gamepad.all.Count >= 1)
                {
                    PlayersConfigs.Add(new PlayerConfig()
                    {
                        playerID = 2,
                        controlScheme = "controller",
                        inputDevice = Gamepad.all[0],
                        tuberType = tuberTypes[(int)Mathf.Floor(UnityEngine.Random.value * 4)],
                    });
                }

                if (Gamepad.all.Count >= 2)
                {
                    PlayersConfigs.Add(new PlayerConfig()
                    {
                        playerID = 3,
                        controlScheme = "controller",
                        inputDevice = Gamepad.all[1],
                        tuberType = tuberTypes[(int)Mathf.Floor(UnityEngine.Random.value * 4)],
                    });
                }
            }
            SpawnAll();
        }
    }

    public void SetInputEnabled(bool enabled)
    {
        foreach (var p in Players) { p.playerInput.enabled = enabled; }
        Debug.Log($"Set all input to enabled: {enabled}");
    }

    private void SpawnAll()
    {
        if (PlayersConfigs.Count == 0) throw new Exception("There are no PlayerConfigs in the GameManager!");
        var positions = new List<Vector3>();

        for (var index = 0; index < PlayersConfigs.Count; index++)
        {
            positions.Add(new Vector3(0, 0, -10 + 20 / (PlayersConfigs.Count + 1) * (index + 1)));
        }

        var rnd = new System.Random();
        var randomizedPositions = positions.OrderBy(x => rnd.Next()).ToList();

        for (var index = 0; index < PlayersConfigs.Count; index++)
        {
            var player = PlayersConfigs[index];
            Spawn(player, randomizedPositions[index]);
        }
    }

    public void Kill(int playerID)
    {
        var player = Players.First(x => x.playerID == playerID);
        player.isDead = true;
        player.playerInput.gameObject.GetComponent<PlayerController>().Disable();

        if (Players.All(x => x.isDead))
        {
            winner = TuberType.NONE;
            SceneManager.LoadScene("EndGame");
        }
    }

    private void Spawn(PlayerConfig config, Vector3 position)
    {
        var player = PlayerInput.Instantiate(playerPrefabs[(int)config.tuberType], controlScheme: config.controlScheme, pairWithDevice: config.inputDevice);
        player.transform.SetPositionAndRotation(position, Quaternion.LookRotation(Vector3.forward));
        config.playerInput = player;
        config.playerInput.gameObject.GetComponent<PlayerController>().playerId = config.playerID;
        Debug.Log($"Spawn Player{config.playerID}, TuberType: {config.tuberType}, ControlScheme: {config.controlScheme}");
        Players.Add(config);
    }
}