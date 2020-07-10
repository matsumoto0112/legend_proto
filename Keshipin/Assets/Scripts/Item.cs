using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.root.tag == "Player" && Input.GetKey(KeyCode.Space))
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {

    }
}
