using UnityEngine;
using Newtonsoft.Json;

public class Deserializer : MonoBehaviour
{
    public string jsonFileName = "DataBase_ParsedToJson";

    [ContextMenu("Load JSON Data")]
    public void LoadData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);

        if (jsonFile == null)
        {
            Debug.LogError($"Файл '{jsonFileName}.txt' не найден в Resources! Сначала запустите парсинг.");
            return;
        }

        try
        {
            DataBasa loadedData = JsonConvert.DeserializeObject<DataBasa>(jsonFile.text);

            if (loadedData != null && loadedData.entries != null)
            {
                Debug.Log($"Данные успешно загружены. Найдено записей: {loadedData.entries.Count}");
                foreach (var entry in loadedData.entries)
                {
                    Debug.Log($"Name: {entry.Name}, Number: {entry.Number}");
                }
            }
            else
            {
                Debug.LogError("Не удалось десериализовать данные или файл пуст.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка при десериализации JSON: {e.Message}");
        }
    }
}
