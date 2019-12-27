using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordController : MonoBehaviour
{
    /// <summary>
    /// Блок с переменными
    /// </summary>
    public GameObject FinalCanvas;
    public static RecordController Instance;
    public LevelData levelData = new LevelData();

    private AudioSource _song; // Здесь хранится трек
    private Object[] _roundTargets; // Объект кружочка - цели
    private LevelMeta _levelMeta; // Level meta such as paths to resources
    
    //public string url; - пока не используется
    private bool _endLevel; // Flag to stop handle updates
    
    void Start()
    {
        if (!LoadGameData() || !LoadResources())
        {
            FinishRecord();
        }
        
        if (Instance == null)
            Instance = gameObject.GetComponent<RecordController>();

        if (_song.clip != null && _song.clip.loadState == AudioDataLoadState.Loaded)
        {
            _song.Play();
        }
    }
    void Update()
    {
        if (_endLevel) return;
        if (!_song.isPlaying)
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
    
    public void FinishRecord()
    {
        _endLevel = true;
        FinalCanvas.SetActive(true);
        if (_song) _song.Stop();
    }

    private void CreateTarget(Vector2 position)
    {
        Instantiate(_roundTargets[Random.Range(0, _roundTargets.Length)], position, new Quaternion(0, 0, 0, 0));

        var item = new ButtonItem {time = _song.time, points = new[] {position}, isSlider = false};
        levelData.buttons.Add(item);
    }

    private bool LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, Globals.SelectedLevel + "Meta.json");
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            _levelMeta = JsonUtility.FromJson<LevelMeta>(dataAsJson);
            return true;
        }

        return false;
    }
    
    private bool LoadResources()
    {
        _roundTargets = UnityEngine.Resources.LoadAll(Globals.TargetPath, typeof(Target));
        
        GameObject songObject = GameObject.Find("Song");
        _song = songObject.GetComponent<AudioSource>();
        _song.clip = UnityEngine.Resources.Load<AudioClip>(_levelMeta.songLink);
        
        if (_roundTargets == null || _song == null)
        {
            return false;
        }

        return true;
    }
}
