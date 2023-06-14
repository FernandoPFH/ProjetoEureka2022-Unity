using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaDeInteraçãoDaLampada : MonoBehaviour
{
    [SerializeField] private Light componenteDeLuz;

    // Start is called before the first frame update
    async void Start()
    {
        componenteDeLuz.enabled = await ConectorDaAPI.conector.PegarStatusLampada();
    }

    public async void trocarStatusLampada()
    {
        if (await ConectorDaAPI.conector.SetarStatusLampada(!componenteDeLuz.enabled))
        {
            componenteDeLuz.enabled = !componenteDeLuz.enabled;
        }
    }
}
