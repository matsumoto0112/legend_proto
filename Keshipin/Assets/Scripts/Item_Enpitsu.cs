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
        GameObject bullet = Instantiate(shin,transform.position + transform.rotation * new Vector3(0, 0, 2), Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(transform.rotation * new Vector3(0, 0, 1) * 30, ForceMode.Impulse);
        bullet.GetComponent<Rigidbody>().useGravity = true;
        bullet.GetComponent<Collider>().isTrigger = false;
        Destroy(gameObject);
    }
}
