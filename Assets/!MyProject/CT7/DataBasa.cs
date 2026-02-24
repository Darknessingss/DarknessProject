using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataBasa
{

    public List<Data> entries = new List<Data>();

    public DataBasa() { }

    public DataBasa(List<Data> dataList)
    {
        entries = dataList;
    }
}