using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    private Animator animator;

    public GameObject game;
    public GameObject enemyGenerator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        bool gamePlaying = game.GetComponent<GameControl>().gameState == GameState.Playing;

        if (gamePlaying == true && (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            UpdateState("Player_Jump");
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
        }
    }

    void GameReady ()
    {
        game.GetComponent<GameControl>().gameState = GameState.Ready;
    }
}
