using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;

public class FalasRobo : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    [SerializeField] private List<AudioClip> falas;
    
    [SerializeField] private AudioSource caixaDeSom;
    [SerializeField] private AudioClip cassetteInicio;
    [SerializeField] private AudioClip backInBlack;
    [SerializeField] private AudioClip musicaGratis;
    [SerializeField] private AudioClip cassetteQuebrada;
    
    [SerializeField] private AudioSource somDeAmbiente;
    [SerializeField] private AudioClip explosao;
    
    [SerializeField] private List<GameObject> controles;

    [SerializeField] private Animator _animator;

    [SerializeField] private SistemaDeInteraçãoDoDHT sistemaDeInteraçãoDoDHT;

    public void darPlayNoAudio(string nomeDoAudio)
    {
        switch (nomeDoAudio)
        {
            case "Frase1":
                audio.clip = falas[0];
                break;
            case "Frase2":
                audio.clip = falas[1];
                break;
            case "Frase3":
                audio.clip = falas[2];
                break;
            case "Frase4":
                audio.clip = falas[3];
                break;
            case "Frase5":
                audio.clip = falas[4];
                break;
            case "Frase6":
                audio.clip = falas[5];
                break;
            case "Frase7":
                audio.clip = falas[6];
                break;
            case "Frase8":
                audio.clip = falas[7];
                break;
            case "Frase9":
                audio.clip = falas[8];
                break;
            case "Frase10":
                audio.clip = falas[9];
                break;
        }
        
        audio.Play();
    }

    public void darPlayMusica(string nomeDaMusica)
    {
        switch (nomeDaMusica)
        {
            case "CassetteInicio":
                caixaDeSom.clip = cassetteInicio;
                break;
            case "BackInBlack":
                caixaDeSom.clip = backInBlack;
                break;
            case "MusicaGratis":
                caixaDeSom.clip = musicaGratis;
                caixaDeSom.volume = 0.1f;
                break;
            case "CassetteQuebrada":
                caixaDeSom.clip = cassetteQuebrada;
                caixaDeSom.volume = 0.8f;
                break;
        }
        
        caixaDeSom.Play();
    }

    public void reativarControles()
    {
        foreach (GameObject controle in controles)
        {
            controle.SetActive(true);
        }
    }

    public async void ativarCatastrofeSeNecessario()
    {
        if (!ConectorDaAPI.conector.isUp)
            return;

        await Task.Delay(7000);
        
        sistemaDeInteraçãoDoDHT.ativarCatastrofe = true;
    }

    public void darPlayExplosao()
    {
        somDeAmbiente.clip = explosao;
        somDeAmbiente.loop = false;
        somDeAmbiente.volume = 1f;
        somDeAmbiente.Play();
    }

    public void fecharOsOlhos()
    {
        _animator.SetTrigger("FecharOsOlhos");
    }
}
