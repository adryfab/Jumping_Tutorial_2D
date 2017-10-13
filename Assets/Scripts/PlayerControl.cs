using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    private Animator animator;
    private AudioSource audioPlayer;
    private float startY;

    public GameObject game;
    public GameObject enemyGenerator;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    public ParticleSystem dust;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y; //Posicion del personaje en el suelo
    }
	
	// Update is called once per frame
	void Update () {
        bool isGrounded = transform.position.y == startY;
        bool gamePlaying = game.GetComponent<GameControl>().gameState == GameState.Playing;
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0) || Input.touchCount > 0;

        if (isGrounded == true && gamePlaying == true && userAction == true)
        {
            UpdateState("Player_Jump");
            audioPlayer.clip = jumpClip;
            audioPlayer.Play();
        }

    }

    public void UpdateState(string state = null)
    {
        if (state != null)
        {
            animator.Play(state);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            UpdateState("Player_Die");
            game.GetComponent<GameControl>().gameState = GameState.Ended;
            enemyGenerator.SendMessage("CancelGenerator", true);
            game.SendMessage("ResetTimeScale", 0.5f);

            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();

            DustStop();
        } else if (collision.gameObject.tag == "Point")
        {
            game.SendMessage("IncreasePoints");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();
        }
    }

    void GameReady ()
    {
        game.GetComponent<GameControl>().gameState = GameState.Ready;
    }

    void DustPlay()
    {
        dust.Play();
    }

    void DustStop()
    {
        dust.Stop();
    }
}
