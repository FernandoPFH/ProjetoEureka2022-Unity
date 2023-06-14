using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FazerAparecerMapa : MonoBehaviour
{
    [SerializeField] private GameObject mapa;
    [SerializeField] private GameObject menuFinal;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fazerAparecerMapa()
    {
        mapa.SetActive(true);
    }

    public void fazerDesaparecerMapa()
    {
        mapa.SetActive(false);
    }

    public void fazerAparecerMenu()
    {
        menuFinal.SetActive(true);
    }
}
