using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public void restartLevel()
    {
        LevelChanger.Instance.FadeToScene(SceneManager.GetActiveScene().name);
    }
}
