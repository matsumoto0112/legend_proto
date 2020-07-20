using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bakuhatsu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 20;
        if (transform.localScale.x >= 15)
        {
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy" || collision.transform.tag == "Player")
        {
            Vector3 attackVector = (collision.transform.position - transform.position).normalized;
            attackVector -= new Vector3(0, attackVector.y, 0);
            collision.transform.GetComponent<Rigidbody>().AddForce((attackVector * 10) + new Vector3(0, 3, 0) * 3, ForceMode.Impulse);
            //Destroy(gameObject);
        }
    }
}
