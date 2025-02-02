using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using OVR;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SistemaDeInteraçãoDoDHT : MonoBehaviour
{
    [SerializeField] private SistemaDeInteraçãoDeGrafico graficoDeTemperatura;
    [SerializeField] private SistemaDeInteraçãoDeGrafico graficoDeHumidade;

    [SerializeField] private List<GameObject> paineisDePerigo;
    [SerializeField] private AudioSource somDeAlerta;

    [SerializeField] private List<UnityEngine.UI.Slider> slidersUILed;
    [SerializeField] private List<Light> luzes;

    [SerializeField] private EfeitosDoSkybox _efeitosDoSkybox;

    [SerializeField] private Animator _animator;

    [SerializeField] private bool debuging = true;

    [HideInInspector] public bool ativarCatastrofe = false;

    [HideInInspector] public bool piscarLed = true;

    [Range(1, 100)] public int HumTreshold = 80; 

    [Range(0, 100)] public int DimHum = 0; 
    
    private async void Start()
    {
        updateDosDisplay();

        if (debuging)
        {
            await Task.Delay(55000);
            graficoDeTemperatura.atualizarValor(95);
            graficoDeHumidade.atualizarValor(5);
            catastrofe();
        }
    }

    async void updateDosDisplay()
    {
        while (Application.isPlaying)
        {
            StatusDHT statusDht = await ConectorDaAPI.conector.PegarInfosDht();
        
            graficoDeTemperatura.atualizarValor(statusDht.Temp);
            graficoDeHumidade.atualizarValor(Mathf.Max(statusDht.Hum - DimHum,0));

            if (ativarCatastrofe || (ConectorDaAPI.conector.isUp && (Mathf.Max(statusDht.Hum - DimHum,0)) > HumTreshold))
            {
                graficoDeTemperatura.atualizarValor(95);
                graficoDeHumidade.atualizarValor(5);
                catastrofe();
                break;
            }
            
            await Task.Delay(1000);
        }
    }

    async void catastrofe()
    {
        setarLuzes();
        acionarEOcilarPaineisDePerigo();
        _efeitosDoSkybox.DeixarFundoLaranja();
        _animator.SetTrigger("Catastrofe");
    }

    async void setarLuzes()
    {
        foreach (UnityEngine.UI.Slider sliderUILed in slidersUILed)
        {
            sliderUILed.interactable = false;
        }

        foreach (Light luz in luzes)
        {
            luz.color = Color.red;
            luz.intensity = 4;
        }
    }

    async void piscarLuzes(bool valorLuz)
    {
        ConectorDaAPI.conector.SetarStatusLed(true, valorLuz?255:0, 0, 0);
        foreach (Light luz in luzes)
        {
            luz.gameObject.SetActive(valorLuz);
        }
    }

    async void acionarEOcilarPaineisDePerigo()
    {
        bool valorPaineis = true;

        while (piscarLed)
        {
            foreach (GameObject painelDePerigo in paineisDePerigo)
                painelDePerigo.SetActive(valorPaineis);

            piscarLuzes(valorPaineis);
            
            if (valorPaineis)
            {
                somDeAlerta.Play();
                await Task.Delay(1000);
            } else
                await Task.Delay(500);

            valorPaineis = !valorPaineis;
        }

        await ConectorDaAPI.conector.SetarStatusLed(false, 0, 0, 0);
    }
}
