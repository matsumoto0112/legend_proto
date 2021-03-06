﻿using System.Collections;
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

    private float moveImpulsePower;
    [SerializeField]
    private Slider movePowerSlider;
    [SerializeField]
    private Text moveTypeText;
    private int moveTypeNumber;
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

    //private List<GameObject> items;

    private Queue<Vector3> stickVector;
    [SerializeField]
    private int stickVectorMax = 30;
    private bool minus;

    [SerializeField]
    private Text speedUI;

    [SerializeField]
    private GameObject moveDirectionObject;

    public enum MoveType {MOVETYPE_1,MOVETYPE_2,MOVETYPE_3,MOVETYPE_4}
    [SerializeField]
    private MoveType moveType = MoveType.MOVETYPE_1;

    private MeshRenderer meshRenderer;

    [SerializeField]
    private Collider triggerCollider;

    private Vector3 firstSize;
    [SerializeField]
    private float keshikasuNumber = 0;

    private bool skillWait;
    private Item itemUp,itemLeft,itemRight;
    enum SkillState {ITEM_UP,ITEM_RIGHT,ITEM_LEFT,NO_ITEM};
    private SkillState skillState;
    [SerializeField]
    private Text skillUI;


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

        //items = new List<GameObject>();
        moveTime = 0;

        moveImpulsePower = 0;
        movePowerSlider.gameObject.SetActive(false);
        minus = false;
        stopMove = false;

        stickVector = new Queue<Vector3>();

        moveTypeText.text = moveType.ToString();

        meshRenderer = GetComponent<MeshRenderer>();

        firstSize = transform.localScale;
        //keshikasuNumber = 0;

        skillWait = false;
        skillState = SkillState.NO_ITEM;

        moveTypeNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        moveTypeText.text = moveType.ToString();
        if (GameManager.turnState == GameManager.TrunState.PLAYERTURN)
        {
            if (!skillWait)
            {
                Move();
                MoveDirectionObject();
            }
            if (!move)
            {
                Skill();
            }
            CameraChange();
            UI();
        }
        
        MoveTypeChange();
        ItemRotation();
        CameraRotate();
    }


    void Move()
    {
        
        switch (moveType)
        {
            case MoveType.MOVETYPE_1:
                nowFrameVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                if (move)
                {
                    moveTime += Time.deltaTime;
                    movePowerSlider.gameObject.SetActive(false);
                    playerCamera.fieldOfView = 60 + (rigid.velocity.magnitude * 2);

                    if (moveTime >= 1 && rigid.velocity.magnitude == 0 && !GameManager.enemyMove)
                    {
                        move = false;
                        mainCamera.enabled = false;
                        playerCamera.enabled = true;
                        dramaticCamera.enabled = false;
                        moveTime = 0;
                        GameManager.turnState = GameManager.TrunState.ENEMYTURN;
                        triggerCollider.enabled = false;
                    }
                }
                else if(!move && GameManager.turnState == GameManager.TrunState.PLAYERTURN)
                {
                    movePowerSlider.gameObject.SetActive(true);
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
                    if (moveImpulsePower >= 0.8f)
                    {
                        rigid.AddForce(playerCamera.transform.rotation * new Vector3(Random.Range(-10, 10), 0, 0), ForceMode.Impulse);
                    }
                    move = true;
                    impulseVector = beforeFrameVector;
                    triggerCollider.enabled = true;
                    SoundManager.PlaySE(0);
                }

                moveImpulsePower = nowFrameVector.magnitude;
                movePowerSlider.value = moveImpulsePower;

                beforeFrameVector = nowFrameVector;

                stickVector.Enqueue(nowFrameVector);

                while (stickVector.Count > stickVectorMax)
                {
                    stickVector.Dequeue();
                }
                break;

            case MoveType.MOVETYPE_2:
                nowFrameVector = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));

                if (move)
                {
                    moveTime += Time.deltaTime;
                    playerCamera.fieldOfView = 60 + (rigid.velocity.magnitude * 2);

                    if (moveTime >= 1 && rigid.velocity.magnitude == 0 && !GameManager.enemyMove)
                    {
                        move = false;
                        mainCamera.enabled = false;
                        playerCamera.enabled = true;
                        dramaticCamera.enabled = false;
                        moveTime = 0;
                        moveImpulsePower = 0;
                        minus = false;
                        GameManager.turnState = GameManager.TrunState.ENEMYTURN;
                        triggerCollider.enabled = false;
                    }
                }

                if (Input.GetButton("BButton"))
                {
                    movePowerSlider.gameObject.SetActive(true);
                    if (!minus)
                    {
                        moveImpulsePower += Time.deltaTime;
                    }
                    else
                    {
                        moveImpulsePower -= Time.deltaTime;
                    }
                    if(moveImpulsePower >= 1)
                    {
                        minus = true;
                    }
                    else if(moveImpulsePower <= 0)
                    {
                        minus = false;
                    }
                    movePowerSlider.value = moveImpulsePower;
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
                        rigid.AddForce(playerCamera.transform.rotation * maxVector * (impulsePower * moveImpulsePower), ForceMode.Impulse);
                        if (moveImpulsePower >= 0.8f)
                        {
                            rigid.AddForce(playerCamera.transform.rotation * new Vector3(Random.Range(-10, 10), 0, 0), ForceMode.Impulse);
                        }
                        move = true;
                        impulseVector = beforeFrameVector;
                        triggerCollider.enabled = true;
                        SoundManager.PlaySE(0);
                    }
                    else
                    {
                        moveImpulsePower = 0;
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

            case MoveType.MOVETYPE_3:
                if (!stopMove)
                {
                    nowFrameVector = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
                }

                if (move)
                {
                    moveTime += Time.deltaTime;
                    playerCamera.fieldOfView = 60 + (rigid.velocity.magnitude * 2);

                    if (moveTime >= 1 && rigid.velocity.magnitude == 0 && !GameManager.enemyMove)
                    {
                        move = false;
                        mainCamera.enabled = false;
                        playerCamera.enabled = true;
                        dramaticCamera.enabled = false;
                        moveTime = 0;
                        moveImpulsePower = 0;
                        minus = false;
                        GameManager.turnState = GameManager.TrunState.ENEMYTURN;
                        triggerCollider.enabled = false;
                    }
                }

                if (Input.GetButton("BButton"))
                {
                    stopMove = true;
                    movePowerSlider.gameObject.SetActive(true);
                    if (!minus)
                    {
                        moveImpulsePower += Time.deltaTime;
                    }
                    else
                    {
                        moveImpulsePower -= Time.deltaTime;
                    }
                    if (moveImpulsePower >= 1)
                    {
                        minus = true;
                    }
                    else if (moveImpulsePower <= 0)
                    {
                        minus = false;
                    }
                    movePowerSlider.value = moveImpulsePower;
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
                        rigid.AddForce(playerCamera.transform.rotation * maxVector * (impulsePower * moveImpulsePower), ForceMode.Impulse);
                        if(moveImpulsePower >= 0.8f)
                        {
                            rigid.AddForce(playerCamera.transform.rotation * new Vector3(Random.Range(-10, 10), 0, 0), ForceMode.Impulse);
                        }
                        move = true;
                        impulseVector = beforeFrameVector;
                        triggerCollider.enabled = true;
                        SoundManager.PlaySE(0);
                    }
                    else
                    {
                        moveImpulsePower = 0;
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
            case MoveType.MOVETYPE_4:
                
                    nowFrameVector = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
                

                if (move)
                {
                    moveTime += Time.deltaTime;
                    playerCamera.fieldOfView = 60 + (rigid.velocity.magnitude * 2);

                    if (moveTime >= 1 && rigid.velocity.magnitude == 0 && !GameManager.enemyMove)
                    {
                        move = false;
                        mainCamera.enabled = false;
                        playerCamera.enabled = true;
                        dramaticCamera.enabled = false;
                        moveTime = 0;
                        moveImpulsePower = 0;
                        minus = false;
                        GameManager.turnState = GameManager.TrunState.ENEMYTURN;
                        triggerCollider.enabled = false;
                    }
                }

                if (nowFrameVector != Vector3.zero)
                {
                    stopMove = true;
                    movePowerSlider.gameObject.SetActive(true);
                    if (!minus)
                    {
                        moveImpulsePower += Time.deltaTime;
                    }
                    else
                    {
                        moveImpulsePower -= Time.deltaTime;
                    }
                    if (moveImpulsePower >= 1)
                    {
                        minus = true;
                    }
                    else if (moveImpulsePower <= 0)
                    {
                        minus = false;
                    }
                    movePowerSlider.value = moveImpulsePower;
                }


                if (nowFrameVector == Vector3.zero)
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
                        rigid.AddForce(playerCamera.transform.rotation * maxVector * (impulsePower * moveImpulsePower), ForceMode.Impulse);
                        if (moveImpulsePower >= 0.8f)
                        {
                            rigid.AddForce(playerCamera.transform.rotation * new Vector3(Random.Range(-10, 10), 0, 0), ForceMode.Impulse);
                        }
                        move = true;
                        impulseVector = beforeFrameVector;
                        triggerCollider.enabled = true;
                        SoundManager.PlaySE(0);
                    }
                    else
                    {
                        moveImpulsePower = 0;
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
        }
        
        if (move)
        {
            ////if(Input.GetButtonDown("BButton"))
            ////{
            ////    rigid.AddTorque(new Vector3(0, 100, 0), ForceMode.Impulse);
            ////}

            //if(Input.GetButton("AButton") && rigid.velocity.magnitude >= 0.01f)
            //{
            //    meshRenderer.material.color = Color.red;
            //    rigid.velocity -= rigid.velocity / 0.8f * Time.deltaTime;
                
            //}
            //else
            //{
            //    meshRenderer.material.color = Color.white;
            //}
        }
        else
        {
            meshRenderer.material.color = Color.white;
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
            playerCamera.transform.RotateAround(transform.position,Vector3.up, Input.GetAxisRaw("Horizontal_R") * 2);
        }
    }

    void MoveDirectionObject()
    {
        switch (moveType)
        {
            case MoveType.MOVETYPE_1:
                moveDirectionObject.transform.position = transform.position + playerCamera.transform.rotation * -nowFrameVector.normalized * 10;
                break;
            case MoveType.MOVETYPE_2:
                moveDirectionObject.transform.position = transform.position + playerCamera.transform.rotation * nowFrameVector.normalized * 10;
                break;
            case MoveType.MOVETYPE_3:
                moveDirectionObject.transform.position = transform.position + playerCamera.transform.rotation * nowFrameVector.normalized * 10;
                break;
            case MoveType.MOVETYPE_4:
                moveDirectionObject.transform.position = transform.position + playerCamera.transform.rotation * nowFrameVector.normalized * 10;
                break;
        }
    }

    void UI()
    {
        speedUI.text = "速度:" + Mathf.Round(rigid.velocity.magnitude) + "km";
        skillUI.text = skillState.ToString();
    }

    void SizeChange()
    {
        transform.localScale = firstSize * (1 + (keshikasuNumber / 100));
        //transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
    }

    void Skill()
    {
        if (Input.GetButtonDown("LButton")&&!Input.GetButton("BButton"))
        {
            skillWait = !skillWait;
        }

        if (skillWait)
        {
            meshRenderer.material.color = Color.blue;
            if (Input.GetAxisRaw("Vertical") >= 0.9f && itemUp != null)
            {
                skillState = SkillState.ITEM_UP;
            }
            else if (Input.GetAxisRaw("Horizontal") >= 0.9f && itemRight != null)
            {
                skillState = SkillState.ITEM_RIGHT;
            }
            else if(Input.GetAxisRaw("Horizontal") <= -0.9f && itemLeft != null)
            {
                skillState = SkillState.ITEM_LEFT;
            }
            else if(itemUp == null && itemRight == null && itemLeft == null)
            {
                skillState = SkillState.NO_ITEM;
            }

            if (Input.GetButtonDown("BButton"))
            {
                switch (skillState)
                {
                    case SkillState.ITEM_UP:
                        if(!itemUp.isAttack)itemUp.SkillStart();
                        break;
                    case SkillState.ITEM_RIGHT:
                        if (!itemRight.isAttack) itemRight.SkillStart();
                        break;
                    case SkillState.ITEM_LEFT:
                        if (!itemLeft.isAttack) itemLeft.SkillStart();
                        break;
                }
            }
        }
        else
        {
            meshRenderer.material.color = Color.white;
        }
    }

    void MoveTypeChange()
    {
        if (Input.GetButtonDown("XButton"))
        {
            moveTypeNumber++;
            if(moveTypeNumber >= 5)
            {
                moveTypeNumber = 1;
            }
        }

        switch (moveTypeNumber)
        {
            case 1:
                moveType = MoveType.MOVETYPE_1;
                break;
            case 2:
                moveType = MoveType.MOVETYPE_2;
                break;
            case 3:
                moveType = MoveType.MOVETYPE_3;
                break;
            case 4:
                moveType = MoveType.MOVETYPE_4;
                break;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            moveType = MoveType.MOVETYPE_1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            moveType = MoveType.MOVETYPE_2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            moveType = MoveType.MOVETYPE_3;
        }
    }

    void ItemRotation()
    {
        if(itemUp != null)
        {
            itemUp.transform.position = transform.position + transform.rotation * new Vector3(0, 0.75f, 1f);
            itemUp.transform.rotation = transform.rotation;
        }
        if (itemRight != null)
        {
            itemRight.transform.position = transform.position + transform.rotation * new Vector3(1.25f, 0, 0);
            itemRight.transform.rotation = transform.rotation;
        }
        if (itemLeft != null)
        {
            itemLeft.transform.position = transform.position + transform.rotation * new Vector3(-1.25f, 0, 0);
            itemLeft.transform.rotation = transform.rotation;
        }
    }

    public bool ReturnMove()
    {
        return move;
    }

    public Vector3 ReturnVector()
    {
        return rigid.velocity.normalized;
    }

    public float ReturnKeshikasuNumber()
    {
        return keshikasuNumber;
    }

    public void MinusKeshikasuNumber()
    {
        keshikasuNumber--;
    }

    public bool ReturnSkillWait()
    {
        return skillWait;
    }

    public MoveType ReturnMoveType()
    {
        return moveType;
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
            //items.Add(other.transform.gameObject);
            if(itemUp == null)
            {
                itemUp = other.GetComponent<Item>();
                skillState = SkillState.ITEM_UP;
            }
            else if(itemRight == null)
            {
                itemRight = other.GetComponent<Item>();
            }
            else if(itemLeft == null)
            {
                itemLeft = other.GetComponent<Item>();
            }
            other.transform.GetComponent<Collider>().enabled = false;
            //other.transform.position = transform.position + new Vector3((items.Count - 1) % 3 - 1, 1 + Mathf.Round((items.Count-1) / 3), 0);
        }
        if(other.tag == "Keshikasu")
        {
            Destroy(other.gameObject);
            //rigid.mass = transform.localScale.y;
            keshikasuNumber++;
            //SizeChange();
        }
    }
}
