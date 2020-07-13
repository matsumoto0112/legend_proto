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

    [SerializeField]
    private Text speedUI;

    [SerializeField]
    private GameObject moveDirectionObject;

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

        stickVector = new Queue<Vector3>();
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
        nowFrameVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (move)
        {
            moveTime += Time.deltaTime;
            playerCamera.fieldOfView = 60 + (rigid.velocity.magnitude * 2);

            if(moveTime >= 1 && rigid.velocity.magnitude == 0)
            {
                move = false;
                mainCamera.enabled = false;
                playerCamera.enabled =true;
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
            rigid.AddForce(playerCamera.transform.rotation *  -maxVector * impulsePower, ForceMode.Impulse);
            move = true;
            impulseVector = beforeFrameVector;
        }

        beforeFrameVector = nowFrameVector;

        stickVector.Enqueue(nowFrameVector);

        while (stickVector.Count > stickVectorMax)
        {
            stickVector.Dequeue();
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
            if (Input.GetButtonDown("Fire1") && !move)
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
        moveDirectionObject.transform.position = transform.position + playerCamera.transform.rotation * -nowFrameVector.normalized * 10;
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
