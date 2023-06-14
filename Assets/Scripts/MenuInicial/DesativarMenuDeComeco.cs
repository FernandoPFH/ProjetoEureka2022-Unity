using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DesativarMenuDeComeco : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private List<GameObject> controles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void desativarMenu()
    {
        _animator.enabled = true;
        gameObject.SetActive(false);
        foreach (GameObject controle in controles)
        {
            controle.SetActive(false);
        }
    }

    public void recomecarExperiencia()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
