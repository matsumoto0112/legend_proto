using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text clearOverText;

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
    }
}
