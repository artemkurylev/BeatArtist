using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public GameObject target;
    public Texture2D[] arr;
    // public Text Score;
    public static LevelController lc;
    public float score;
    private float time;
    private GameObject Background;
    private int layer_number = 1;
    public string name_template = "../Resources/art43/layer";
    public string format = ".png";
    public Slider score_slider;
    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
        if (lc == null)
            lc = this.gameObject.GetComponent<LevelController>();
        Background = GameObject.Find("Background");
    }
    // Update is called once per frame
    void Update()
    {
        float cur_time = Time.time;
        if(cur_time - time > 1)
        {
            time = cur_time;
            Vector3 pos = this.transform.position; 
            Instantiate(target, GeneratePosition(), new Quaternion(0,0,0,0));
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
        string new_layer = name_template + layer_number.ToString() + format;
        Texture2D tex = arr[layer_number];
        Background.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        score_slider.value = this.score;
    }
}
