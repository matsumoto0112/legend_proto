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
    [SerializeField]
    private Text turnNumberUI;
    [SerializeField]
    private Text poseUI;
    [SerializeField]
    private Image posePanel;
    [SerializeField]
    private Text protoText;
    [SerializeField]
    private Text keshikasuUI;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        clearOverText.text = "";
        player = GameObject.FindGameObjectWithTag("Player");
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

        turnNumberUI.text = GameManager.turnNumber + "ターン目";

        if (player.GetComponent<Keshipin_Move>().ReturnSkillWait())
        {
            protoText.text = "次の操作方法に切り替え:Xボタン\n左スティック:使いたいスキルを指定\n右スティック: 視点の回転\nLボタン:スキル待機状態切り替え\nAボタン:視点切り替え\nBボタン:スキルを発動\nStartボタン: ポーズ\nStart + Select:ゲームを終了\nRボタン:リトライ";
        }
        else
        {
            switch (player.GetComponent<Keshipin_Move>().ReturnMoveType())
            {
                case Keshipin_Move.MoveType.MOVETYPE_1:
                    protoText.text = "次の操作方法に切り替え:Xボタン\n左スティックを引く:飛ばす方向を指定\n左スティックを離す:はじく\n右スティック: 視点の回転\nLボタン:スキル待機状態切り替え\nAボタン:視点切り替え\nStartボタン: ポーズ\nStart + Select:ゲームを終了\nRボタン:リトライ";
                    break;
                case Keshipin_Move.MoveType.MOVETYPE_2:
                    protoText.text = "次の操作方法に切り替え:Xボタン\n左スティック:飛ばす方向を指定\n右スティック: 視点の回転\nLボタン:スキル待機状態切り替え\nAボタン:視点切り替え\nBボタン(長押し):威力調整\nBボタン(離す):はじく\nStartボタン: ポーズ\nStart + Select:ゲームを終了\nRボタン:リトライ";
                    break;
                case Keshipin_Move.MoveType.MOVETYPE_3:
                    protoText.text = "次の操作方法に切り替え:Xボタン\n左スティック:飛ばす方向を指定\n右スティック: 視点の回転\nLボタン:スキル待機状態切り替え\nAボタン:視点切り替え\nBボタン(長押し):威力調整\nBボタン(離す):はじく\nStartボタン: ポーズ\nStart + Select:ゲームを終了\nRボタン:リトライ";
                    break;
            }
            
        }

        keshikasuUI.text = "所持ケシカス数: " + player.GetComponent<Keshipin_Move>().ReturnKeshikasuNumber();

        if(GameManager.gameState == GameManager.GameState.GAMEPLAY)
        {
            if (GameManager.pose)
            {
                poseUI.text = "ぽーず中";
                posePanel.enabled = true;
            }
            else
            {
                poseUI.text = "";
                posePanel.enabled = false;
            }
        }
        
    }
}
