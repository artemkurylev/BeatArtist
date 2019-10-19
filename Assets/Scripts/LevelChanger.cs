using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : Singleton<LevelChanger>
{
    public Animator Animator;
    private String SceneToLoad;


    public void FadeToLevel(int chosenTrack)
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
