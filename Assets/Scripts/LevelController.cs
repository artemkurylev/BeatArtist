using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public GameObject target;
    public Text Score;
    public static LevelController lc;
    public float score;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
        if (lc == null)
            lc = this.gameObject.GetComponent<LevelController>();
    }
    // Update is called once per frame
    void Update()
    {
        float cur_time = Time.time;
        if(cur_time - time > 3)
        {
            time = cur_time;
            Vector3 pos = this.transform.position; 
            Instantiate(target, GeneratePosition(), new Quaternion(0,0,0,0));
        }
    }
    private Vector3 GeneratePosition()
    {
        float x, y, z;
        z = -8;
        x = Random.Range(-5, 5);
        y = Random.Range(-3.5f, 3);
        return new Vector3(x, y, z);
    }
    public void updateScore(float score)
    {
        this.score += score;
        if (Score != null)
        {
            Score.text = this.score.ToString();
        }
    }
}
