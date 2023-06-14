using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class ConectorDaAPI : MonoBehaviour
{
    [SerializeField] private string IP;
    [SerializeField] private string PORT;

    [SerializeField] private SistemaDeInteraçãoDeGrafico graficoDeTemperatura;
    [SerializeField] private SistemaDeInteraçãoDeGrafico graficoDeHumidade;
    
    public static ConectarComAPI conector;

    private void Awake()
    {
        conector = new ConectarComAPI(IP,PORT);
    }
    
    async void Start()
    {
        // FuncaoParaTestarAAPI();
        await ConectorDaAPI.conector.SetarStatusLed(true, 0,0, 0);
        
        StatusDHT statusDht = await ConectorDaAPI.conector.PegarInfosDht();
        
        graficoDeTemperatura.atualizarValor(statusDht.Temp);
        graficoDeHumidade.atualizarValor(statusDht.Hum);
    }

    public async void FuncaoParaTestarAAPI()
    {
        // Debug.Log("Teste Status Lampada");
        // Debug.Log($"Status Antes De Atualizar: {await conector.PegarStatusLampada()}");
        // await conector.SetarStatusLampada(true);
        // Debug.Log($"Status Depois De Atualizar: {await conector.PegarStatusLampada()}");
        // await conector.SetarStatusLampada(false);
        
        Debug.Log("Teste Status Led");
        Debug.Log($"Status Antes De Atualizar: {JsonUtility.ToJson(await conector.PegarInfosLed())}");
        await conector.SetarStatusLed(true,50,50,50);
        Debug.Log($"Status Depois De Atualizar: {JsonUtility.ToJson(await conector.PegarInfosLed())}");
        await conector.SetarStatusLed(false,0,0,0);
        
        Debug.Log("Teste Status DHT");
        Debug.Log($"Status: {JsonUtility.ToJson(await conector.PegarInfosDht())}");
    }
}

public class ConectarComAPI
{
    private String PROTOCOLO = "http";
    private String IP;
    private String PORT;

    private readonly HttpClient httpClient;

    public ConectarComAPI(String ip, String port)
    {
        this.IP = ip;
        this.PORT = port;
        
        this.httpClient = new HttpClient();
    }

    private async Task<Status> RequesteGetDaAPI(string caminho)
    {
        return JsonUtility.FromJson<Status>(await this.httpClient.GetStringAsync($"{PROTOCOLO}://{IP}:{PORT}/{caminho}"));
    }

    private async Task<bool> RequestePostDaAPI(string caminho,Status dadosParaEnviar)
    {
        var conteudo = new StringContent(JsonUtility.ToJson(dadosParaEnviar), Encoding.UTF8, "application/json");
        
        return (await this.httpClient.PostAsync($"{PROTOCOLO}://{IP}:{PORT}/{caminho}",conteudo)).IsSuccessStatusCode;
    }

    public async Task<bool> PegarStatusLampada()
    {
        Status statusLampada = await RequesteGetDaAPI("lamp");

        return statusLampada.status;
        
        // Retorna Bool Do Status Da Lampada
    }

    public async Task<bool> SetarStatusLampada(bool status)
    {
        Status statusLampada = new Status();
        statusLampada.status = status;

        return await RequestePostDaAPI("lamp", statusLampada);
        
        // Retorna Bool Se Deu Certo
    }

    public async Task<StatusLed> PegarInfosLed()
    {
        Status statusRequeste = await RequesteGetDaAPI("led");

        return new StatusLed(statusRequeste.status,statusRequeste.r,statusRequeste.g,statusRequeste.b);
        
        // Retorna StatusLed
        //
        //  {
        //      bool status;
        //      int R;
        //      int G;
        //      int B;
        //  }
    }

    public async Task<bool> SetarStatusLed(bool status,int R,int G,int B)
    {
        Status statusLed = new Status();
        statusLed.status = status;
        statusLed.r = R;
        statusLed.g = G;
        statusLed.b = B;

        return await RequestePostDaAPI("led/soft", statusLed);
        
        // Retorna Bool Se Deu Certo
    }

    public async Task<StatusDHT> PegarInfosDht()
    {
        Status statusRequeste = await RequesteGetDaAPI("DHT");

        return new StatusDHT(statusRequeste.temperature,statusRequeste.humidity);
        
        // Retorna StatusLed
        //
        //  {
        //      float Temp;
        //      float Hum;
        //  }
    }
}

class Status
{
    public bool status;
    public int r = 0;
    public int g = 0;
    public int b = 0;
    public float temperature = 0;
    public float humidity = 0;
}

public class StatusLed
{
    public bool status;
    public int R = 0;
    public int G = 0;
    public int B = 0;

    public StatusLed(bool status,int r,int g,int b)
    {
        this.status = status;
        this.R = r;
        this.G = g;
        this.B = b;
    }
}

public class StatusDHT
{
    public float Temp = 0;
    public float Hum = 0;

    public StatusDHT(float temp,float hum)
    {
        this.Temp = temp;
        this.Hum = hum;
    }
}


