using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TrackSelection;
using Resources;

public class TrackChooseMenuScript : MonoBehaviour
{
    public RawImage MenuBackground;
    public Text ChosenTrackName;
    public static TrackChooseMenuScript Instance;

    private SelectedTrack _selectedTrack = SelectedTrack.GetInstance();
    private ResourceLoader _resourceLoader = new ResourceLoader();
    private Image _previousHighlighted;
    private GameObject _recordButton;

    void Start()
    {
        if (Instance == null)
            Instance = gameObject.GetComponent<TrackChooseMenuScript>();
        
        _recordButton = GameObject.Find("/Canvas/Navigation bar/Record");
        if (_recordButton)
        {
            // Look how Developer Mode is defined! Currently, recording will not appear on an Android device due to it.
            _recordButton.SetActive(Globals.DeveloperMode);
        }
        GameObject.Find("Scrollbar").GetComponent<Scrollbar>().value = 1.0f;
    }

    public void OnClickPlay()
    {
        if (Globals.SelectedLevel != null)
        {
            LevelChanger.Instance.FadeToLevel();
        }
    }
    
    public void OnClickRecord()
    {
        if (Globals.SelectedLevel != null)
        {
            LevelChanger.Instance.FadeToLevelRecord();
        }
    }
    
    
    public void OnClickBack()
    {
        Debug.Log("Back button is pressed!");
    }
    
    public void UpdateHighlightedTrack(TrackListItem item)
    {
        var images = item.GetComponentsInChildren<Image>();
        Debug.Log(images);
        foreach (Image img in images)
        {
            if (img.name == "Highlight")
            {
                Color newCol;
                if (ColorUtility.TryParseHtmlString(Globals.SelectionColor, out newCol))
                {
                    newCol.a = 0.4f;
                    img.color = newCol;
        
                    if (_previousHighlighted && _previousHighlighted != img)
                    {
                        _previousHighlighted.color = Color.clear;
                    }
        
                    _previousHighlighted = img;
                }
            }
        }
    }
}