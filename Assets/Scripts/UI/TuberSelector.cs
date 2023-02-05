using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TuberSelector : MonoBehaviour
{
    public string controlScheme;
    [SerializeField] private Sprite potatoImage;
    [SerializeField] private Sprite carrotImage;
    [SerializeField] private Sprite beetImage;
    [SerializeField] private Sprite scallionImage;
    [SerializeField] private Button leftButton;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI controlSchemeText;
    public Image tuberImage;
    public int playerId;

    static private TuberType[] tuberTypes = new TuberType[] {TuberType.Potato, TuberType.Carrot, TuberType.Beet, TuberType.Scallion};
    static private int _i = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRightPressed()
    {
        OnTuberChanged(true);
    }

    public void OnLeftPressed() 
    {
        OnTuberChanged(false);    
    }

    private void OnTuberChanged(bool increment)
    {
        _i = mod(increment ? (_i + 1) : (_i - 1), tuberTypes.Length);
        tuberImage.sprite = GetSprite(tuberTypes[_i]);
    }

    private Sprite GetSprite(TuberType type)
    {
        switch (type)
        {
            case TuberType.Beet: return beetImage;
            case TuberType.Scallion: return scallionImage;
            case TuberType.Potato: return potatoImage;
            case TuberType.Carrot: return carrotImage;
            default: return null;
        }
    }

    public PlayerConfig GetPlayerConfig(int controllersInUse)
    {
        return new PlayerConfig()
        {
            playerID = playerId,
            controlScheme = controlScheme,
            inputDevice = GetInputDevice(controlScheme, controllersInUse),
            tuberType = tuberTypes[_i],
        };
    }

    public void SetFocusOnButton()
    {
        var es = EventSystem.current;
        es.SetSelectedGameObject(leftButton.gameObject, new BaseEventData(es));
    }

    private int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    private InputDevice GetInputDevice(string controlScheme, int controllersInUse)
    {
        switch (controlScheme) 
        {
            case "wasd": return Keyboard.current;
            case "arrows": return Keyboard.current;
            case "controller": return Gamepad.all[controllersInUse];
            default: return null;
        }
    }
}
