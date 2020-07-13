using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keshipin_Enemy : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField]
    private float beAttackedImpulsePower = 10;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.transform.tag == "Player")
    //    {
    //        Vector3 attackVector = (transform.position - collision.transform.position).normalized;
    //        attackVector -= new Vector3(0, attackVector.y, 0);
    //        rigid.AddForce((attackVector * impulsePower) + new Vector3(0, 10, 0), ForceMode.Impulse);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Vector3 attackVector = (transform.position - other.transform.position).normalized;
            attackVector -= new Vector3(0, attackVector.y, 0);
            rigid.AddForce((attackVector * beAttackedImpulsePower) + new Vector3(0, 10, 0), ForceMode.Impulse);
        }
    }
}
