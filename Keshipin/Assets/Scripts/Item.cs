using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   protected Camera playerCamera;
    private GameObject player;
    public bool isAttack;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        
            //transform.rotation = playerCamera.transform.rotation;
            //transform.position = player.transform.position + new Vector3(0, 1, 0);
    }

    public void SkillStart()
    {
        if (!isAttack)
        {
            Attack();
            isAttack = true;
        }
    }

    protected virtual void Attack()
    {

    }
}
