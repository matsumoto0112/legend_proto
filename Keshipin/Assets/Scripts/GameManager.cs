using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState {GAMEPLAY,GAMEOVER,GAMECLEAR};
    public static GameState gameState;

    private GameObject player;
    private int nowEnemyCount;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.GAMEPLAY;

        player = GameObject.FindGameObjectWithTag("Player");
        nowEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    // Update is called once per frame
    void Update()
    {
        nowEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (gameState == GameState.GAMEPLAY)
        {
            if (nowEnemyCount <= 0)
            {
                gameState = GameState.GAMECLEAR;
            }
            if (player.transform.position.y <= -10)
            {
                gameState = GameState.GAMEOVER;
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
