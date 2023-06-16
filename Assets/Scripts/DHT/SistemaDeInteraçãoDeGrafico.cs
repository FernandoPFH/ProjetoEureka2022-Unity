using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SistemaDeInteraçãoDeGrafico : MonoBehaviour
{
    [SerializeField] private float minumoValor;
    [SerializeField] private float maximoValor;
    
    [SerializeField] private List<float> valoresDeTreshold;
    [SerializeField] private List<Color> coresDeTreshold;
    
    [SerializeField] private float pontoMinimoDoGrafico;
    [SerializeField] private float pontoMaximoDoGrafico;
    
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private Image grafico;

    [SerializeField] private String sufixo;

    private float valorAtual = 33f;
    
    private int animacao;

    private float map(float x, float in_min, float in_max, float out_min, float out_max) {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public void atualizarValor(float novoValor)
    {
        Color cor = default;
        
        if (valoresDeTreshold.Count == 0)
            cor = Color.white;
        else
        {
            for (int i = 0; i < valoresDeTreshold.Count; i++)
                if (novoValor <= valoresDeTreshold[i])
                {
                    cor = coresDeTreshold[i];
                    break;
                }
            
            if (cor == default)
                cor = coresDeTreshold[valoresDeTreshold.Count];
        }
        
        display.color = cor;
        grafico.color = cor;

        animarParaValorNovo(novoValor);
    }

    private void animarParaValorNovo(float novoValor)
    {
        LeanTween.cancel(gameObject,animacao);

        animacao = LeanTween.value(gameObject, valorAtual, novoValor, 1).setEaseInOutBack()
            .setOnUpdate(value =>
            {
                valorAtual = value;
                display.text = $"{Mathf.RoundToInt(value).ToString()}{sufixo}";
                grafico.fillAmount = map(value, minumoValor, maximoValor, pontoMinimoDoGrafico, pontoMaximoDoGrafico);
            }).setOnComplete(
            _ => { valorAtual = novoValor;}).uniqueId;
    }
}
