using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Target : MonoBehaviour
{
    public const float LifeTime = 2.0f;
    private float m_time;
    public static int MaxScore = 500;
    public static int MinScore = 50;
    public float step;
    Color _color;
    
    // Start is called before the first frame update
    void Start()
    {
        m_time = Time.time;
    }
    
    // Update is called once per frame
    void Update()
    {
       smoothAppearance();
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                int height = Screen.height;
                int width = Screen.width;
                //Debug.Log("Click on object");
                //Destroy(this.gameObject);
                //UpdateScore();
                Vector2 touch_position = touch.position;
                touch_position.x = touch_position.x / width * LevelController.SceneWidth - LevelController.SceneWidth/2;
                touch_position.y = touch_position.y / height * LevelController.SceneHeight - LevelController.SceneHeight / 2;

                Vector2 this_position = this.transform.position;
                //this_position.x /= width;
                //this_position.y /= height;
                float distance = Vector2.Distance(touch_position, this_position);
                if (distance < 0.8)
                {
                    Debug.Log("Click on object");
                    Destroy(this.gameObject);
                    UpdateScore();
                    LevelController.lc.SuperTouch = true;
                }
                else
                {

                    //LevelController.lc.decreaseLIfe();
                }
            }

        }
        if (Time.time - m_time > LifeTime)
        {
            Debug.Log("Time ended");
            Destroy(this.gameObject);
            LevelController.lc.decreaseLIfe();
        }
    }
    void UpdateScore()
    {
        float clickTime = Time.time - m_time;
        float score = (1 - clickTime / LifeTime) * MaxScore;
        LevelController.lc.updateScore(score);
    }

    public void OnMouseDown()
    {
        if (LevelController.lc.Developer_Mode)
        {
            Debug.Log("Click on object");
            UpdateScore();
            Destroy(this.gameObject);
        }
        
    }
    
     //smooth appearance
    void smoothAppearance(){
        _color = gameObject.GetComponent<Renderer>().material.color;
        if (_color.a < 255){
            _color.a += step/100;
            Debug.Log(_color.a);
           gameObject.GetComponent<Renderer>().material.color = _color;
        }
    }    
}
