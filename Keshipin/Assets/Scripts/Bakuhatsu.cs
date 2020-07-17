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
        transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * 3;

        if(transform.localScale.x >= 5)
        {
            Destroy(gameObject);
        }
    }
}
