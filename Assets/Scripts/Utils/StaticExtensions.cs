using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class StaticExtensions {
    public static Color WithAlpha(this Color color, float alpha) {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static Vector3 WithY(this Vector3 vector, float y) {
        return new Vector3(vector.x, y, vector.z);
    }

    public static Vector3 WithX(this Vector3 vector, float x) {
        return new Vector3(x, vector.y, vector.z);
    }

    public static void DOPop(this Transform transform) {
        transform.DOKill();
        transform.DOPunchScale(Vector3.one * 0.1f, 0.25f);
    }

    // From: https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}