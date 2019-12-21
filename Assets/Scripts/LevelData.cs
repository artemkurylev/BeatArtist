using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public List<ButtonItem> buttons = new List<ButtonItem>();
    public LevelParameters levelParameters = new LevelParameters();
}

[System.Serializable]
public class ButtonItem
{
    public float time;
    public Vector2[] points;
    public bool isSlider;
}

[System.Serializable]
public class LevelParameters
{
    public string lifeIncrease;
    public string lifeDecrease;
    public string percentToShowPicture;
}