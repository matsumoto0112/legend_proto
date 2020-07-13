using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Keshipin_Move : MonoBehaviour
{
    private Rigidbody rigid;

    private Vector3 beforeFrameVector;
    private Vector3 nowFrameVector;
    private Vector3 impulseVector;

    [SerializeField]
    private float impulsePower = 10;

    private float typeB_impulsePower;
    [SerializeField]
    private Slider movePowerSlider;
    [SerializeField]
    private Text moveTypeText;
    private bool stopMove;

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private Camera dramaticCamera;

    private bool move;
    private float moveTime;

    private bool hitGround;

    private List<GameObject> items;

    private Queue<Vector3> stickVector;
    [SerializeField]
    private int stickVectorMax = 30;
    private bool minus;

    [SerializeField]
    private Text speedUI;

    [SerializeField]
    private GameObject moveDirectionObject;

    enum MoveType {MOVETYPE_A,MOVETYPE_B,MOVETYPE_B_A}
    [SerializeField]
    private MoveType moveType = MoveType.MOVETYPE_A;

    

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        beforeFrameVector = Vector3.zero;
        nowFrameVector = Vector3.zero;
        impulseVector = Vector3.zero;

        mainCamera.enabled = false;
        playerCamera.enabled = true;
        dramaticCamera.enabled = false;

        move = false;

        hitGround = true;

        items = new List<GameObject>();
        moveTime = 0;

        typeB_impulsePower = 0;
        movePowerSlider.gameObject.SetActive(false);
        minus = false;
        stopMove = false;

        stickVector = new Queue<Vector3>();

        moveTypeText.text = moveType.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraChange();
        UI();
        CameraRotate();
        MoveDirectionObject();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    void Move()
    {
        
        switch (moveType)
        {
            case MoveType.MOVETYPE_A:
                nowFrameVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

                if (move)
                {
                    moveTime += Time.deltaTime;
                    playerCamera.fieldOfView = 60 + (rigid.velocity.magnitude * 2);

                    if (moveTime >= 1 && rigid.velocity.magnitude == 0)
                    {
                        move = false;
                        mainCamera.enabled = false;
                        playerCamera.enabled = true;
                        dramaticCamera.enabled = false;
                        moveTime = 0;
                    }
                }

                if (beforeFrameVector != Vector3.zero && nowFrameVector == Vector3.zero && !move && playerCamera.enabled)
                {
                    Vector3 maxVector = Vector3.zero;
                    while (stickVector.Count > 0)
                    {
                        if (maxVector.magnitude < stickVector.Peek().magnitude)
                        {
                            maxVector = stickVector.Dequeue();
                            continue;
                        }
                        stickVector.Dequeue();
                    }
                    rigid.AddForce(playerCamera.transform.rotation * -maxVector * impulsePower, ForceMode.Impulse);
                    move = true;
                    impulseVector = beforeFrameVector;
                }

                beforeFrameVector = nowFrameVector;

                stickVector.Enqueue(nowFrameVector);

                while (stickVector.Count > stickVectorMax)
                {
                    stickVector.Dequeue();
                }
                break;

            case MoveType.MOVETYPE_B:
                nowFrameVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

                if (move)
                {
                    moveTime += Time.deltaTime;
                    playerCamera.fieldOfView = 60 + (rigid.velocity.magnitude * 2);

                    if (moveTime >= 1 && rigid.velocity.magnitude == 0)
                    {
                        move = false;
                        mainCamera.enabled = false;
                        playerCamera.enabled = true;
                        dramaticCamera.enabled = false;
                        moveTime = 0;
                        typeB_impulsePower = 0;
                        minus = false;
                    }
                }

                if (Input.GetButton("BButton"))
                {
                    movePowerSlider.gameObject.SetActive(true);
                    if (!minus)
                    {
                        typeB_impulsePower += Time.deltaTime;
                    }
                    else
                    {
                        typeB_impulsePower -= Time.deltaTime;
                    }
                    if(typeB_impulsePower >= 1)
                    {
                        minus = true;
                    }
                    else if(typeB_impulsePower <= 0)
                    {
                        minus = false;
                    }
                    movePowerSlider.value = typeB_impulsePower;
                }
                if (Input.GetButtonUp("BButton"))
                {
                    movePowerSlider.gameObject.SetActive(false);
                    if (beforeFrameVector != Vector3.zero && !move && playerCamera.enabled)
                    {
                        Vector3 maxVector = Vector3.zero;
                        while (stickVector.Count > 0)
                        {
                            if (maxVector.magnitude < stickVector.Peek().magnitude)
                            {
                                maxVector = stickVector.Dequeue();
                                continue;
                            }
                            stickVector.Dequeue();
                        }
                        rigid.AddForce(playerCamera.transform.rotation * maxVector * (typeB_impulsePower * impulsePower), ForceMode.Impulse);
                        move = true;
                        impulseVector = beforeFrameVector;
                    }
                    else
                    {
                        typeB_impulsePower = 0;
                        minus = false;
                    }
                }

                beforeFrameVector = nowFrameVector;

                stickVector.Enqueue(nowFrameVector);

                while (stickVector.Count > stickVectorMax)
                {
                    stickVector.Dequeue();
                }
                break;

            case MoveType.MOVETYPE_B_A:
                if (!stopMove)
                {
                    nowFrameVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                }

                if (move)
                {
                    moveTime += Time.deltaTime;
                    playerCamera.fieldOfView = 60 + (rigid.velocity.magnitude * 2);

                    if (moveTime >= 1 && rigid.velocity.magnitude == 0)
                    {
                        move = false;
                        mainCamera.enabled = false;
                        playerCamera.enabled = true;
                        dramaticCamera.enabled = false;
                        moveTime = 0;
                        typeB_impulsePower = 0;
                        minus = false;
                    }
                }

                if (Input.GetButton("BButton"))
                {
                    stopMove = true;
                    movePowerSlider.gameObject.SetActive(true);
                    if (!minus)
                    {
                        typeB_impulsePower += Time.deltaTime;
                    }
                    else
                    {
                        typeB_impulsePower -= Time.deltaTime;
                    }
                    if (typeB_impulsePower >= 1)
                    {
                        minus = true;
                    }
                    else if (typeB_impulsePower <= 0)
                    {
                        minus = false;
                    }
                    movePowerSlider.value = typeB_impulsePower;
                }
                
                
                if (Input.GetButtonUp("BButton"))
                {
                    stopMove = false;
                    movePowerSlider.gameObject.SetActive(false);
                    if (beforeFrameVector != Vector3.zero && !move && playerCamera.enabled)
                    {
                        Vector3 maxVector = Vector3.zero;
                        while (stickVector.Count > 0)
                        {
                            if (maxVector.magnitude < stickVector.Peek().magnitude)
                            {
                                maxVector = stickVector.Dequeue();
                                continue;
                            }
                            stickVector.Dequeue();
                        }
                        rigid.AddForce(playerCamera.transform.rotation * maxVector * (typeB_impulsePower * impulsePower), ForceMode.Impulse);
                        move = true;
                        impulseVector = beforeFrameVector;
                    }
                    else
                    {
                        typeB_impulsePower = 0;
                        minus = false;
                    }
                }

                if (!stopMove)
                {
                    {
                        beforeFrameVector = nowFrameVector;

                        stickVector.Enqueue(nowFrameVector);

                        while (stickVector.Count > stickVectorMax)
                        {
                            stickVector.Dequeue();
                        }
                    }
                }
                break;
        }
    }

    void CameraChange()
    {
        if (move)
        {
            mainCamera.enabled = false;
            playerCamera.enabled = false;
            dramaticCamera.enabled = true;
            //if (hitGround)
            //{
            //    playerCamera.transform.position = transform.position + new Vector3(0, 2, 0) + impulseVector * 5;
            //}
        }
        else
        {
            if (Input.GetButtonDown("AButton") && !move)
            {
                mainCamera.enabled = !mainCamera.enabled;
                playerCamera.enabled = !playerCamera.enabled;
            }
        }
    }

    void CameraRotate()
    {
        if (playerCamera.enabled)
        {
            playerCamera.transform.RotateAround(transform.position,Vector3.up, Input.GetAxisRaw("Horizontal_R"));
        }
    }

    void MoveDirectionObject()
    {
        switch (moveType)
        {
            case MoveType.MOVETYPE_A:
                moveDirectionObject.transform.position = transform.position + playerCamera.transform.rotation * -nowFrameVector.normalized * 10;
                break;
            case MoveType.MOVETYPE_B:
                moveDirectionObject.transform.position = transform.position + playerCamera.transform.rotation * nowFrameVector.normalized * 10;
                break;
            case MoveType.MOVETYPE_B_A:
                moveDirectionObject.transform.position = transform.position + playerCamera.transform.rotation * nowFrameVector.normalized * 10;
                break;
        }
    }

    void UI()
    {
        speedUI.text = "速度:" + Mathf.Round(rigid.velocity.magnitude) + "km";
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.tag == "Stage")
        {
            hitGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Stage")
        {
            hitGround = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            items.Add(other.transform.gameObject);
            other.transform.parent = transform;
            other.transform.GetComponent<Collider>().enabled = false;
            other.transform.position = transform.position + new Vector3((items.Count - 1) % 3 - 1, 1 + Mathf.Round((items.Count-1) / 3), 0);
        }
    }
}
