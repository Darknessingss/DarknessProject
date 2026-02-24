using System;
using UnityEngine;


[Serializable]
public class Data
{
    public string Name;
    public string Number;

    public Data(string name, string number)
    {
        Name = name;
        Number = number;
    }
}
