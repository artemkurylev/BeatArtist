using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TrackSelection;
using Resources;

public class TrackChooseMenuScript : MonoBehaviour
{
    public RawImage MenuBackground;

    private SelectedTrack SelectedTrack = SelectedTrack.GetInstance();
    private ResourceLoader ResourceLoader = new ResourceLoader();


    public void OnClickBack()
    {
        int MainMenu = 0;
        SceneManager.LoadScene(MainMenu);
    }


    public void OnClickTrack(int trackId)
    {
        SelectedTrack.SetId(trackId);
        StartCoroutine(ResourceLoader.GetMenuTrackBackground(trackId, MenuBackground));
    }


    public void OnClickPlay()
    {
        int chosenTrack = SelectedTrack.GetId();
        if (chosenTrack > 0)
        {
            SelectedTrack.SetId(0);
            LevelChanger.Instance.FadeToLevel(chosenTrack);
        }
    }
}