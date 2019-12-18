﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Target : MonoBehaviour
{
    public const float LifeTime = 2.0f;
    public static int MaxScore = 500;
    public static int MinScore = 50;
    public float step;
    public int currentNumber = -1;
    
    private float creationTime;
    Color _color;

    // Start is called before the first frame update
    void Start()
    {
        creationTime = Time.time;
    }
    
    // Update is called once per frame
    void Update()
    {
        smoothAppearance();

        if (Math.Abs(Time.time - Globals.lastUpdateTime) > Globals.minTimeDifference && currentNumber == Globals.nextClickableTarget)
        {
            // Блок, обрабатывающий тапы на экран
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    int height = Screen.height;
                    int width = Screen.width;
                    
                    Vector2 touch_position = touch.position;
                    touch_position.x = touch_position.x / width * LevelController.SceneWidth - LevelController.SceneWidth/2;
                    touch_position.y = touch_position.y / height * LevelController.SceneHeight - LevelController.SceneHeight / 2;
                    Vector2 this_position = this.transform.position;
                    
                    float distance = Vector2.Distance(touch_position, this_position);
                    if (distance < 0.8)
                    {
                        Hit();
                    } 
                    else 
                    {
                        Miss();
                    }
                    Destroy(this.gameObject);
                    Globals.nextClickableTarget++;
                }
            }
            
            // Блок, обрабатывающий нажатия мыши (для дебага)
            if (Globals.DeveloperMode && Input.GetMouseButtonDown(0))
            {
                Debug.Log("Clicked");
                Vector2 click_position = Input.mousePosition;
                
                int height = Screen.height;
                int width = Screen.width;
                
                click_position.x = click_position.x / width * LevelController.SceneWidth - LevelController.SceneWidth/2;
                click_position.y = click_position.y / height * LevelController.SceneHeight - LevelController.SceneHeight / 2;
                Vector2 this_position = this.transform.position;
                float distance = Vector2.Distance(click_position, this_position);
                
                if (distance < 0.8)
                {
                    Hit();
                } 
                else
                {
                    Miss();
                }
                Destroy(this.gameObject);
                Globals.nextClickableTarget++;
            }
        }
        
        if (Time.time - creationTime > LifeTime)
        {
            Destroy(this.gameObject);
            if (LevelController.lc)
            {
                LevelController.lc.DecreaseLife();
            }
            Globals.nextClickableTarget++;
        }
    }
    void UpdateScore()
    {
        float clickTime = Time.time - creationTime;
        float score = (clickTime / LifeTime) * MaxScore;
        if (LevelController.lc)
        {
            LevelController.lc.UpdateScore(score);
        }
    }

    //smooth appearance
    void smoothAppearance(){
        _color = gameObject.GetComponent<Renderer>().material.color;
        if (_color.a < 4.5f){
            _color.a += step/100;
           gameObject.GetComponent<Renderer>().material.color = _color;
        }
    }

    private void Hit()
    {
        UpdateScore();
        if (LevelController.lc)
        {
            LevelController.lc.IncreaseLife();
        }
        Globals.lastUpdateTime = Time.time;
    }
    
    private void Miss()
    {
        if (LevelController.lc)
        {
            LevelController.lc.DecreaseLife();
        }
        Globals.lastUpdateTime = Time.time;
    }
}
