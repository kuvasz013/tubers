using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    private TuberType winner;
    public TextMeshProUGUI title;
    public List<GameObject> veggiePics;

    private void Start()
    {
        winner = FindObjectOfType<GameManager>().winner;

        switch (winner)
        {
            case TuberType.Potato:
                title.text = "Potato";
                veggiePics[0].SetActive(true);
                break;
            case TuberType.Carrot:
                title.text = "Carrot";
                veggiePics[1].SetActive(true);
                break;
            case TuberType.Beet:
                title.text = "Beet";
                veggiePics[2].SetActive(true);
                break;
            case TuberType.Scallion:
                title.text = "Leek";
                veggiePics[3].SetActive(true);
                break;
            case TuberType.NONE:
            default:
                title.text = "Grandma";
                veggiePics[4].SetActive(true);
                break;
        }

        StartCoroutine(End());
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
