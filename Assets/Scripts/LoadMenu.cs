using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    public void OpenMenu()
    {
        LevelChanger.Instance.FadeToScene("TrackChooseMenu");
    }
}
