using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject target;
    public Texture2D[] arr;
    // public Text Score;
    public const float Max_Life = 100;
    private const int decrease_score = 10;
    public static LevelController lc;
    public float score;
    private float time;
    private GameObject Background;
    private int layer_number = 1;
    public Slider score_slider;
    public Slider hp_slider;
    private float life_points;
    public float bpm;
    public static float scene_Height = 10.0f;
    public static float scene_Width = 5.6f;
    public float stake_life_decrease = 0.2f;
    public const float stake_life_increase = 0.01f;
    public bool super_touch = false;
    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
        if (lc == null)
            lc = this.gameObject.GetComponent<LevelController>();
        Background = GameObject.Find("Background");
        int x = GameObject.Find("Background").transform.childCount;
        // int x = Background.transform.childCount;
        this.life_points = Max_Life;
        hp_slider.value = this.life_points;
    }
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
        this.score += score;
        /*f (Score != null)
        {
            Score.text = this.score.ToString();
        }*/
        layer_number++;
        //string new_layer = name_template + layer_number.ToString() + format;
        Texture2D tex = arr[layer_number];
        Debug.Log(layer_number);
        Background = GameObject.Find("Background");
        int x = GameObject.Find("Background").transform.childCount;
        GameObject BackImage = GameObject.Find("BackImage");
        Transform X = Background.transform;
        x = X.childCount;
        BackImage.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        score_slider.value = this.score;

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
