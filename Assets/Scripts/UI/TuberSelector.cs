using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuberSelector : MonoBehaviour
{
    [SerializeField] private string controlScheme;
    [SerializeField] private string potatoImage;
    [SerializeField] private string carrotImage;
    [SerializeField] private string beetImage;
    [SerializeField] private string scallionImage;

    private TuberType[] tuberTypes = new TuberType[] {TuberType.Potato, TuberType.Carrot, TuberType.Beet, TuberType.Scallion};

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
