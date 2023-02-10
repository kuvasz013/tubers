using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using static Controls;

[RequireComponent(typeof(PlayerInput), typeof(AudioSource))]
public class SelectorController : MonoBehaviour, SelectorControls.ISelectorActions
{
    [Header("GameObject references")]
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private TextMeshProUGUI playerTextObject;
    [SerializeField] private TextMeshProUGUI nameTextObject;
    [SerializeField] private Image rootImgObject;
    [SerializeField] private Image controlImgObject;
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject checkmark;

    [Header("SFX")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip selectSound;

    [Header("DO NOT CHANGE THE ORDER")]
    [SerializeField] private List<Sprite> tuberSprites;
    [SerializeField] private List<string> tuberNames;
    [SerializeField] private List<Sprite> controlSprites;

    private PlayerInput input;
    private GameManager manager;
    private SelectorManager selectorManager;
    private AudioSource source;

    [HideInInspector] public bool Ready { get; private set; }  = false;
    private TuberType tuber = TuberType.Potato;

    private readonly List<Key> WASDKeys = new() { Key.W, Key.A, Key.S, Key.D, Key.Space };
    private readonly List<Key> ArrowsKeys = new() { Key.LeftArrow, Key.RightArrow, Key.DownArrow, Key.UpArrow, Key.RightShift };

    static readonly object SpawnLock = new object();

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        selectorManager = FindObjectOfType<SelectorManager>();
        input = GetComponent<PlayerInput>();
        source = GetComponent<AudioSource>();
        UpdateUI();
    }

    private void CheckToAddKeyboardPlayer(InputControl control)
    {
        if (input == null || input.currentControlScheme == ControlSchemes.Controller) return;
        var kControl = (KeyControl)control;

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

    private void UpdateUI()
    {
        playerTextObject.text = $"Player{(input.playerIndex == 0 ? 1 : input.playerIndex + 1)}";
        nameTextObject.text = tuberNames[(int)tuber];

        rootImgObject.sprite = tuberSprites[(int)tuber];
        rootImgObject.SetNativeSize();

        controlImgObject.sprite = input.currentControlScheme switch
        {
            "wasd" => controlSprites[0],
            "arrows" => controlSprites[1],
            "controller" => controlSprites[2],
            _ => controlSprites[0],
        };
        controlImgObject.SetNativeSize();

        if (tuber == TuberType.Potato)
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(true);
        }
        else if (tuber == TuberType.Scallion)
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(false);
        }
        else
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
        }

        if (Ready)
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        CheckToAddKeyboardPlayer(context.control);
        if (input == null || !context.started) return;
        if (input.currentControlScheme == ControlSchemes.Arrows && !ArrowsKeys.Contains(((KeyControl)context.control).keyCode)) return;
        if (input.currentControlScheme == ControlSchemes.WASD && !WASDKeys.Contains(((KeyControl)context.control).keyCode)) return;
        OnReady();
    }

    // This is public to be accessible to UI Buttons
    public void OnReady()
    {
        if (!Ready)
        {
            Debug.Log($"Player{input.playerIndex + 1} selected {tuberNames[(int)tuber]}");
            buttonText.text = "Back";
            Ready = true;
            manager.PlayersConfigs.First(pc => pc.playerID == input.playerIndex).tuberType = tuber;
            checkmark.SetActive(true);
        }
        else
        {
            Debug.Log($"Player{input.playerIndex + 1} un-readied");
            buttonText.text = "Ready!";
            Ready = false;
            checkmark.SetActive(false);
        }

        source.clip = selectSound;
        source.Play();
        UpdateUI();

        var controllers = FindObjectsOfType<SelectorController>();
        if (controllers.All(c => c.Ready))
        {
            selectorManager.StartCountdown();
        }
        else
        {
            selectorManager.StopCountdown();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        CheckToAddKeyboardPlayer(context.control);

        if (input == null || !context.started || Ready) return;
        if (input.currentControlScheme == ControlSchemes.Arrows && !ArrowsKeys.Contains(((KeyControl)context.control).keyCode)) return;
        if (input.currentControlScheme == ControlSchemes.WASD && !WASDKeys.Contains(((KeyControl)context.control).keyCode)) return;

        var value = context.ReadValue<Vector2>();
        if (value == Vector2.zero) return;

        if (Mathf.Abs(value.x) > Mathf.Abs(value.y))
        {
            if (value.x > 0) NextTuber();
            else if (value.x < 0) PreviousTuber();
        }
    }

    //These are here to be accessible from UI Buttons
    public void NextTuber()
    {
        if ((int)tuber >= 3) return;
        tuber++;
        UpdateUI();
        source.clip = clickSound;
        source.Play();
    }

    public void PreviousTuber()
    {
        if (tuber <= 0) return;
        tuber--;
        UpdateUI();
        source.clip = clickSound;
        source.Play();
    }
}
