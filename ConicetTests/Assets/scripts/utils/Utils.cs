 ﻿using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;
 
 public static class Utils {

    public static void Shuffle<T>(List<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
    public static void RemoveAllChildsIn(Transform container)
    {
        int num = container.transform.childCount;
        for (int i = 0; i < num; i++) UnityEngine.Object.DestroyImmediate(container.transform.GetChild(0).gameObject);
    }

    public static class CoroutineUtil
    {
        public static IEnumerator WaitForRealSeconds(float time)
        {
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + time)
            {
                yield return null;
            }
        }
    }
    public static string FormatNumbers(int num)
    {
        return string.Format("{0:#,#}", num);
    }
    public static int ConvertToToTimestamp(System.DateTime value)
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(value - epochStart).TotalSeconds;
        return cur_time;
    }
    public static int GetTotalSecondsFromString(string text)
    {
        string[] arr = text.Split(":"[0]);
        if (arr.Length < 2) return 0;

        int min = int.Parse(arr[0]);
        int sec = int.Parse(arr[1]);
        return sec + (60*min);
    }
    public static string ParseText(string text)
    {
        if (text.Length < 2)
            return text;
        return text.Replace("_", " ");

    }
}
