using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour
{

    [SerializeField] private GameObject devicesParent;
    [SerializeField] private Button schemeButtonTemplate;
    [SerializeField] private GameObject tuberSelectorsParent;
    [SerializeField] private GameObject tuberSelectorPrefab;
    [SerializeField] private GameManager gameManager;

    private List<string> _controlSchemes;
    private List<string> _selectedControlSchemes;
    private List<TuberSelector> _tuberSelectors;

    // Start is called before the first frame update
    void Start()
    {
        _tuberSelectors = new List<TuberSelector>();
        _selectedControlSchemes = new List<string>();
        _controlSchemes = GetAvailableControlSchemes();
        RenderAvailableControlSchemes(_controlSchemes);
        Destroy(devicesParent.transform.GetChild(0).gameObject); // Remove template object
        FocusOnFirstControlScheme();
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
            var listItem = Instantiate(schemeButtonTemplate, devicesParent.transform);
            listItem.GetComponentInChildren<TextMeshProUGUI>().text = scheme;
            listItem.GetComponent<Button>().onClick.AddListener(() => OnControlSchemeSelected(scheme, listItem.gameObject));
        }
    }

    private void FocusOnFirstControlScheme()
    {
        var es = EventSystem.current;
        es.SetSelectedGameObject(devicesParent.transform.GetChild(1).gameObject, new BaseEventData(es));
    }

    public void OnControlSchemeSelected(string controlScheme, GameObject go)
    {
        _selectedControlSchemes.Add(controlScheme);
        Destroy(go);
        var shouldFocusOnTuberSelectorButton = _selectedControlSchemes.Count == _controlSchemes.Count;
        if (!shouldFocusOnTuberSelectorButton) FocusOnFirstControlScheme();
        DisplayTuberSelector(_selectedControlSchemes.Count - 1, controlScheme, shouldFocusOnTuberSelectorButton);
    }

    private void DisplayTuberSelector(int playerId, string controlScheme, bool shouldFocusOnTuberSelectorButton)
    {
        var tuberSelector = Instantiate(tuberSelectorPrefab, tuberSelectorsParent.transform);
        tuberSelector.GetComponent<TuberSelector>().playerName.text = "Player " + (playerId + 1);
        tuberSelector.GetComponent<TuberSelector>().controlSchemeText.text = controlScheme;
        tuberSelector.GetComponent<TuberSelector>().controlScheme = controlScheme;
        tuberSelector.GetComponent<TuberSelector>().playerId = playerId;
        if (shouldFocusOnTuberSelectorButton)
        {
            tuberSelectorsParent.transform.GetChild(0).GetComponent<TuberSelector>().SetFocusOnButton();
        }
        
        _tuberSelectors.Add(tuberSelector.GetComponent<TuberSelector>());
    }

    public void OnStartGame()
    {
        if (_selectedControlSchemes.Count <= 0) return;
        var playerConfigs = new List<PlayerConfig>();
        var controllersInUse = 0;
        foreach (var tuberSelector in _tuberSelectors)
        {
            playerConfigs.Add(tuberSelector.GetPlayerConfig(controllersInUse));
            if (tuberSelector.controlScheme == "controller") ++controllersInUse;
        }
        gameManager.PlayersConfigs= playerConfigs;
    }
}
