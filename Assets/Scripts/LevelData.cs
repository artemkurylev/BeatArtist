using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public List<ButtonItem> buttons = new List<ButtonItem>();
}

[System.Serializable]
public class ButtonItem
{
    public float time;
    public Vector2[] points;
    public bool isSlider;
}