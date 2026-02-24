using UnityEngine;

public class Parser : MonoBehaviour
{
    public string csvFilePath = "Assets/!MyProject/CT7/my_table.csv";

    [ContextMenu("Run CSV Parsing")]
    public void RunParsing()
    {
        Utils.ParseCSV(csvFilePath);
    }
}