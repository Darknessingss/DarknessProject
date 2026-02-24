using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
public static class Utils
{
    private const string ParsedFileName = "DataBase_ParsedToJson";

    public static void ParseCSV(string csvFilePath = "Assets/!MyProject/CT7/my_table.csv")
    {
        if (!File.Exists(csvFilePath))
        {
            Debug.LogError($"CSV файл не найден по пути: {csvFilePath}");
            return;
        }

        string[] lines = File.ReadAllLines(csvFilePath);
        List<Data> dataList = new List<Data>();

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');
            if (values.Length >= 2)
            {
                string name = values[0].Trim();
                string number = values[1].Trim();
                dataList.Add(new Data(name, number));
            }
        }

        DataBasa database = new DataBasa(dataList);
        string jsonString = JsonConvert.SerializeObject(database, Formatting.Indented);

        string resourcesFolderPath = Path.Combine(Application.dataPath, "Resources");
        if (!Directory.Exists(resourcesFolderPath))
        {
            Directory.CreateDirectory(resourcesFolderPath);
        }

        string filePath = Path.Combine(resourcesFolderPath, ParsedFileName + ".txt");

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.Write(jsonString);
        }

        Debug.Log($"JSON файл успешно создан: {filePath}");

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}