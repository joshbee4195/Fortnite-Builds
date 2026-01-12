using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{

    totkPlayer Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<totkPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);


        if (collision.gameObject.tag == "build")
        {

            Destroy(collision.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.gameObject.tag == "build")
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "object")
        {
            if (Player != null)
            {
                if (Player.stasissed) //current object is stasissed
                {
                    //simulate object hit

                    Debug.Log("hit object");
                }
            }
        }

    }

    public void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);

      
        if (other.gameObject.tag == "object")
        {
            if (Player != null)
            {
                if (Player.stasissed) //current object is stasissed
                {
                    //simulate object hit

                    Debug.Log("hit object");
                }
            }
        }

    }
}
