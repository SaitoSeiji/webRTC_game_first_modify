using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;
using HC.Common;

public static class JsonConverter
{
    public static string ToJson_full<T>(T data)
    {
        var json = StringSerializationAPI.Serialize(typeof(T), data);
        // Assetsフォルダに保存する
        json = JsonFormatter.ToPrettyPrint(json, JsonFormatter.IndentType.Space);
        return json;
    }

    public static T FromJson_full<T>(string json)
    {
        var data = (T)StringSerializationAPI.Deserialize(typeof(T), json);
        return data;
    }
    public static string ToJson<T>(T data)
    {
        var json = JsonUtility.ToJson(data);
        return json;
    }

    public static T FromJson<T>(string json)
    {
        var data = JsonUtility.FromJson<T>(json);
        return data;
    }
}
