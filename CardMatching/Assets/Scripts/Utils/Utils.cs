using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class Utils
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public static async Task WaitForSeconds(float seconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(seconds));
    }

    public static void TrySave<T>(T data, string fileName)
    {
        string json = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            File.WriteAllText(path, json);
        }
        catch (Exception e)
        {
            Debug.Log($" JSON Loading failed {e}");
        }
    }

    public static void TryLoad<T>(string fileName, out T data)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(path))
        {
            data = default;
        }

        try
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<T>(json);
        }
        catch (Exception e)
        {
            Debug.Log($" JSON Loading failed {e}");
            data = default;
        }
    }
}
