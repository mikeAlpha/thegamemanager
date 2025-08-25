using System;
using System.Collections.Generic;
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
}
