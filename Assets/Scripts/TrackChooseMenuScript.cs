﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TrackSelection;
using Resources;
using UnityEditor;

public class TrackChooseMenuScript : MonoBehaviour
{
    public RawImage MenuBackground;
    public Text ChosenTrackName;
    public TrackChooseMenuScript Controller;

    private SelectedTrack SelectedTrack = SelectedTrack.GetInstance();
    private ResourceLoader ResourceLoader = new ResourceLoader();
    private Image PreviousHighlighted;


    void Start()
    {
        if (Controller)
        {
            Controller.OnClickTrack(1);
            GameObject.Find("Scrollbar").GetComponent<Scrollbar>().value = 1.0f;
        }
    }


    public void OnClickTrack(int trackId)
    {
        SelectedTrack.SetId(trackId);
        var track = GameObject.Find("Track" + trackId.ToString());

        var texts = track.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            if (text.name == "Song name")
            {
                ChosenTrackName.text = text.text;
            }
        }
        
        var images = track.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            Debug.Log(img.name);
            if (img.name == "Highlight")
            {
                string htmlValue = "#7ABAF2";
                Color newCol;
                if (ColorUtility.TryParseHtmlString(htmlValue, out newCol))
                {
                    newCol.a = 0.4f;
                    img.color = newCol;

                    if (PreviousHighlighted && PreviousHighlighted != img)
                    {
                        PreviousHighlighted.color = Color.clear;
                    }

                    PreviousHighlighted = img;
                }
            }
        }
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
    
    
    public void OnClickBack()
    {
        Debug.Log("Back button is pressed!");
    }
}