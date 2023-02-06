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
        var manager = FindObjectOfType<GameManager>();
        var winner = manager == null ? TuberType.NONE : manager.winner;

        if (winner == TuberType.NONE)
        {
            winText.SetActive(false);
            loseText.SetActive(true);
            soupImg.sprite = soupSprites[(int)TuberType.NONE];
        }
        else
        {
            Debug.Log($"{winner} | {tuberNames[(int)winner]} | {winnerSprites[(int)winner].name}");
            loseText.SetActive(false);
            winText.SetActive(true);
            soupImg.sprite = soupSprites[(int)winner];

            winnerImg.gameObject.SetActive(true);
            winnerName.text = tuberNames[(int)winner];
            winnerImg.sprite = winnerSprites[(int)winner];
            winnerImg.SetNativeSize();
        }
    }

    public void OnBackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
