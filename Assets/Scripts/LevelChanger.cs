using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public static LevelChanger Instance;
    public Animator Animator;
    
    private string _sceneToLoad;

    public void Start()
    {
        if (Instance == null)
            Instance = this.gameObject.GetComponent<LevelChanger>();
    }

    public void FadeToLevel(string chosenTrack)
    {
        if (chosenTrack == null) throw new ArgumentNullException(nameof(chosenTrack));
        _sceneToLoad = "Level" + chosenTrack.ToString();
        Animator.SetBool("Fade", true);
    }
    
    public void FadeToScene(string scene)
    {
        _sceneToLoad = scene;
        Animator.SetBool("Fade", true);
    }


    public void OnFadeComplete()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
}
