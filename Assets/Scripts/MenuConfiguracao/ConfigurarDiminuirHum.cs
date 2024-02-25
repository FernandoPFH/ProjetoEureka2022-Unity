using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class ConfigurarDiminuirHum : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texto_dimhum;
    [SerializeField] private SistemaDeInteraçãoDoDHT sistemaDeInteraçãoDoDht;
    
    private int DimHum;

    private string pathArquivoConfiguracao = "/configuracoes_dimhum.json";

    // Start is called before the first frame update
    void Start()
    {
        CarregarInformacoes();
    }

    private void atualizarTexto()
    {
        texto_dimhum.text = $"{DimHum}%";
    }

    public void AcrescentarValor()
    {
        acrescentarValor(5);
    }

    public void SubtrairValor()
    {
        acrescentarValor(-5);
    }
    
    private void acrescentarValor(int valor=1)
    {
        if (DimHum + valor > 100 || DimHum + valor < 0)
            return;

        DimHum += valor;
        
        atualizarTexto();
    }

    public void CarregarInformacoes()
    {
        string path = Application.persistentDataPath + pathArquivoConfiguracao;
        if (File.Exists(path))
        {
            string fileContents = File.ReadAllText(path);

            ConfiguracoesDimHum configuracoesHum = JsonUtility.FromJson<ConfiguracoesDimHum>(fileContents);
            
            DimHum = configuracoesHum.DimHum;
        }
        else
        {
            DimHum = sistemaDeInteraçãoDoDht.DimHum;
        }

        sistemaDeInteraçãoDoDht.DimHum = DimHum;

        atualizarTexto();
    }

    public void SalvarInformacoes()
    {
        string configuracoesHum = JsonUtility.ToJson(new ConfiguracoesDimHum(DimHum));
        File.WriteAllText(Application.persistentDataPath + pathArquivoConfiguracao, configuracoesHum);
    }
}

[System.Serializable]
public class ConfiguracoesDimHum
{
    public int DimHum;

    public ConfiguracoesDimHum(int DimHum)
    {
        this.DimHum = DimHum;
    }
}