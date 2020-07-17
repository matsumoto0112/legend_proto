using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shin : MonoBehaviour
{
    private Rigidbody rigid;
    private Collider collider;

    [SerializeField]
    private GameObject bakuhatsu;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;
        collider = GetComponent<Collider>();
        collider.isTrigger = true;

        //rigid.AddForce(transform.parent.rotation * new Vector3(0, 0, 15), ForceMode.Impulse);
        //transform.parent = null;
        //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!rigid.isKinematic)
        //{
        //    transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        //    if (transform.localScale.x >= 3f)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Stage")
        {
            Instantiate(bakuhatsu, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
