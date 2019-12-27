using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    public void OpenMenu()
    {
        LevelChanger.Instance.FadeToScene("TrackChooseMenu");
    }

    public void OpenMenuAndSaveRecord()
    {
        var lifeIncrease = GetInputFieldData("LifeIncrease");
        var lifeDecrease = GetInputFieldData("LifeDecrease");
        var percentToShowPicture = GetInputFieldData("PercentToShowPicture");

        var filePath = Path.Combine(Application.streamingAssetsPath, Globals.SelectedLevel + ".json");
        if (!string.IsNullOrEmpty(filePath))
        {
            RecordController.Instance.levelData.levelParameters.lifeIncrease = lifeIncrease;
            RecordController.Instance.levelData.levelParameters.lifeDecrease = lifeDecrease;
            RecordController.Instance.levelData.levelParameters.percentToShowPicture = percentToShowPicture;
            Debug.Log(RecordController.Instance.levelData.levelParameters.lifeIncrease);
            var dataAsJson = JsonUtility.ToJson(RecordController.Instance.levelData);
            Debug.Log(dataAsJson);
            File.WriteAllText(filePath, dataAsJson);
        }
        
        LevelChanger.Instance.FadeToScene("TrackChooseMenu");
    }

    private string GetInputFieldData(string inputField)
    {
        var content = GameObject.Find("FinalCanvas/" + inputField + "/Text").GetComponent<Text>().text;
        if (content.Length == 0)
        {
            content = GameObject.Find("FinalCanvas/" + inputField + "/Placeholder").GetComponent<Text>().text;
        }

        return content;
    }
}
