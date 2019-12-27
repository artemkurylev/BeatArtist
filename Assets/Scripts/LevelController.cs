using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;
using Application = UnityEngine.Application;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    /// <summary>
    /// Блок с переменными
    /// </summary>
    // public Text Score;
    public static LevelController Instance; // Собственно сам контроллер, чтобы к нему было легко получить доступ от остальных объектов
    public Text FinalScore;
    public GameObject FinalCanvas;
    public Slider ScoreSlider; // Слайдер для отображения очков игрока
    public Slider HpSlider; // Слайдер для отображения жизней игрока
    // public float bpm = 120; // BPM трека 
    public Animator animator; // Объект, отвественный за вызов анимаций уровня
    
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
    private bool _endLevel; // Flag to stop handle updates
    private Object[] _layers; // Массив со слоями
    private Object[] _roundTargets; // Объект кружочка - цели
    private LevelData _levelData; // Level data such as array of targets
    private LevelMeta _levelMeta; // Level meta such as paths to resources
    private float _lifeIncrease;
    private float _lifeDecrease; // Уменьшение жизни при промахе / исчезновении цели без попадания
    private int _percentToShowPict; // Процент очков необходимый для показа полной картинки

    void Start()
    {
        if(!LoadGameData() || _levelData.buttons.Count == 0 || !LoadResources())
        {
            _maxScore = 0;
            EndLevel();
        }
        
        if (Instance == null)
            Instance = gameObject.GetComponent<LevelController>();

        Globals.NextClickableTarget = 0;
        _nextBeatExpiration = _levelData.buttons[0].time;
        _lifeIncrease = (float) Convert.ToInt32(_levelData.levelParameters.lifeIncrease) / 100;
        _lifeDecrease = (float) Convert.ToInt32(_levelData.levelParameters.lifeDecrease) / 100;
        _percentToShowPict = Convert.ToInt32(_levelData.levelParameters.percentToShowPicture);
        _lifePoints = Globals.MaxLife;
        HpSlider.value = _lifePoints;
        _numOfTargets = _levelData.buttons.Count;
        _maxScore = _numOfTargets * Target.MaxScore;
        _scorePerLayer = (_maxScore * _percentToShowPict / 100) / _layers.Length;
        ScoreSlider.maxValue = _maxScore;

        //StartCoroutine(GetAudioClip());
        if (_song != null && _song.clip != null && _song.clip.loadState == AudioDataLoadState.Loaded)
        {
            _song.Play();
        }
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
        if (_endLevel) return;
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
                    Target target = (Target) Instantiate(_roundTargets[Random.Range(0, _roundTargets.Length)], GetPosition(), 
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
            _layerNumber = Math.Min(_layers.Length - 1, (int) _score / _scorePerLayer);
            Texture2D tex = (Texture2D) _layers[_layerNumber];
            GameObject BackImage = GameObject.Find("BackImage");
            BackImage.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width,
                tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        ScoreSlider.value = _score;
    }
    public void DecreaseLife()
    {
        _lifePoints -= _lifeDecrease * Globals.MaxLife;
        HpSlider.value = _lifePoints;
    }
    public void IncreaseLife()
    {
        _lifePoints = Math.Min(Globals.MaxLife, _lifePoints + _lifeIncrease * Globals.MaxLife);
        HpSlider.value = _lifePoints;
    }
    private Vector2 GetPosition()
    {
        return _levelData.buttons[_targetCounter].points[0];
    }
    private void EndLevel()
    {
        _endLevel = true;
        FinalCanvas.SetActive(true);
        int finalScore = (int) _score;
        FinalScore.text = "Your final score is: " + finalScore.ToString() + " of " + _maxScore;
        if (_song) _song.Stop();
    }
    private bool LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, Globals.SelectedLevel + ".json");

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            _levelData = JsonUtility.FromJson<LevelData>(dataAsJson);
            
            filePath = Path.Combine(Application.streamingAssetsPath, Globals.SelectedLevel + "Meta.json");
            if (File.Exists(filePath))
            {
                dataAsJson = File.ReadAllText(filePath);
                _levelMeta = JsonUtility.FromJson<LevelMeta>(dataAsJson);
                return true;
            }
        }
        
        return false;
    }

    private bool LoadResources()
    {
        _layers = UnityEngine.Resources.LoadAll(_levelMeta.pictureLink, typeof(Texture2D));
        _roundTargets = UnityEngine.Resources.LoadAll(Globals.TargetPath, typeof(Target));
        
        GameObject songObject = GameObject.Find("Song");
        _song = songObject.GetComponent<AudioSource>();
        _song.clip = UnityEngine.Resources.Load<AudioClip>(_levelMeta.songLink);
        
        if (_layers == null || _roundTargets == null || _song == null)
        {
            return false;
        }

        return true;
    }
}
