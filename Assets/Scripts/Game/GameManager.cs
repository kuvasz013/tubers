using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("DO NOT CHANGE THE ORDER HERE")]
    [SerializeField] private List<GameObject> playerPrefabs;

    [HideInInspector] public static IList<PlayerConfig> PlayersConfigs { get; private set; } = new List<PlayerConfig>();
    [HideInInspector] public static IList<PlayerConfig> Players { get; private set; } = new List<PlayerConfig>();
    public static TuberType Winner { get; set; }
    [HideInInspector] public bool levelLoaded;

    [Header("Set this to true when testing level in isolation")]
    [SerializeField] private bool spawnFallback;

    private void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        if (Application.isEditor && spawnFallback)
        {
            Debug.Log("Running in Unity Editor, trigger scene loaded event");
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level"))
        {
            if (levelLoaded) return;
            levelLoaded = true;
            SpawnAll();
        }
    }

    public void ResetManager()
    {
        Debug.Log($"Reset GameManager, winner stay: {Winner}");
        Debug.Log($"Configs deleted: {PlayersConfigs.Count}");
        PlayersConfigs = new List<PlayerConfig>();
        Players = new List<PlayerConfig>();
        levelLoaded = false;
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
        if (player.isDead) return;
        player.isDead = true;
        var pc = player.playerInput.gameObject.GetComponent<PlayerController>();
        pc.Disable();
        pc.emitters.ForEach(e => e.Play());

        if (Players.All(x => x.isDead))
        {
            StartCoroutine(KillCoroutine());
        }
    }

    private IEnumerator KillCoroutine()
    {
        yield return new WaitForSeconds(4f);
        Debug.Log("Everyone is dead, grandma wins!");
        Winner = TuberType.NONE;
        SceneManager.LoadScene("EndGame");
    }

    private void Spawn(PlayerConfig config, Vector3 position)
    {
        var player = PlayerInput.Instantiate(playerPrefabs[(int)config.tuberType], controlScheme: config.controlScheme, pairWithDevice: config.inputDevice);
        player.transform.SetPositionAndRotation(position, Quaternion.LookRotation(Vector3.forward));
        config.playerInput = player;
        config.playerInput.gameObject.GetComponent<PlayerController>().playerId = config.playerID;
        config.playerInput.gameObject.GetComponent<PlayerController>().PlayerConfig = config;
        Debug.Log($"Spawn Player{config.playerID}, TuberType: {config.tuberType}, ControlScheme: {config.controlScheme}");
        Players.Add(config);
    }
}