using UnityEngine;
using System.Collections;

public static class ExtensionMethods {

    public static string ColorToHex(this Color32 color)
    {
        string hex = "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

}
