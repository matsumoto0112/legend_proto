using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rakugaki : MonoBehaviour
{
    [SerializeField]
    private GameObject keshikasu;

    [SerializeField]
    private float keshikasuTime = 0.1f;
    private float keshikasuTimer;

    private MeshRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        keshikasuTimer = 0;
        render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<Keshipin_Move>().ReturnMove())
            {


                keshikasuTimer += Time.deltaTime;
                if (keshikasuTimer >= keshikasuTime)
                {
                    if (other.GetComponent<Rigidbody>().velocity.magnitude >= 0.2f)
                    {
                        GameObject keshikasuObj = Instantiate(keshikasu, other.transform.position + -other.GetComponent<Keshipin_Move>().ReturnVector() * 3, Quaternion.identity);
                        keshikasuObj.transform.position -= new Vector3(0, keshikasuObj.transform.position.y, 0);
                        render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, render.material.color.a - 0.01f);
                        other.GetComponent<Keshipin_Move>().MinusKeshikasuNumber();
                        keshikasuTimer = 0;
                    }

                }

                if (render.material.color.a <= 0.01f)
                {
                    Destroy(gameObject);
                }
            }
        }
        if (other.tag == "Enemy")
        {
            keshikasuTimer += Time.deltaTime;
            if (keshikasuTimer >= keshikasuTime)
            {
                if (other.GetComponent<Rigidbody>().velocity.magnitude >= 0.2f)
                {
                    GameObject keshikasuObj = Instantiate(keshikasu, other.transform.position + -other.GetComponent<Rigidbody>().velocity.normalized * 3, Quaternion.identity);
                    keshikasuObj.transform.position -= new Vector3(0, keshikasuObj.transform.position.y, 0);
                    render.material.color = new Color(render.material.color.r, render.material.color.g, render.material.color.b, render.material.color.a - 0.01f);
                    keshikasuTimer = 0;
                }
            }

            if (render.material.color.a <= 0.01f)
            {
                Destroy(gameObject);
            }
        }
    }
}
