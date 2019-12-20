using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public List<ButtonItem> buttons = new List<ButtonItem>();
    public Dictionary<string, string> levelParameters = new Dictionary<string, string>();
}

[System.Serializable]
public class ButtonItem
{
    public float time;
    public Vector2[] points;
    public bool isSlider;
}