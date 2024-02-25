using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class ConectorDaAPI : MonoBehaviour
{
    [SerializeField] public string IP;
    [SerializeField] public string PORT;
    [SerializeField] private int TOTAL_TENTATIVAS;

    [SerializeField] private SistemaDeInteraçãoDeGrafico graficoDeTemperatura;
    [SerializeField] private SistemaDeInteraçãoDeGrafico graficoDeHumidade;
    
    public static ConectarComAPI conector;

    async void Awake()
    {
        ConectarComAPI testarConexao = new ConectarComAPI(IP,PORT);
        
        int iteracao = 0;

        Debug.Log("Comecando Teste");
        
        while (iteracao < TOTAL_TENTATIVAS)
        {
            if (await testarConexao.TestarConexao())
                break;
            
            Debug.Log($"Tentiva {++iteracao} Falhou!");
        }
        

        if (iteracao >= TOTAL_TENTATIVAS)
        {
            Debug.Log("Iniciando ConectarComAPISimulador");
            conector = new ConectarComAPISimulador(IP,PORT);
        }
        else
        {
            Debug.Log("Iniciando ConectarComAPI");
            conector = new ConectarComAPI(IP,PORT);
        }
    }
    
    async void Start()
    {
        // FuncaoParaTestarAAPI();

        
        conector.SetarStatusLed(true, 0, 0, 0);
        StatusDHT statusDht = await conector.PegarInfosDht();

        graficoDeTemperatura.atualizarValor(statusDht.Temp);
        graficoDeHumidade.atualizarValor(statusDht.Hum);
    }

    void OnApplicationQuit()
    {
        conector.SetarStatusLed(true, 0, 0, 0);
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

    public void SetarConfiguracoes(string IP, string PORT)
    {
        this.IP = IP;
        this.PORT = PORT;
    }
}

public class ConectarComAPI
{
    public readonly bool isUp = true;
    
    private String PROTOCOLO = "http";
    private String IP;
    private String PORT;

    private readonly HttpClient httpClient;

    public ConectarComAPI(String ip, String port)
    {
        IP = ip;
        PORT = port;
        
        httpClient = new HttpClient();
    }

    public  async Task<bool> TestarConexao()
    {
        try
        {
            // Cria uma solicitação HTTP para a URL da API
            var request = (HttpWebRequest)WebRequest.Create($"{PROTOCOLO}://{IP}:{PORT}/led");
            request.Timeout = 5000; // Define um tempo limite de 5 segundos para a resposta da API

            // Envia a solicitação e obtém a resposta
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Debug.Log(response.StatusCode);
                // Verifica se o código de status da resposta é 200 (OK)
                return response.StatusCode == HttpStatusCode.OK;
            }
        }
        catch (Exception)
        {
            // Se ocorrer qualquer exceção durante a verificação, assume que a API está offline
            return false;
        }
    }

    private async Task<Status> RequesteGetDaAPI(string caminho)
    {
        return JsonUtility.FromJson<Status>(await httpClient.GetStringAsync($"{PROTOCOLO}://{IP}:{PORT}/{caminho}"));
    }

    private async Task<bool> RequestePostDaAPI(string caminho,Status dadosParaEnviar)
    {
        var conteudo = new StringContent(JsonUtility.ToJson(dadosParaEnviar), Encoding.UTF8, "application/json");
        
        return (await httpClient.PostAsync($"{PROTOCOLO}://{IP}:{PORT}/{caminho}",conteudo)).IsSuccessStatusCode;
    }

    public virtual async Task<bool> PegarStatusLampada()
    {
        Status statusLampada = await RequesteGetDaAPI("lamp");

        return statusLampada.status;
        
        // Retorna Bool Do Status Da Lampada
    }

    public virtual async Task<bool> SetarStatusLampada(bool status)
    {
        Status statusLampada = new Status();
        statusLampada.status = status;

        return await RequestePostDaAPI("lamp", statusLampada);
        
        // Retorna Bool Se Deu Certo
    }

    public virtual async Task<StatusLed> PegarInfosLed()
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

    public virtual async Task<bool> SetarStatusLed(bool status,int R,int G,int B)
    {
        Status statusLed = new Status();
        statusLed.status = status;
        statusLed.r = R;
        statusLed.g = G;
        statusLed.b = B;

        return await RequestePostDaAPI("led/soft", statusLed);
        
        // Retorna Bool Se Deu Certo
    }

    public virtual async Task<StatusDHT> PegarInfosDht()
    {
        Status statusRequeste = await RequesteGetDaAPI("dht");

        return new StatusDHT(statusRequeste.temperature,statusRequeste.humidity);
        
        // Retorna StatusLed
        //
        //  {
        //      float Temp;
        //      float Hum;
        //  }
    }
}

// Simulador De Conexão Com API Quando Não Puder Acessar API
public class ConectarComAPISimulador : ConectarComAPI
{
    public readonly bool isUp = false;
    
    private StatusLed statusLed = new(true,0,0,0);
    private StatusDHT statusDht = new(30,40);

    public ConectarComAPISimulador(String ip, String port) : base(ip, port)
    {
    }

    public override async Task<bool> PegarStatusLampada()
    {
        return true;
        
        // Retorna Bool Do Status Da Lampada
    }

    public override async Task<bool> SetarStatusLampada(bool status)
    {
        Status statusLampada = new Status();
        statusLampada.status = status;

        return true;
        
        // Retorna Bool Se Deu Certo
    }

    public override async Task<StatusLed> PegarInfosLed()
    {
        return new StatusLed(statusLed.status,statusLed.R,statusLed.G,statusLed.B);
        
        // Retorna StatusLed
        //
        //  {
        //      bool status;
        //      int R;
        //      int G;
        //      int B;
        //  }
    }

    public override async Task<bool> SetarStatusLed(bool status,int R,int G,int B)
    {
        statusLed.status = status;
        statusLed.R = R;
        statusLed.G = G;
        statusLed.B = B;

        return true;
        
        // Retorna Bool Se Deu Certo
    }

    public override async Task<StatusDHT> PegarInfosDht()
    {
        float valorMudarTemp = Random.Range(-2f, 2f);

        statusDht.Temp += valorMudarTemp;
        
        float valorMudarHum = Random.Range(-2f, 2f);

        if (statusDht.Hum + valorMudarHum >= 80f)
            statusDht.Hum -= valorMudarHum;
        else
            statusDht.Hum += valorMudarHum;
        
        return statusDht;
        
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
    public int r;
    public int g;
    public int b;
    public float temperature = 0;
    public float humidity = 0;
}

public class StatusLed
{
    public bool status;
    public int R;
    public int G;
    public int B;

    public StatusLed(bool status,int r,int g,int b)
    {
        this.status = status;
        R = r;
        G = g;
        B = b;
    }
}

public class StatusDHT
{
    public float Temp;
    public float Hum;

    public StatusDHT(float temp,float hum)
    {
        Temp = temp;
        Hum = hum;
    }
}


