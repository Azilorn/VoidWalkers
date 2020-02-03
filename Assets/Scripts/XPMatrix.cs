﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPMatrix : MonoBehaviour
{
    public static List<int> xpLevelList = new List<int>();

    public void Start()
    {
        SetMatrix();
    }

    public static void SetMatrix() {

        xpLevelList.Clear();
        xpLevelList.Add(100);
        string xps = "";
        for (int i = 1; i < 50; i++) {
            xpLevelList.Add((int)(xpLevelList[i - 1] * 1.2f));
            xps += " Lvl " + i + ": " + xpLevelList[i];
        }
    }

    public static int ReturnLevel(int xp)
    {
        for (int i = 0; i < xpLevelList.Count; i++)
        {
            if (xpLevelList[i] >= xp )
            {
                return i - 1;
            }
        }
        return 0;
    }
}
