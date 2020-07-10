using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private Camera subCamera;

    private bool move;

    private bool hitGround;

    private List<GameObject> items;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        beforeFrameVector = Vector3.zero;
        nowFrameVector = Vector3.zero;
        impulseVector = Vector3.zero;

        mainCamera.enabled = true;
        subCamera.enabled = false;

        move = false;

        hitGround = true;

        items = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraChange();
        ItemMove();

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

        if (rigid.velocity.magnitude == 0 && move)
        {
            move = false;
        }

        if (beforeFrameVector != Vector3.zero && nowFrameVector == Vector3.zero && !move)
        {
            rigid.AddForce(-beforeFrameVector * impulsePower, ForceMode.Impulse);
            move = true;
            impulseVector = beforeFrameVector;
        }

        beforeFrameVector = nowFrameVector;
        
    }

    void CameraChange()
    {
        //if(rigid.velocity.magnitude != 0)
        //{
        //    mainCamera.enabled = false;
        //    subCamera.enabled = true;
        //    subCamera.transform.position = transform.position + -rigid.velocity * 2;
        //    subCamera.transform.LookAt(transform.position + rigid.velocity * 2);
        //}
        //else
        //{
        //    mainCamera.enabled = true;
        //    subCamera.enabled = false;
        //}

        if (move)
        {
            mainCamera.enabled = false;
            subCamera.enabled = true;
            if (hitGround)
            {
                subCamera.transform.position = transform.position + new Vector3(0, 2, 0) + impulseVector * 5;
                subCamera.transform.LookAt(transform.position + -impulseVector * 5);
            }
        }
        else
        {
            mainCamera.enabled = true;
            subCamera.enabled = false;
        }
    }

    void ItemMove()
    {
        for(int i = 0; i < items.Count; i++)
        {
            items[i].transform.position = transform.position + new Vector3(i%3 -1, 1 + Mathf.Round(i/3), 0);
        }
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
            other.transform.GetComponent<Collider>().enabled = false;
        }
    }
}
