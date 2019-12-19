using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordController : MonoBehaviour
{
    /// <summary>
    /// Блок с переменными
    /// </summary>
    public Target[] RoundTargets; // Объект кружочка - цели
    public string gameDataFileName;
    
    private static RecordController _instance; // Собственно сам контроллер, чтобы к нему было легко получить доступ от остальных объектов
    private AudioSource song; // Здесь хранится трек
    //public string url; - пока не используется
    private LevelData _levelData = new LevelData();

    void Start()
    {
        Globals.NextClickableTarget = 0; // Инициализируем глобальные переменные
        if (_instance == null)
            _instance = this.gameObject.GetComponent<RecordController>();

        GameObject songObject = GameObject.Find("Song");
        song = songObject.GetComponent<AudioSource>();

        if (song.clip != null && song.clip.loadState == AudioDataLoadState.Loaded)
        {
            song.Play();
        }
    }
    void Update()
    {
        if (!song.isPlaying)
        {
            FinishRecord();
        } 
        else if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            var touchPosition = touch.position;
            var height = Screen.height;
            var width = Screen.width;
            touchPosition.x = touchPosition.x / width * Globals.SceneWidth - Globals.SceneWidth / 2;
            touchPosition.y = touchPosition.y / height * Globals.SceneHeight - Globals.SceneHeight / 2;
            CreateTarget(touchPosition);
        }
        
        // Блок, обрабатывающий нажатия мыши (для дебага)
        if (Globals.DeveloperMode && Input.GetMouseButtonUp(0))
        {
            Vector2 click_position = Input.mousePosition;

            var height = Screen.height;
            var width = Screen.width;
            click_position.x = click_position.x / width * Globals.SceneWidth - Globals.SceneWidth / 2;
            click_position.y = click_position.y / height * Globals.SceneHeight - Globals.SceneHeight / 2;
            CreateTarget(click_position);
        }
    }

    private void CreateTarget(Vector2 position)
    {
        Instantiate(RoundTargets[Random.Range(0, RoundTargets.Length)], position, new Quaternion(0, 0, 0, 0));

        var item = new ButtonItem {time = song.time, points = new[] {position}, isSlider = false};
        _levelData.buttons.Add(item);
    }
    
    private void FinishRecord()
    {
        var filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
        
        if (!string.IsNullOrEmpty(filePath))
        {
            var dataAsJson = JsonUtility.ToJson(_levelData);
            File.WriteAllText(filePath, dataAsJson);
        }
        
        SceneManager.LoadScene("TrackChooseMenu");
    }
}
