using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EfeitosDoSkybox : MonoBehaviour
{
    [SerializeField] private Material skybox;
    [SerializeField] private float velocidadeDeRotacao = 1f;

    private float rotacaoInicial = 222f;
    
    // Start is called before the first frame update
    async void Start()
    {
        skybox.SetColor("_Tint",new Color(0.5f, 0.5f, 0.5f, 0.5f));
        skybox.SetFloat("_Exposure",1);
    }

    // Update is called once per frame
    void Update()
    {
        skybox.SetFloat("_Rotation",rotacaoInicial);

        rotacaoInicial += velocidadeDeRotacao;
    }

    public async void DeixarFundoLaranja()
    {
        trocarCor();
    }

    private async void trocarCor()
    {
        Color corInicial = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        Color corFinal = new Color(1f, 0.5f, 0f, 0.5f);
        float t = 0f;
        while (t <= 1f)
        {
            t += 0.01f;
            skybox.SetColor("_Tint",Color.Lerp(corInicial, corFinal, t));
            // skybox.SetFloat("_Exposure",Mathf.Lerp(1,2.5f,t));
            await Task.Delay(10);
        }
    }

    private void OnApplicationQuit()
    {
        skybox.SetColor("_Tint",new Color(0.5f, 0.5f, 0.5f, 0.5f));
        skybox.SetFloat("_Rotation",222f);
        skybox.SetFloat("_Exposure",1);
    }
}
