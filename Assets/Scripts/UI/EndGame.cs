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
        var manager = FindObjectOfType<GameManager>();
        if (manager != null) {
            winner = TuberType.NONE;
        }
        
        switch (winner)
        {
            case TuberType.Potato:
                title.text = "You’ve made it, Pot Ato! You are a free veggie now!";
                veggiePics[0].SetActive(true);
                break;
            case TuberType.Carrot:
                title.text = "You’ve made it, C’Arrot! You are a free veggie now!";
                veggiePics[1].SetActive(true);
                break;
            case TuberType.Beet:
                title.text = "You’ve made it, B.T. Root! You are a free veggie now!";
                veggiePics[2].SetActive(true);
                break;
            case TuberType.Scallion:
                title.text = "You’ve made it, L33k! You are a free veggie now!";
                veggiePics[3].SetActive(true);
                break;
            case TuberType.NONE:
            default:
                title.text = ":( You didn’t make it. But at least the borscht is perfect, and Grandma is happy.";
                veggiePics[4].SetActive(true);
                break;
        }

        StartCoroutine(End());
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(5f);
        //SceneManager.LoadScene(0);
    }
}
