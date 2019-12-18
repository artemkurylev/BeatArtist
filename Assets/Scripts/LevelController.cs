using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LevelController : MonoBehaviour
{
    /// <summary>
    /// Блок с переменными
    /// </summary>
    public Target[] RoundTargets; // Объект кружочка - цели
    public Texture2D[] layers; // Массив со слоями
    // public Text Score;
    public const float MaxLife = 100; // Максимально возможный запас жизни игрока
    public static LevelController lc; // Собственно сам контроллер, чтобы к нему было легко получить доступ от остальных объектов
    public Text FinalScore;
    public GameObject FinalCanvas;
    public Slider ScoreSlider; // Слайдер для отображения очков игрока
    public Slider HpSlider; // Слайдер для отображения жизней игрока
    public static float SceneHeight = 10.0f; // Размеры сцены
    public static float SceneWidth = 5.6f;
    public float LifeDecrease = 0.2f; // Уменьшение жизни при промахе / исчезновении цели без попадания
    public const float stake_life_increase = 0.01f;
    public int PercentToShowPict = 90;//Процент очков необходимый для показа полной картинки
    public float bpm = 120; // BPM трека 
    public Animator animator; // Объект, отвественный за вызов анимаций уровня
    public string GameDataFileName = "";
    
    private float m_lifePoints; // Текущее кол-во очков здоровья
    private float m_nextBeatExpiration;
    private int m_layer_number = 1;
    private AudioSource m_song; // Здесь хранится трек
    //public string url; - пока не используется
    private float m_score; // Текущее кол-во очков
    private int m_maxScore; // Максимальное кол-во очков для уровня
    private int m_numOfTargets; //  Количество целей, появляющихся на уровне
    private int m_pointsPerLayer;
    private int m_targetCounter = 0; // Counter for created beat buttons
    private LevelData m_LevelData; // Level data such as array of beat buttons
    // Start is called before the first frame update
    void Start()
    {
        if(!LoadGameData() || m_LevelData.buttons.Count == 0)
        {
            m_maxScore = 0;
            EndLevel();
        }
        
        Globals.nextClickableTarget = 0; // Инициализируем глобальные переменные
        m_nextBeatExpiration = m_LevelData.buttons[0].time;
        if (lc == null)
            lc = gameObject.GetComponent<LevelController>();
        m_lifePoints = MaxLife;
        HpSlider.value = m_lifePoints;
        
        GameObject songObject = GameObject.Find("Song");
        m_song = songObject.GetComponent<AudioSource>();

        //StartCoroutine(GetAudioClip());
        if (m_song.clip != null && m_song.clip.loadState == AudioDataLoadState.Loaded)
        {
            m_song.Play();
        }
        m_numOfTargets = m_LevelData.buttons.Count;
        m_maxScore = m_numOfTargets * Target.MaxScore;
        m_pointsPerLayer = (m_maxScore * PercentToShowPict / 100) / layers.Length;
        ScoreSlider.maxValue = m_maxScore;
    }
    //IEnumerator GetAudioClip()
    //{
    //    using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
    //    {
    //        yield return www.SendWebRequest();
    //        if (www.isNetworkError)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            song.clip = DownloadHandlerAudioClip.GetContent(www);
    //        }
    //    }
    //}
    void Update()
    {
        if (m_song)
        {
            float cur_time = m_song.time;
            if (m_lifePoints <= 0 || Globals.nextClickableTarget >= m_LevelData.buttons.Count || !m_song.isPlaying)
            {
                EndLevel();
            }
            else
            {
                if (m_nextBeatExpiration - cur_time < Globals.timeToResponse && m_targetCounter < m_LevelData.buttons.Count)
                {
                    Target target = Instantiate(RoundTargets[Random.Range(0, RoundTargets.Length)], GetPosition(), 
                        new Quaternion(0, 0, 0, 0));
                    target.currentNumber = m_targetCounter;
                    m_targetCounter++;
                    if (m_targetCounter < m_LevelData.buttons.Count)
                    {
                        m_nextBeatExpiration = m_LevelData.buttons[m_targetCounter].time;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    public void UpdateScore(float score)
    {
        m_score += score;
        if (m_score / m_pointsPerLayer > m_layer_number - 1)
        {
            m_layer_number++;
            //string new_layer = name_template + layer_number.ToString() + format;
            Texture2D tex = layers[m_layer_number];
            GameObject BackImage = GameObject.Find("BackImage");
            BackImage.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width,
                tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        ScoreSlider.value = m_score;

    }
    public void DecreaseLife()
    {
        m_lifePoints -= MaxLife * LifeDecrease;
        HpSlider.value = m_lifePoints;
    }
    public void IncreaseLife()
    {
        m_lifePoints += m_lifePoints * stake_life_increase;
        HpSlider.value = m_lifePoints;
    }
    private Vector2 GetPosition()
    {
        return m_LevelData.buttons[m_targetCounter].points[0];
    }
    private void EndLevel()
    {
        FinalCanvas.SetActive(true);
        int finalScore = (int) m_score;
        FinalScore.text = "Your final score is: " + finalScore.ToString() + " of " + m_maxScore;
        if (m_song) m_song.Stop();
    }
    private bool LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, GameDataFileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            m_LevelData = JsonUtility.FromJson<LevelData>(dataAsJson);

            return true;
        }

        return false;
    }
}
