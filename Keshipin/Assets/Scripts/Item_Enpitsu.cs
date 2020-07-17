using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Enpitsu : Item
{
    [SerializeField]
    private GameObject shin;


    protected override void Attack()
    {
        //Instantiate(shin, transform.rotation * ( transform.position + new Vector3(0, 0, 3f)),Quaternion.identity,transform);
        shin.GetComponent<Rigidbody>().AddForce(transform.rotation * new Vector3(0, 0, 1), ForceMode.Impulse);
        shin.GetComponent<Rigidbody>().useGravity = true;
        shin.GetComponent<Collider>().isTrigger = false;
        shin.transform.parent = null;
        Destroy(gameObject);
    }
}
