using System.IO;
using Resources;
using UnityEngine;
using UnityEngine.UI;

public class TrackListItem : MonoBehaviour
{
    public string levelName;
    
    private LevelMeta _levelMeta;
    
    public void OnClickTrack()
    {
        Debug.Log("Click");
        LoadLevelMeta();
        
        // Set name of a chosen track
        Debug.Log(_levelMeta);
        if (_levelMeta != null)
        {
            TrackChooseMenuScript.Instance.ChosenTrackName.text = _levelMeta.name;
        }
        
        // var texts = track.GetComponentsInChildren<Text>();
        // foreach (Text text in texts)
        // {
        //     if (text.name == "Song name")
        //     {
        //         ChosenTrackName.text = text.text;
        //     }
        // }
        
        // Highlight chosen track in the list
        // SelectedTrack.SetId(trackId);
        
        TrackChooseMenuScript.Instance.UpdateHighlightedTrack(this);
        Globals.SelectedLevel = levelName; // Update selected level
        // StartCoroutine(ResourceLoader.GetMenuTrackBackground(trackId, MenuBackground));
    }
    
    private bool LoadLevelMeta()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, levelName + "Meta.json");
        Debug.Log(filePath);
        
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            _levelMeta = JsonUtility.FromJson<LevelMeta>(dataAsJson);
            return true;
        }

        return false;
    }
}