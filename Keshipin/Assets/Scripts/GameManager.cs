using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState {GAMEPLAY,GAMEOVER,GAMECLEAR};
    public static GameState gameState;

    public enum TrunState {PLAYERTURN,ENEMYTURN}
    public static TrunState turnState;

    private GameObject player;
    private int nowEnemyCount;

    private GameObject[] enemyList;
    private int enemyStopNumber;
    private int enemyAttackNumber;
    [SerializeField]
    private float enemyAttackTime = 1;
    private float enemyAttackTimer;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private int maxTurn = 10;

    public static int turnNumber;

    public static bool enemyMove;

    public static bool pose;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.GAMEPLAY;
        turnState = TrunState.PLAYERTURN;

        player = GameObject.FindGameObjectWithTag("Player");
        nowEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        enemyStopNumber = 0;
        turnNumber = 1;

        enemyMove = false;

        pose = false;
        //SoundManager.PlayBGM(0,0.5f);

        enemyAttackNumber = 0;
        enemyAttackTimer = 1;
    }

    // Update is called once per frame
    void Update()
    {

        nowEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");

        if (gameState == GameState.GAMEPLAY)
        {
            if (nowEnemyCount <= 0 && turnNumber >= maxTurn)
            {
                gameState = GameState.GAMECLEAR;
                if (Input.anyKeyDown)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
            if (player.transform.position.y <= -10)
            {
                gameState = GameState.GAMEOVER;
                if (Input.anyKeyDown)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

        enemyStopNumber = 0;
        for (int i = 0; i <= enemyList.Length - 1; i++)
        {
            if (enemyList[i].GetComponent<Keshipin_Enemy>().ReturnSpeed() <= 0.1f)
            {
                enemyStopNumber++;
            }
        }
        if (enemyStopNumber >= enemyList.Length)
        {
            enemyMove = false;
        }
        else
        {
            enemyMove = true;
        }

        switch (turnState)
        {
            case TrunState.ENEMYTURN:
                enemyStopNumber = 0;

                for (int i = 0; i <= enemyList.Length - 1; i++)
                {
                    if (enemyList[i].GetComponent<Keshipin_Enemy>().StopEnemy())
                    {
                        enemyStopNumber++;
                    }
                }

                enemyAttackTimer += Time.deltaTime;
                if(enemyAttackTimer >= enemyAttackTime && enemyAttackNumber < enemyList.Length)
                {
                    enemyList[enemyAttackNumber].GetComponent<Keshipin_Enemy>().Attack(player);
                    enemyAttackNumber++;
                    enemyAttackTimer = 0;
                }

                //Debug.Log("敵カウント: " + enemyList.Length + " / 停止カウント: " + enemyStopNumber + " / 攻撃カウント: " + enemyAttackNumber);
                if (enemyStopNumber >= enemyList.Length && enemyAttackNumber >= enemyList.Length)
                {
                    turnState = TrunState.PLAYERTURN;
                    for (int i = 0; i <= enemyList.Length - 1; i++)
                    {
                        enemyList[i].GetComponent<Keshipin_Enemy>().Setting();
                    }
                    turnNumber++;
                    if ((turnNumber % 3 == 0 || turnNumber.ToString().Contains("3") || enemyList.Length == 0)&&turnNumber<maxTurn)
                    {
                        Instantiate(enemy, new Vector3(0, 3.5f, 0), Quaternion.identity);
                        Instantiate(enemy, new Vector3(2, 3.5f, 0), Quaternion.identity);
                        Instantiate(enemy, new Vector3(-2, 3.5f, 0), Quaternion.identity);
                    }
                    enemyAttackNumber = 0;
                    enemyAttackTimer = enemyAttackTime;
                }
                enemyStopNumber = 0;

                
                break;
        }
        enemyList.Initialize();


        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetButton("StartButton") && Input.GetButton("SelectButton")))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetButtonDown("StartButton"))
        {
            pose = !pose;
        }

        if (pose)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
