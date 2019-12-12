using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public static LevelChanger Instance;
    public Animator Animator;
    private String SceneToLoad;

    public void Start()
    {
        if (Instance == null)
            Instance = this.gameObject.GetComponent<LevelChanger>();
    }

    public void FadeToLevel(String chosenTrack)
    {
        SceneToLoad = "Level" + chosenTrack.ToString();
        Animator.SetBool("Fade", true);
    }
    
    public void FadeToScene(String scene)
    {
        SceneToLoad = scene;
        Animator.SetBool("Fade", true);
    }


    public void OnFadeComplete()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
}
