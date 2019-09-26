using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TrackSelection;
using Resources;
using System;

public class TrackChooseMenuScript : MonoBehaviour
{
    private SelectedTrack SelectedTrack = SelectedTrack.GetInstance();
    private ResourceLoader ResourceLoader = new ResourceLoader();
    public RawImage MenuBackground;

    public void OnClickBack()
    {
        int MainMenu = 0;
        SceneManager.LoadScene(MainMenu);
    }

    public void OnClickTrack(int trackId)
    {
        SelectedTrack.SetId(trackId);
        // MenuBackground.texture = ResourceLoader.GetMenuTrackBackground(trackId);
        StartCoroutine(ResourceLoader.GetMenuTrackBackground(trackId, MenuBackground));
        Debug.Log(MenuBackground.texture);
    }

    public void OnClickPlay()
    {
        int ChosenTrack = SelectedTrack.GetId();
        if (ChosenTrack >= 0)
        {
            SceneManager.LoadScene(ChosenTrack);
        }
    }
}