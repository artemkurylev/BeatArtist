using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Target : MonoBehaviour
{
    public const float LifeTime = 2.0f;
    private float time;
    private int maxScore = 500;
    public int minScore = 50;
    
    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }
    
    // Update is called once per frame
    void Update()
    {
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
                touch_position.x = touch_position.x / width * LevelController.scene_Width - LevelController.scene_Width/2;
                touch_position.y = touch_position.y / height * LevelController.scene_Height - LevelController.scene_Height / 2;

                Vector2 this_position = this.transform.position;
                //this_position.x /= width;
                //this_position.y /= height;
                float distance = Vector2.Distance(touch_position, this_position);
                if (distance < 0.8)
                {
                    Debug.Log("Click on object");
                    Destroy(this.gameObject);
                    UpdateScore();
                }
            }

        }
        if (Time.time - time > LifeTime)
        {
            Debug.Log("Time ended");
            Destroy(this.gameObject);
        }
    }
    void UpdateScore()
    {
        float clickTime = Time.time - time;
        float score = (1 - clickTime / LifeTime) * maxScore;
        LevelController.lc.updateScore(score);
    }

    //public void OnMouseDown()
    //{
    //    Debug.Log("Click on object");
    //    UpdateScore();
    //    Destroy(this.gameObject);
    //}
}
