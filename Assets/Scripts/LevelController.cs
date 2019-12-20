using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    /// <summary>
    /// Блок с переменными
    /// </summary>
    public Target[] RoundTargets; // Объект кружочка - цели
    public Texture2D[] layers; // Массив со слоями
    // public Text Score;
    public const float MaxLife = 100; // Максимально возможный запас жизни игрока
    public static LevelController Instance; // Собственно сам контроллер, чтобы к нему было легко получить доступ от остальных объектов
    public Text FinalScore;
    public GameObject FinalCanvas;
    public Slider ScoreSlider; // Слайдер для отображения очков игрока
    public Slider HpSlider; // Слайдер для отображения жизней игрока
    
    // public float bpm = 120; // BPM трека 
    public Animator animator; // Объект, отвественный за вызов анимаций уровня
    public string GameDataFileName = "";
    private float _lifePoints; // Current user health
    private float _score; // Current user score
    private float _nextBeatExpiration; // Deadline to hit next target
    private int _layerNumber; // Current layer number
    private AudioSource _song; // Audio source
    //public string url; - Currently not used
    private int _maxScore; // Maximum possible score
    private int _numOfTargets; //  Total number of targets on a level
    private int _scorePerLayer; // Score to hit to get next layer of background image
    private int _targetCounter; // Counter for created targets
    private LevelData _levelData; // Level data such as array of targets
    
    private float _lifeIncrease;
    private float _lifeDecrease; // Уменьшение жизни при промахе / исчезновении цели без попадания
    private int _percentToShowPict; // Процент очков необходимый для показа полной картинки
    
    void Start()
    {
        if(!LoadGameData() || _levelData.buttons.Count == 0)
        {
            _maxScore = 0;
            EndLevel();
        }
        
        Globals.NextClickableTarget = 0; // Инициализируем глобальные переменные
        _nextBeatExpiration = _levelData.buttons[0].time;
        if (Instance == null)
            Instance = gameObject.GetComponent<LevelController>();
        _lifePoints = MaxLife;
        HpSlider.value = _lifePoints;
        
        GameObject songObject = GameObject.Find("Song");
        _song = songObject.GetComponent<AudioSource>();

        //StartCoroutine(GetAudioClip());
        if (_song.clip != null && _song.clip.loadState == AudioDataLoadState.Loaded)
        {
            _song.Play();
        }
        _numOfTargets = _levelData.buttons.Count;
        _maxScore = _numOfTargets * Target.MaxScore;
        _scorePerLayer = (_maxScore * _percentToShowPict / 100) / layers.Length;
        ScoreSlider.maxValue = _maxScore;
        Debug.Log(_levelData.levelParameters.Keys.Count);
        foreach (var a in _levelData.levelParameters.Keys)
        {
            Debug.Log(a);
        }
        _lifeIncrease = (float) Convert.ToInt32(_levelData.levelParameters["LifeIncrease"]) / 100;
        _lifeDecrease = (float) Convert.ToInt32(_levelData.levelParameters["LifeDecrease"]) / 100;
        _percentToShowPict = Convert.ToInt32(_levelData.levelParameters["PercentToShowPicture"]);
        
        Debug.Log("Input increase is " + _lifeIncrease);
        Debug.Log("Input decrease is " + _lifeDecrease);
        Debug.Log("Picture percent is " + _percentToShowPict);
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
        if (_song)
        {
            float cur_time = _song.time;
            if (_lifePoints <= 0 || Globals.NextClickableTarget >= _levelData.buttons.Count || !_song.isPlaying)
            {
                EndLevel();
            }
            else
            {
                if (_nextBeatExpiration - cur_time < Globals.TimeToHit && _targetCounter < _levelData.buttons.Count)
                {
                    Target target = Instantiate(RoundTargets[Random.Range(0, RoundTargets.Length)], GetPosition(), 
                        new Quaternion(0, 0, 0, 0));
                    target.currentNumber = _targetCounter;
                    _targetCounter++;
                    if (_targetCounter < _levelData.buttons.Count)
                    {
                        _nextBeatExpiration = _levelData.buttons[_targetCounter].time;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    public void UpdateScore(float score)
    {
        _score += score;
        if (_score / _scorePerLayer > _layerNumber)
        {
            //string new_layer = name_template + layer_number.ToString() + format;
            Texture2D tex = layers[_layerNumber++];
            GameObject BackImage = GameObject.Find("BackImage");
            BackImage.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width,
                tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        ScoreSlider.value = _score;

    }
    public void DecreaseLife()
    {
        _lifePoints -= MaxLife * _lifeDecrease;
        HpSlider.value = _lifePoints;
    }
    public void IncreaseLife()
    {
        _lifePoints += _lifePoints * _lifeIncrease;
        HpSlider.value = _lifePoints;
    }
    private Vector2 GetPosition()
    {
        return _levelData.buttons[_targetCounter].points[0];
    }
    private void EndLevel()
    {
        FinalCanvas.SetActive(true);
        int finalScore = (int) _score;
        FinalScore.text = "Your final score is: " + finalScore.ToString() + " of " + _maxScore;
        if (_song) _song.Stop();
    }
    private bool LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, GameDataFileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            _levelData = JsonUtility.FromJson<LevelData>(dataAsJson);

            return true;
        }

        return false;
    }
}
