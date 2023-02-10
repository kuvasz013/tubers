using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SelectorManager : MonoBehaviour
{
    private GameManager manager;
    private bool firstWASD = true;

    [SerializeField] private Transform emptySelectors;
    [SerializeField] private Transform playerSelectors;
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private TextMeshProUGUI countdownTimer;
    [SerializeField] private int countdownTimeout = 5;
    [SerializeField] private GameObject loader;
    [SerializeField] private Spinner spinner;

    private Coroutine cdCoroutine;
    private AudioSource source;


    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        source = GetComponent<AudioSource>();
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

    public void StartCountdown()
    {
        countdownTimer.text = countdownTimeout.ToString();
        countdownPanel.SetActive(true);
        cdCoroutine = StartCoroutine(Countdown());
    }

    public void StopCountdown()
    {
        if (cdCoroutine != null) StopCoroutine(cdCoroutine);
        countdownPanel.SetActive(false);
        countdownTimer.text = countdownTimeout.ToString();
    }

    private IEnumerator Countdown()
    {
        for (var i = countdownTimeout; i >= 0; i--)
        {
            countdownTimer.text = i.ToString();
            source.Play();
            yield return new WaitForSeconds(1f);
        }

        loader.SetActive(true);
        spinner.StartSpinner();

        var players = FindObjectsOfType<SelectorController>();
        foreach (var player in players)
        {
            player.enabled= false;
        }

        SceneManager.LoadSceneAsync(2 + (int)Mathf.Floor(Random.value * 3));
    }
}
