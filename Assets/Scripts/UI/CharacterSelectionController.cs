using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour
{

    [SerializeField] private GameObject devicesParent;
    [SerializeField] private Button listItemTemplate;

    // Start is called before the first frame update
    void Start()
    {
        var controlSchemes = GetAvailableControlSchemes();
        RenderAvailableControlSchemes(controlSchemes);
        Destroy(devicesParent.transform.GetChild(0).gameObject); // Remove template object
        SelectFirstControlScheme();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<string> GetAvailableControlSchemes()
    {
        List<string> controlSchemes = new List<string>();
        foreach (InputDevice device in InputSystem.devices)
        {
            var isKeyboard = device.displayName.ToLower().Contains("keyboard");
            var isController = device.displayName.ToLower().Contains("controller");

            if (isKeyboard)
            {
                controlSchemes.Add("wasd");
                controlSchemes.Add("arrows");
            }
            else if (isController)
            {
                controlSchemes.Add("controller");
            }
        }
        return controlSchemes;
    }

    private void RenderAvailableControlSchemes(List<string> controlSchemes)
    {
        foreach (string scheme in controlSchemes) 
        {
            var listItem = Instantiate(listItemTemplate, devicesParent.transform);
            listItem.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = scheme;
        }
    }

    private void SelectFirstControlScheme()
    {
        var es = EventSystem.current;
        es.SetSelectedGameObject(devicesParent.transform.GetChild(1).gameObject, new BaseEventData(es));
    }
}
