using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ConfigurarHumTreshold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texto_hum;
    [SerializeField] private SistemaDeInteraçãoDoDHT sistemaDeInteraçãoDoDht;
    
    private int Hum;

    private string pathArquivoConfiguracao = "/configuracoes_hum.json";
    
    // Start is called before the first frame update
    void Start()
    {
        CarregarInformacoes();
    }

    private void atualizarTexto()
    {
        texto_hum.text = $"{Hum}%";
    }

    public void AcrescentarValor()
    {
        acrescentarValor(1);
    }

    public void SubtrairValor()
    {
        acrescentarValor(-1);
    }
    
    private void acrescentarValor(int valor=1)
    {
        if (Hum + valor > 100 || Hum + valor < 1)
            return;

        Hum += valor;
        
        atualizarTexto();
    }

    public void CarregarInformacoes()
    {
        string path = Application.persistentDataPath + pathArquivoConfiguracao;
        if (File.Exists(path))
        {
            string fileContents = File.ReadAllText(path);

            ConfiguracoesHum configuracoesHum = JsonUtility.FromJson<ConfiguracoesHum>(fileContents);
            
            Hum = configuracoesHum.Hum;
        }
        else
        {
            Hum = sistemaDeInteraçãoDoDht.HumTreshold;
        }

        sistemaDeInteraçãoDoDht.HumTreshold = Hum;

        atualizarTexto();
    }

    public void SalvarInformacoes()
    {
        string configuracoesHum = JsonUtility.ToJson(new ConfiguracoesHum(Hum));
        File.WriteAllText(Application.persistentDataPath + pathArquivoConfiguracao, configuracoesHum);
    }
}

[System.Serializable]
public class ConfiguracoesHum
{
    public int Hum;

    public ConfiguracoesHum(int Hum)
    {
        this.Hum = Hum;
    }
}