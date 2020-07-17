using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   protected Camera playerCamera;
    public bool havePlayer;
    private GameObject player;
    protected bool isAttack;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        havePlayer = false;
        player = GameObject.FindGameObjectWithTag("Player");
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (havePlayer && !isAttack)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Attack();
                isAttack = true;
            }

            transform.rotation = playerCamera.transform.rotation;

            transform.position = player.transform.position + new Vector3(0, 1, 0);
        }
        
    }

    protected virtual void Attack()
    {

    }
}
