using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public const float LifeTime = 2.0f;
    private float time;
    public int maxScore = 100;
    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click on object");
            UpdateScore();
            Destroy(this.gameObject);
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
}
