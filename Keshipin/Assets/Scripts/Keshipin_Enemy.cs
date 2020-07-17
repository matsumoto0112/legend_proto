using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keshipin_Enemy : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField]
    private float impulsePower = 10;

    [SerializeField]
    private float beAttackedImpulsePower = 10;

    private bool isAttack;
    private float attackTimer;

    [SerializeField]
    private GameObject keshikasu;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        isAttack = false;
        attackTimer = 0;
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

    public void Attack(GameObject player)
    {
        if (!isAttack)
        {
            Vector3 attackVector = (player.transform.position - transform.position).normalized;

            rigid.AddForce(attackVector * impulsePower, ForceMode.Impulse);
            isAttack = true;
        }
        else
        {
            attackTimer += Time.deltaTime;
        }
    }

    public bool StopEnemy()
    {
        if(isAttack && attackTimer >= 2 && rigid.velocity.magnitude <= 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Setting()
    {
        isAttack = false;
        attackTimer = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.turnState == GameManager.TrunState.PLAYERTURN)
        {
            if (other.transform.tag == "Player")
            {
                Vector3 attackVector = (transform.position - other.transform.position).normalized;
                attackVector -= new Vector3(0, attackVector.y, 0);
                //rigid.AddForce((attackVector * beAttackedImpulsePower) + new Vector3(0, 10, 0), ForceMode.Impulse);
                rigid.AddForce((attackVector * other.transform.GetComponent<Rigidbody>().velocity.magnitude) + new Vector3(0, other.transform.localScale.y -1, 0) * other.transform.GetComponent<Rigidbody>().velocity.magnitude, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (GameManager.turnState == GameManager.TrunState.PLAYERTURN)
        {
            if(collision.transform.tag == "Stage")
            {
                if(rigid.velocity.magnitude >= 1 && Random.Range(0,100) <= 5)
                {
                    Instantiate(keshikasu, transform.position - new Vector3(0, transform.position.y, 0), Quaternion.identity);
                    Debug.Log("HAIUWNCAWDWA");
                }
            }
        }
    }
}
