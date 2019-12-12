using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static int nextClickableTarget = 0;
    public static float lastUpdateTime = 0;
    
    // Минимальное расстояние во времени между двумя нажатиями на экран.
    // Не позволяет исчезнуть всем целям по одному нажатию.
    public static readonly float minTimeDifference = 0.01f;
    public static readonly bool DeveloperMode = (Application.platform != RuntimePlatform.Android);
    // public static readonly bool DeveloperMode = false;
}
