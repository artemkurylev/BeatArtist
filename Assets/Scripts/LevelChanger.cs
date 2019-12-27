using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator Animator;
    public static LevelChanger Instance;

    private string _sceneToLoad;
    private static readonly object Lock = new object();

    public void Start()
    {
        if (Instance == null)
        {
            Instance = gameObject.GetComponent<LevelChanger>();
        }
    }

    public void FadeToLevel()
    {
        lock (Lock)
        {
            _sceneToLoad = "Scenes/Level";
            Animator.SetBool("Fade", true);
        }
    }
    
    public void FadeToLevelRecord()
    {
        lock (Lock)
        {
            _sceneToLoad = "Scenes/LevelRecord";
            Animator.SetBool("Fade", true);
        }
    }

    public void FadeToScene(string scene)
    {
        lock (Lock)
        {
            _sceneToLoad = "Scenes/" + scene;
            Animator.SetBool("Fade", true);
        }
    }


    public void OnFadeComplete()
    {
        Debug.Log(_sceneToLoad);
        lock (Lock)
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
