using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LevelController : MonoBehaviour
{
    /// <summary>
    /// Блок с переменными
    /// </summary>
    public GameObject target; // Объект кружочка - цели
    public Texture2D[] layers; // Массив со слоями 
    // public Text Score;
    public const float Max_Life = 100; // Максимально возможный запас жизни игрока
    public static LevelController lc; // Собственно сам контроллер, чтобы к нему было легко получить доступ от остальных объектов
    public float Score; // Текущее кол-во очков
    private float time;
    private GameObject Background;
    private int layer_number = 1;
    public Slider score_slider; // Слайдер для отображения очков игрока
    public Slider hp_slider; // Слайдер для отображения жизней игрока
    private float life_points; // Текущее кол-во очков здоровья
    public float bpm; // BPM трека 
    public static float scene_Height = 10.0f; // Размеры сцены
    public static float scene_Width = 5.6f;
    public float stake_life_decrease = 0.2f; // Уменьшение жизни при промахе / исчезновении цели без попадания
    public const float stake_life_increase = 0.01f;
    public bool super_touch = false; // Нужна для определение, было ли касание по какой либо цели
    public AudioClip clip; // Здесь хранится трек
    private AudioSource song; // Здесь хранится трек
    //public string url; - пока не используется
    private int MaxPoints; // Максимальное кол-во очков для уровня
    private int NumOfTargets; //  Количество целей, появляющихся на уровне
    private int pointsPerLayer;
    public int percentToShowPict = 90;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
        if (lc == null)
            lc = this.gameObject.GetComponent<LevelController>();
        Background = GameObject.Find("Background");
        // int x = Background.transform.childCount;
        this.life_points = Max_Life;
        hp_slider.value = this.life_points;
        GameObject songObject = GameObject.Find("Song");
        song = songObject.GetComponent<AudioSource>();
        
        
        //StartCoroutine(GetAudioClip());
        if (song.clip != null && song.clip.loadState == AudioDataLoadState.Loaded)
            song.Play();
        Debug.Log(this.song.clip.length);
        NumOfTargets = (int)this.song.clip.length / (int)bpm;
        
        MaxPoints = NumOfTargets * Target.maxScore;
        pointsPerLayer = (MaxPoints * percentToShowPict / 100) / this.layers.Length;

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
        float cur_time = Time.time;
        if(life_points <= 0)
        {
            SceneManager.LoadScene("TrackChooseMenu");
        }
        if(cur_time - time > 60/bpm)
        {
            time = cur_time;
            Vector3 pos = this.transform.position; 
            Instantiate(target, GeneratePosition(), new Quaternion(0,0,0,0));
            increaseLife();
        }
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                if (!super_touch)
                    decreaseLIfe();
                super_touch = false;
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
        if (this.Score / this.pointsPerLayer > layer_number - 1)
        {
            layer_number++;
            //string new_layer = name_template + layer_number.ToString() + format;
            Texture2D tex = layers[layer_number];
            Debug.Log(layer_number);
            GameObject BackImage = GameObject.Find("BackImage");
            BackImage.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        score_slider.value = this.Score;

    }
    public void increaseLife()
    {
        this.life_points += this.life_points * stake_life_increase;
        hp_slider.value = this.life_points;
    }
    public void decreaseLIfe()
    {
        this.life_points -= Max_Life * stake_life_decrease;
        hp_slider.value = this.life_points;
    }
}
