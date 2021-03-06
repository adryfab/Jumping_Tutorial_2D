﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Idle=Parado, Playing=Jugando, Ended=Terminando, Ready=Perparado
public enum GameState { Idle, Playing, Ended, Ready }; 

public class GameControl : MonoBehaviour {

    [Range(0f, 0.20f)]
    public float parallaxSpeed = 0.02f;
    public RawImage background;
    public RawImage platform;
    public GameObject uiIdle;
    public GameObject uiScore;
    public Text pointText;
    public Text recordText;

    public GameState gameState = GameState.Idle;

    public GameObject player;
    public GameObject enemyGenerator;

    public float scaleTime = 6f;
    public float scaleInc = 0.25f;

    private AudioSource musicPlayer;
    private int points = 0;

	// Use this for initialization
	void Start () {
        musicPlayer = GetComponent<AudioSource>();
        recordText.text = "BEST: " + GetMaxScore().ToString();
    }
	
	// Update is called once per frame
	void Update () {
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0) || Input.touchCount > 0;

        //Empieza el juego
        if (gameState == GameState.Idle && userAction == true)
        {
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            uiScore.SetActive(true);
            player.SendMessage("UpdateState", "Player_Run");
            player.SendMessage("DustPlay");
            enemyGenerator.SendMessage("StartGenerator");
            musicPlayer.Play();
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);
        }
        //Juego en marcha
        else if (gameState == GameState.Playing)
        {
            Parallax();
        }
        //Juego preparado para reiniciarse
        else if (gameState == GameState.Ready)
        {
            if (userAction == true)
            {
                RestarGame();
            }
        }
    }

    void Parallax ()
    {
        float finalSpeed = parallaxSpeed * Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 4, 0f, 1f, 1f);
    }

    public void RestarGame()
    {
        ResetTimeScale();
        SceneManager.LoadScene("Escena1");
    }

    void GameTimeScale()
    {
        Time.timeScale += scaleInc;
        Debug.Log("Ritmo incrementado: " + Time.timeScale.ToString());
    }

    public void ResetTimeScale(float newTimeScale = 1f)
    {
        CancelInvoke("GameTimeScale");
        Time.timeScale = newTimeScale;
        Debug.Log("Ritmo restablecido: " + Time.timeScale.ToString());
    }

    public void IncreasePoints()
    {
        points++;
        pointText.text = points.ToString();
        if (points >= GetMaxScore())
        {
            recordText.text = "BEST: " + points.ToString();
            SaveScore(points);
        }
    }

    public int GetMaxScore()
    {
        return PlayerPrefs.GetInt("Max_Points", 0);
    }

    public void SaveScore(int currentsPoints)
    {
        PlayerPrefs.SetInt("Max_Points", currentsPoints);
    }
}
