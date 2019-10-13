using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : Singleton<LevelChanger>
{
    private int levelToLoad;
    private Animator Animator = null;


    public void SetAnimatorPermanently(Animator animator)
    {
        if (Animator == null)
        {
            this.Animator = animator;
        }
    }


    public void FadeToLevel(int chosenTrack)
    {
        levelToLoad = chosenTrack;
        Animator.SetBool("Fade", true);
    }


    public void OnFadeComplete()
    {
        SceneManager.LoadScene("Level" + levelToLoad.ToString());
    }
}
