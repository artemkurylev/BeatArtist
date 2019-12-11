using System.Collections;
using System.Collections.Generic;
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
    public float Score; // Текущее кол-во очков
    public Text FinalScore;
    public GameObject FinalCanvas;
    public Slider ScoreSlider; // Слайдер для отображения очков игрока
    public Slider HpSlider; // Слайдер для отображения жизней игрока
    public static float SceneHeight = 10.0f; // Размеры сцены
    public static float SceneWidth = 5.6f;
    public float LifeDecrease = 0.2f; // Уменьшение жизни при промахе / исчезновении цели без попадания
    public const float stake_life_increase = 0.01f;
    public bool SuperTouch = false; // Нужна для определение, было ли касание по какой либо цели
    public AudioClip Clip; // Здесь хранится трек
    public int PercentToShowPict = 90;//Процент очков необходимый для показа полной картинки
    public float bpm = 120; // BPM трека 
    public Animator animator; // Объект, отвественный за вызов анимаций уровня
    
    private float m_lifePoints; // Текущее кол-во очков здоровья
    private float m_time;
    private int layer_number = 1;
    private AudioSource song; // Здесь хранится трек
    //public string url; - пока не используется
    private int MaxPoints; // Максимальное кол-во очков для уровня
    private int NumOfTargets; //  Количество целей, появляющихся на уровне
    private int m_pointsPerLayer;
    private bool m_appearFlag = false;
    private int targetCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        Globals.nextClickableTarget = 0; // Инициализируем глобальные переменные
        m_time = Time.time;
        if (lc == null)
            lc = this.gameObject.GetComponent<LevelController>();
        this.m_lifePoints = MaxLife;
        HpSlider.value = this.m_lifePoints;
        
        GameObject songObject = GameObject.Find("Song");
        song = songObject.GetComponent<AudioSource>();

        //StartCoroutine(GetAudioClip());
        if (song.clip != null && song.clip.loadState == AudioDataLoadState.Loaded)
        {
            song.Play();
            m_time = song.time;
        }
        Debug.Log(this.song.clip.length);
        NumOfTargets = (int)this.song.clip.length * 60/ (int)bpm;
        MaxPoints = NumOfTargets * Target.MaxScore;
        m_pointsPerLayer = (MaxPoints * PercentToShowPict / 100) / this.layers.Length;
        ScoreSlider.maxValue = MaxPoints;
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
    // Update is called once per frame
    void Update()
    {
        Debug.Log(song.isPlaying);
        float cur_time = song.time;
        if (m_lifePoints <= 0)
        {
            //SceneManager.LoadScene("TrackChooseMenu");
            FinalCanvas.SetActive(true);
            int finalScore = (int)this.Score;
            FinalScore.text = "Your final score is: " + finalScore.ToString() + " of " + lc.MaxPoints;
            song.Stop();
        }
        else if (!this.song.isPlaying)
        {
            int score = (int)this.Score;
            FinalScore.text = " Your final score:\n" + score.ToString() + " of " + this.MaxPoints.ToString();
                // End Level
            FinalCanvas.SetActive(true);
        }
        else
        {
            if (!m_appearFlag && (cur_time - m_time > 4 * 60 / bpm))
            {
                m_appearFlag = true;
                m_time = cur_time;
                Vector3 pos = this.transform.position;
                Target target = Instantiate(RoundTargets[Random.Range(0, RoundTargets.Length)], GeneratePosition(), new Quaternion(0, 0, 0, 0));
                target.currentNumber = targetCounter;
                targetCounter++;
                increaseLife();
            }
            if (cur_time - m_time > 60 / bpm && m_appearFlag)
            {
                m_time = cur_time;
                Vector3 pos = this.transform.position;
                Target target = Instantiate(RoundTargets[Random.Range(0, RoundTargets.Length)], GeneratePosition(), new Quaternion(0, 0, 0, 0));
                target.currentNumber = targetCounter;
                targetCounter++;
                increaseLife();
            }
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                // if (touch.phase == TouchPhase.Ended)
                // {
                //     if (!SuperTouch)
                //         decreaseLIfe();
                //     SuperTouch = false;
                // }
            }
            
        }
    }
    private Vector3 GeneratePosition()
    {
        float x, y, z;
        z = -1;
        x = Random.Range(-1.9f, 1.9f);
        y = Random.Range(-3.5f, 2.5f);
        return new Vector3(x, y, z);
    }
    public void updateScore(float score)
    {
        this.Score += score;
        /*f (Score != null)
        {
            Score.text = this.Score.ToString();
        }*/
        if (this.Score / this.m_pointsPerLayer > layer_number - 1)
        {
            layer_number++;
            //string new_layer = name_template + layer_number.ToString() + format;
            Texture2D tex = layers[layer_number];
            GameObject BackImage = GameObject.Find("BackImage");
            BackImage.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        ScoreSlider.value = this.Score;

    }
    public void increaseLife()
    {
        this.m_lifePoints += this.m_lifePoints * stake_life_increase;
        HpSlider.value = this.m_lifePoints;
    }
    public void decreaseLIfe()
    {
        this.m_lifePoints -= MaxLife * LifeDecrease;
        HpSlider.value = this.m_lifePoints;
    }
}
