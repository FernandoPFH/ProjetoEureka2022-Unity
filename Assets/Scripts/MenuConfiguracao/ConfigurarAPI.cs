using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ConfigurarAPI : MonoBehaviour
{
    [SerializeField] private string PROTOCOL = "http";
    [SerializeField] private ConectorDaAPI conectorDaAPI;
    
    [SerializeField] private GameObject imagemCarregandoTesteAPI;
    [SerializeField] private GameObject imagemErroTesteAPI;
    [SerializeField] private GameObject imagemSucessoTesteAPI;

    [SerializeField] private TextMeshProUGUI texto_IP_PORT;
    
    private string IP;
    private string PORT;

    private string pathArquivoConfiguracao = "/configuracoes_api.json";
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        
        CarregarInformacoes();

        atualizarTexto();
    }

    public async void testarAPI()
    {
        imagemCarregandoTesteAPI.SetActive(true);
        
        ConectarComAPI testarConexao = new ConectarComAPI(IP, PORT);

        if (await testarConexao.TestarConexao())
        {
            imagemSucessoTesteAPI.SetActive(true);
        }
        else
        {
            imagemErroTesteAPI.SetActive(true);
        }

        await Task.Delay(3000);
        
        imagemCarregandoTesteAPI.SetActive(false);
        imagemSucessoTesteAPI.SetActive(false);
        imagemErroTesteAPI.SetActive(false);
    }

    private void atualizarTexto()
    {
        string[] ip_parts = IP.Split('.');

        string texto = "";

        foreach (var ip_part in ip_parts)
        {
            texto += $"{int.Parse(ip_part):000}.";
        }

        texto_IP_PORT.text = $"{texto.Remove(texto.Length - 1, 1)}:{int.Parse(PORT):0000}";
    }

    public void AcrescentarValor(int index = 0)
    {
        acrescentarValor(index, 1);
    }

    public void SubtrairValor(int index = 0)
    {
        acrescentarValor(index, -1);
    }
    
    private void acrescentarValor(int index=0, int valor=1)
    {
        string[] ip_parts = IP.Split('.');

        if (index == ip_parts.Length)
        {
            int port_int = int.Parse(PORT);
            if (port_int + valor > 9999 || port_int + valor < 1)
                return;
            
            PORT = (port_int + 1).ToString();

            atualizarTexto();
        } else if (index < ip_parts.Length)
        {
            int ip_part_int = int.Parse(ip_parts[index]);

            if (ip_part_int + valor > 255 || ip_part_int + valor < 1)
                return;
            
            ip_parts[index] = (ip_part_int + valor).ToString();

            string texto = "";
            
            foreach (var ip_part in ip_parts)
            {
                texto += $"{int.Parse(ip_part):000}.";
            }

            IP = $"{texto.Remove(texto.Length - 1, 1)}";
            
            atualizarTexto();
        }
    }

    void CarregarInformacoes()
    {
        string path = Application.persistentDataPath + pathArquivoConfiguracao;
        if (File.Exists(path))
        {
            string fileContents = File.ReadAllText(path);

            ConfiguracoesAPI configuracoesAPI = JsonUtility.FromJson<ConfiguracoesAPI>(fileContents);
            
            IP = configuracoesAPI.IP;
            PORT = configuracoesAPI.PORT;

            conectorDaAPI.IP = IP;
            conectorDaAPI.PORT = PORT;
        }
        else
        {
            IP = conectorDaAPI.IP;
            PORT = conectorDaAPI.PORT;
        }

        conectorDaAPI.IP = IP;
        conectorDaAPI.PORT = PORT;
    }

    public void SalvarInformacoes()
    {
        string configuracoesAPI = JsonUtility.ToJson(new ConfiguracoesAPI(IP, PORT));
        File.WriteAllText(Application.persistentDataPath + pathArquivoConfiguracao, configuracoesAPI);
    }
}

[System.Serializable]
public class ConfiguracoesAPI
{
    public string IP;
    public string PORT;

    public ConfiguracoesAPI(string IP, string PORT)
    {
        this.IP = IP;
        this.PORT = PORT;
    }
}