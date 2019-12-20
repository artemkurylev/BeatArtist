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
        Debug.Log("Input increase is " + lifeIncrease);
        Debug.Log("Input decrease is " + lifeDecrease);
        Debug.Log("Picture percent is " + percentToShowPicture);
        
        
        var filePath = Path.Combine(Application.streamingAssetsPath, RecordController.Instance.gameDataFileName);

        if (!string.IsNullOrEmpty(filePath))
        {
            RecordController.Instance.levelData.levelParameters = new Dictionary<string, string>
            {
                {"LifeIncrease", lifeIncrease},
                {"LifeDecrease", lifeDecrease},
                {"PercentToShowPicture", percentToShowPicture}
            };
            Debug.Log(RecordController.Instance.levelData.levelParameters["LifeIncrease"]);
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
