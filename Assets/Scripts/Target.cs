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
    public void OnMouseDown()
    {
        Debug.Log("Click on object");
        UpdateScore();
        Destroy(this.gameObject);
    }
}
