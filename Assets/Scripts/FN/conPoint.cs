using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conPoint : MonoBehaviour
{

    public Collider col;

    public bool touching;

    public Transform parentobj;

    public Transform parentobj2;

    public List<GameObject> touchingOBJs;


    public GameObject touchingOBJ;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();

        col.isTrigger = true;
        // col.enabled = false;

        parentobj = gameObject.transform.parent;

        parentobj2 = parentobj.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //disable con point when placed
    }

    
    public void OnTriggerEnter(Collider other)
    {
        //if other is build piece, is in range

        if (other.gameObject.tag == "build")
        {
            if (other.gameObject != parentobj2.gameObject)
            {
                BuildPiece build = other.gameObject.GetComponent<BuildPiece>();

                if (build.Placed)
                {
                    touching = true;
                    //  gen.buildCanConnect = true;

                    touchingOBJs.Add(other.gameObject);

                    touchingOBJ = other.gameObject;
                }
            }
        }
    }

    //poss on trigger stay could fix con points not detecting valid positions?

    public void OnTriggerStay(Collider other)
    {
        //if other is build piece, is in range

        if (other.gameObject.tag == "build")
        {
            if (other.gameObject != parentobj2.gameObject)
            {
                BuildPiece build = other.gameObject.GetComponent<BuildPiece>();

                if (build.Placed)
                {
                    touching = true;
                    //  gen.buildCanConnect = true;

                    touchingOBJ = other.gameObject;
                }
            }
        }
    }


    public void OnTriggerExit(Collider other)
    {
        //if other is build piece, is in range

        if (other.gameObject.tag == "build")
        {
            if (other.gameObject != parentobj2.gameObject)
            {
                BuildPiece build = other.gameObject.GetComponent<BuildPiece>();

                if (build.Placed)
                {
                    touching = false;

                    touchingOBJs.Remove(other.gameObject);
                    //  gen.buildCanConnect = true;

                    touchingOBJ = null;
                }
            }
        }
    }
}
