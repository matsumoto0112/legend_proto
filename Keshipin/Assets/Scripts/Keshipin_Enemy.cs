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
        if (transform.position.y <= -10 || transform.position.y >= 20)
        {
            Destroy(gameObject);
        }

        if (isAttack)
        {
            attackTimer += Time.deltaTime;
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
            var power = impulsePower;
            var search = FindObjectOfType<SearchAI_Manager>();
            if (search != null)
            {
                var s = search.SearchCourse(gameObject);
                var p = GameObject.FindGameObjectWithTag("Player");
                var vector = (s - transform.position);
                vector.y = 0;
                attackVector = vector.normalized;
                power = ((p == null) || ((p.transform.position - s).magnitude < 0.01f)) ? power : Mathf.Clamp(vector.magnitude, 10, power);
                //Debug.Log(name + ": " + s + " - " + transform.position + " => " + vector);
                //power = (vector.magnitude <= impulsePower) ? vector.magnitude : impulsePower;
            }

            SoundManager.PlaySE(0);
            rigid.AddForce(attackVector * power, ForceMode.Impulse);
            isAttack = true;
        }
        else
        {
            attackTimer += Time.deltaTime;
        }
    }

    public bool StopEnemy()
    {
        if (isAttack && rigid.velocity.magnitude <= 0.1f && attackTimer >= 2f)
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

    public float ReturnSpeed()
    {
        return rigid.velocity.magnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.turnState == GameManager.TrunState.PLAYERTURN)
        {
            if (other.transform.tag == "Player")
            {
                SoundManager.PlaySE(1);
                Vector3 attackVector = (transform.position - other.transform.position).normalized;
                attackVector -= new Vector3(0, attackVector.y, 0);
                //rigid.AddForce((attackVector * beAttackedImpulsePower) + new Vector3(0, 10, 0), ForceMode.Impulse);
                if(other.transform.GetComponent<Keshipin_Move>().ReturnKeshikasuNumber() >= 0)
                {
                    rigid.AddForce((attackVector * other.transform.GetComponent<Rigidbody>().velocity.magnitude * 1.5f) + new Vector3(0, other.transform.GetComponent<Keshipin_Move>().ReturnKeshikasuNumber() * 0.05f, 0) * other.transform.GetComponent<Rigidbody>().velocity.magnitude, ForceMode.Impulse);
                }
                else
                {
                    rigid.AddForce((attackVector * (other.transform.GetComponent<Rigidbody>().velocity.magnitude * 1.5f - (other.transform.GetComponent<Keshipin_Move>().ReturnKeshikasuNumber() * 0.5f * -1))), ForceMode.Impulse);
                }
                
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //if (GameManager.turnState == GameManager.TrunState.PLAYERTURN)
        //{
        //    if (collision.transform.tag == "Stage")
        //    {
        //        if (rigid.velocity.magnitude >= 1 && Random.Range(0, 100) <= 5)
        //        {
        //            GameObject keshikasuObj = Instantiate(keshikasu, transform.position - new Vector3(0, transform.position.y, 0), Quaternion.identity);
        //            keshikasuObj.transform.position -= new Vector3(0, keshikasuObj.transform.position.y, 0);
        //        }
        //    }
        //}
    }
}
