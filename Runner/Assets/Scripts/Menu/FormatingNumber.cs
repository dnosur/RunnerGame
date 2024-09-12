using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormatingNumber
{

    private static string[] names = {
        "",
        "K",
        "M",
        "B",
        "T",
    };

    public static string FormatingNum(int num)
    {
        if (num == 0) return "0";

        double helpingObject = num;

        int i = 0;
        for (; i + 1 < names.Length && helpingObject / 1000 > 1;)
        {
            helpingObject /= 1000;
            Debug.Log(helpingObject);
            i++;
        }

        return helpingObject.ToString("#.##") + names[i];

    }
}
