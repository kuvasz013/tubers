using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TuberSelector : MonoBehaviour
{
    public string controlScheme;
    [SerializeField] private Sprite potatoImage;
    [SerializeField] private Sprite carrotImage;
    [SerializeField] private Sprite beetImage;
    [SerializeField] private Sprite scallionImage;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI controlSchemeText;
    public Image tuberImage; 

    static private TuberType[] tuberTypes = new TuberType[] {TuberType.Potato, TuberType.Carrot, TuberType.Beet, TuberType.Scallion};
    static private int _i = 0;
    private TuberType selectedTuberType = tuberTypes[_i];

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
        selectedTuberType = tuberTypes[_i];
        tuberImage.sprite = GetSprite(selectedTuberType);
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

    private int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
