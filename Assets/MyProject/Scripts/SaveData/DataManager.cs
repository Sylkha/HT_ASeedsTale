using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Yarn;
using Yarn.Unity;

[RequireComponent(typeof(DataSave))]
public class DataManager : MonoBehaviour
{
    DataSave dataSave;

    [SerializeField] GameObject Player;
    [SerializeField] InMemoryVariableStorage memoryVariable;

    [SerializeField] string file = "data.txt";

    string[] data;

    private void Awake()
    {
        dataSave = GetComponent<DataSave>();
        Load();
        dataSave.load_Variables(Player.transform, memoryVariable);
    }

    //Guardamos el archivo en el documento de texto en lenguaje JSON
    public void Save()
    {
        /*data = dataSave.dataToSave();

        string json = JsonUtility.ToJson(data);*/

        dataSave.save_Variables(Player.transform, memoryVariable);

        string json = JsonUtility.ToJson(dataSave);

        WriteToFile(file, json);

        Debug.Log(memoryVariable.GetValue("$ship_something"));


    }
    public bool Load()
    {
        
        string path = GetFilePath(file);
        try
        {
        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(json, dataSave);            
            return true;
        }
        catch(FileNotFoundException e) { 
            Debug.LogWarning("No existe el fichero");
            Debug.LogWarning(e);
            return false;
        }

    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        /*FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }*/
        print(path);

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(json);
        writer.Close();
    }

    //Cogemos la ruta en la que dejar el archivo de texto
    private string GetFilePath(string fileName)
    {
        return Application.dataPath + "/" + fileName;
    }
}
