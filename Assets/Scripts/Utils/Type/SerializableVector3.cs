using System;
using System.Collections.Generic;

using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;
 
    [JsonIgnore]
    public Vector3 UnityVector 
    {
        get => new(x, y, z);
    }
 
    public SerializableVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }
 
    public static List<SerializableVector3> GetSerializableList(List<Vector3> vList)
    {
        List<SerializableVector3> list = new List<SerializableVector3>(vList.Count);

        for (int i = 0 ; i < vList.Count ; i++)
        {
            list.Add(new SerializableVector3(vList[i]));
        }
        return list;
    }
 
    public static List<Vector3> GetSerializableList(List<SerializableVector3> vList)
    {
        List<Vector3> list = new List<Vector3>(vList.Count);

        for (int i = 0 ; i < vList.Count ; i++)
        {
            list.Add(vList[i].UnityVector);
        }
        return list;
    }
}