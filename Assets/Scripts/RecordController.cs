using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class RecordController : MonoBehaviour
{
    /// <summary>
    /// Блок с переменными
    /// </summary>
    public Target[] RoundTargets; // Объект кружочка - цели
    public Texture2D[] layers; // Массив со слоями
    // public Text Score;
    public const float MaxLife = 100; // Максимально возможный запас жизни игрока
    public static RecordController lc; // Собственно сам контроллер, чтобы к нему было легко получить доступ от остальных объектов
    public float Score; // Текущее кол-во очков
    public Text FinalScore;
    public GameObject FinalCanvas;
    public static float SceneHeight = 10.0f; // Размеры сцены
    public static float SceneWidth = 5.6f;
    public bool SuperTouch = false; // Нужна для определение, было ли касание по какой либо цели
    public AudioClip Clip; // Здесь хранится трек
    public int PercentToShowPict = 90;//Процент очков необходимый для показа полной картинки
    public float bpm = 120; // BPM трека 
    public Animator animator; // Объект, отвественный за вызов анимаций уровня
    public string gameDataFileName;

    private float m_lifePoints; // Текущее кол-во очков здоровья
    private float m_time;
    private int layer_number = 1;
    private AudioSource song; // Здесь хранится трек
    //public string url; - пока не используется
    private int MaxPoints; // Максимальное кол-во очков для уровня
    private int m_pointsPerLayer;
    private bool m_appearFlag = false;
    private int targetCounter = 0;
    private LevelData _levelData = new LevelData();
    // Start is called before the first frame update
    void Start()
    {
        Globals.nextClickableTarget = 0; // Инициализируем глобальные переменные
        m_time = Time.time;
        if (lc == null)
            lc = this.gameObject.GetComponent<RecordController>();

        GameObject songObject = GameObject.Find("Song");
        song = songObject.GetComponent<AudioSource>();

        if (song.clip != null && song.clip.loadState == AudioDataLoadState.Loaded)
        {
            song.Play();
            m_time = song.time;
        }
    }
    void Update()
    {
        if (!this.song.isPlaying)
        {
            FinishRecord();
        } 
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touch_position = touch.position;
            int height = Screen.height;
            int width = Screen.width;
            touch_position.x = touch_position.x / width * SceneWidth - SceneWidth / 2;
            touch_position.y = touch_position.y / height * SceneHeight - SceneHeight / 2;
            CreateTarget(touch_position);
        }
        
        // Блок, обрабатывающий нажатия мыши (для дебага)
        if (Globals.DeveloperMode && Input.GetMouseButtonUp(0))
        {
            Vector2 click_position = Input.mousePosition;

            int height = Screen.height;
            int width = Screen.width;
            click_position.x = click_position.x / width * SceneWidth - SceneWidth / 2;
            click_position.y = click_position.y / height * SceneHeight - SceneHeight / 2;
            CreateTarget(click_position);
        }
    }

    private void CreateTarget(Vector2 position)
    {
        Instantiate(RoundTargets[Random.Range(0, RoundTargets.Length)], position, new Quaternion(0, 0, 0, 0));

        ButtonItem item = new ButtonItem {time = song.time, points = new[] {position}, isSlider = false};
        _levelData.buttons.Add(item);
    }
    
    private void FinishRecord()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
        
        if (!string.IsNullOrEmpty(filePath))
        {
            string dataAsJson = JsonUtility.ToJson(_levelData);
            File.WriteAllText(filePath, dataAsJson);
        }
        
        SceneManager.LoadScene("TrackChooseMenu");
    }
}
