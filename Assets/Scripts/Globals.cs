using UnityEngine;

public static class Globals
{
    // Id of a next target that user should click.
    // Any other clicked target is considered as a miss.
    public static int NextClickableTarget = 0; 
    public static float LastUpdateTime = 0; // Time of last target disappearance, either on a hit or on a miss
    public static string SelectedLevel; // Level selected by player
    
    // Minimum time interval between two user taps on a screen
    // Saves from situation when all targets disappear on a single touch
    public const float MinTimeDifference = 0.01f;
    public const float TimeToHit = 2; // Time interval in which it is possible to hit a target  (in seconds)
    public const string SelectionColor = "#7ABAF2"; // Color of a selection highlight
    public const string TargetPath = "Prefabs/Targets"; // Path to target prefabs
    public const float SceneHeight = 10.0f; // Размеры сцены
    public const float SceneWidth = 5.6f;
    public const float MaxLife = 100; // Максимально возможный запас жизни игрока

    public static readonly bool DeveloperMode = (Application.platform != RuntimePlatform.Android);
    // public static readonly bool DeveloperMode = false;
}
