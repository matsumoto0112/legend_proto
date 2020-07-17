using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text clearOverText;
    [SerializeField]
    private Text turnStateText;

    // Start is called before the first frame update
    void Start()
    {
        clearOverText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameState == GameManager.GameState.GAMECLEAR)
        {
            clearOverText.text = "げーむくりあ";
        }
        if (GameManager.gameState == GameManager.GameState.GAMEOVER)
        {
            clearOverText.text = "げーむおーばー";
        }

        turnStateText.text = GameManager.turnState.ToString();
    }
}
