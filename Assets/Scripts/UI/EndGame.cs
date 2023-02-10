using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public GameObject winText;
    public GameObject loseText;
    public TextMeshProUGUI winnerName;
    public Image soupImg;
    public Image winnerImg;

    [HeaderAttribute("DO NOT MODIFY THE ORDER")]
    public List<Sprite> soupSprites;
    public List<Sprite> winnerSprites;
    public List<string> tuberNames;

    private void Start()
    {
        if (GameManager.Winner == TuberType.NONE)
        {
            winText.SetActive(false);
            loseText.SetActive(true);
            soupImg.sprite = soupSprites[(int)TuberType.NONE];
        }
        else
        {
            Debug.Log($"{GameManager.Winner} | {tuberNames[(int)GameManager.Winner]} | {winnerSprites[(int)GameManager.Winner].name}");
            loseText.SetActive(false);
            winText.SetActive(true);
            soupImg.sprite = soupSprites[(int)GameManager.Winner];

            winnerImg.gameObject.SetActive(true);
            winnerName.text = tuberNames[(int)GameManager.Winner];
            winnerImg.sprite = winnerSprites[(int)GameManager.Winner];
            winnerImg.SetNativeSize();
        }
    }

    public void OnBackToMenu()
    {
        var manager = FindObjectOfType<GameManager>();
        manager.ResetManager();
        SceneManager.LoadScene(0);
    }
}
