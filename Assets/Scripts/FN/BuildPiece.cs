using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPiece : MonoBehaviour
{

    public Collider col;

    public bool Placed;


    public Renderer rend;
    public Material white;
    public Material clear;

    public GameObject[] conPoints;

    public Collider[] conPointCols;


    public int touchingCount;

    public bool isConnected;



    public bool Checked;

    public List<GameObject> buildsOBJ;

    public float buildDistThresh;


    public bool containsInTrigger;


    public bool touchingGround;

    public bool isCon;

    public bool hasConned;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();

        rend = GetComponent<Renderer>();

        isConnected = true; 

        Placed = false;

        fnPlayer.CanPlaceBuild = true;

        //  conPoints = FindObjectsOfType<GameObject>();

        for (int i = 0; i < conPoints.Length; i++)
        {
            // conPoints[i].SetActive(false);
           // conPointCols[i] = conPoints[i].gameObject.GetComponent<Collider>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isCon)
        {
            hasConned = true;
        }

        if (touchingGround)
        {
            isCon = true;
        }

        if (col.isTrigger)
        {
            Placed = false;
        }

        else
        {
            Placed = true;
        }

        if (!Placed)
        {
            //if can place, make clear
        }

        if (Placed)
        {
            rend.material = white;

            for (int i = 0; i < conPoints.Length; i++)
            {
               // conPoints[i].SetActive(true);
            }
        }

        else //not placed
        {
            for (int i = 0; i < conPoints.Length; i++)
            {
               // conPoints[i].SetActive(false);
            }
        }

        if (!Placed)
        {
           // Connected();
        }

        if (Placed)
        {
           // StillConnected();
        }

        if (Placed)
        {
            if (!isConnected)
            {
            //    Destroy(gameObject);
            }
        }



        if (Placed)
        {
            if (!Checked)
            {
                CheckOverLap();

                Checked = true;
            }
        }
    }

    private void LateUpdate()
    {
        if (!isCon && hasConned)
        {
            Destroy(gameObject);
        }
    }

    public void CheckConnections()
    {
        /*
         * When build is destroyed, run this function

A build checks which con points are connected, and finds the build pieces they are touching

Then finds which build pieces that build’s con points are touching, etc

And if finds one that touches floor, knows is connected

If doesn’t, then not connected


^poss max iteration count / not moving back to already checked pieces

         * */



        touchingCount = 0;

        for (int i = 0; i < conPoints.Length; i++)
        {
            conPoint p = conPoints[i].gameObject.GetComponent<conPoint>();


            if (p.touching)
            {
                // canPlace = false;

                touchingCount += 1;
            }
        }

        Debug.Log("touching:" + touchingCount);


        if (touchingCount > 0) // at least 1
        {
            //get object(s) touching


            for (int i = 0; i < conPoints.Length; i++)
            {
                conPoint p = conPoints[i].gameObject.GetComponent<conPoint>();

                //use conpoint's touchingOBJ list

                //p.touchingOBJs



            }
        }

        else
        {
            //def on ground? - as if have 0 con points, must be on ground to exist
        }
    }

    public void CheckOverLap()
    {

        Debug.Log("Checking distance");

        BuildPiece[] buildPieces = FindObjectsOfType<BuildPiece>();

        //GameObject[] buildsOBJ = //builds;

       // List<GameObject> buildsOBJ;

        for (int i = 0; i < buildPieces.Length; i++)
        {
            buildsOBJ.Add(buildPieces[i].gameObject);
        }


        for (int i = 0; i < buildsOBJ.Count; i++)
        {
            //distance

           float curDist = Vector3.Distance(transform.position, buildsOBJ[i].transform.position);

            BuildPiece p = buildsOBJ[i].GetComponent<BuildPiece>();



            if (curDist < buildDistThresh)
            {
                if (buildsOBJ[i] != gameObject)
                {
                    if (p.Placed)
                    {
                        Debug.Log("Checking distance bad");
                          Destroy(gameObject);
                    }
                }
            }
        }

        //check distance of each
    }


    public void NearConPoint()
    {
        //Build currently being placed checks its connection point collision areas, if a build that has been placed is inside at least one of them, can place

        //If build placed in none of them, can’t place

        // bool canPlace = true;

        Debug.Log("touching:" + touchingCount);


        //lower than 2 automatically can place as on ground

          
    }


    //not active
    public void StillConnected()
    {
        //check if have connection

        touchingCount = 0;

        for (int i = 0; i < conPoints.Length; i++)
        {
            conPoint p = conPoints[i].gameObject.GetComponent<conPoint>();


            if (p.touching)
            {
                // canPlace = false;

                touchingCount += 1;
            }
        }

        Debug.Log("build touch: " + touchingCount);

        if (transform.position.y > 2)
        {
            if (touchingCount > 0) // at least 1
            {
                //can place

                isConnected = true;
            }

            else
            {
                //can't place

                isConnected = false;
            }
        }
    }


    public void Connected()
    {
        //need to check if con points connect to other build piece 

        //^poss just in range?

        //check if in trigger of con point script?
    }



    public void OnCollisionEnter(Collision collision)
    {
        if (Placed)
        {
            if (collision.gameObject.tag == "build")
            {
                Debug.Log("Touching build");

                if (collision.gameObject.name == gameObject.name)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!Placed)
        {
            if (other.gameObject.tag == "build")
            {
                //can't place

                //  fnPlayer.CanPlaceBuild = false;

                if (other.gameObject.name == "Cone (2)(Clone)" || other.gameObject.name == "Ramp(Clone)")
                {
                    containsInTrigger = true;
                }
            }
        }

        if (Placed)
        {
            if (other.gameObject.tag == "ground")
            {
                touchingGround = true;
            }
        }
       
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Placed)
        {
            if (other.gameObject.tag == "build")
            {
                //can't place

              //  fnPlayer.CanPlaceBuild = false;
            }
        }

        if (Placed)
        {
            if (other.gameObject.tag == "ground")
            {
                touchingGround = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!Placed)
        {


            if (other.gameObject.tag == "build")
            {
                //can place
                if (other.gameObject.name == "Cone (2)(Clone)" || other.gameObject.name == "Ramp(Clone)")
                {
                    containsInTrigger = false;
                }
                //  fnPlayer.CanPlaceBuild = true;
            }
        }
    }
}
