using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConnectionChecker : MonoBehaviour
{

    public List<GameObject> builds;


    public conPoint con;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        RefreshBuildList();

        ConnectionChecks();
    }


    public void RefreshBuildList()
    {
        builds.Clear();

        BuildPiece[] buildPieces = FindObjectsOfType<BuildPiece>();


        for (int i = 0; i < buildPieces.Length; i++)
        {
            if (buildPieces[i].Placed)
            {
                GameObject curOBJ = buildPieces[i].gameObject;


                builds.Add(curOBJ);     // = buildPieces.ToList<GameObject>();
            }
        }

    }

    public void ConnectionChecks()
    {
        //for each build
        for (int i = 0; i < builds.Count; i++)
        {
            {
                //check if on ground

                BuildPiece p = builds[i].GetComponent<BuildPiece>();

                if (p.touchingGround)
                {
                    //is connected
                }

                else if (!p.touchingGround)//not on ground
                {
                    //if touching / next to piece on ground (using dist?)

                    //check con points?

                    Debug.Log("Checking builds");


                    //poss best way

                    //check con points - which builds they are touching
                    //if touching one on the ground, then is connected

                    GameObject[] conPs = p.conPoints;

                    for (int j = 0; j < conPs.Length; j++)
                    {
                        con = conPs[j].GetComponent<conPoint>();

                        //if(conPs[i] != null)

                        if (con != null)
                        {
                            if (con.touching)
                            {
                                GameObject touchingOBJ = con.touchingOBJ;

                                if (touchingOBJ != null)
                                {
                                    BuildPiece touchingP = touchingOBJ.GetComponent<BuildPiece>();

                                    if (touchingP.touchingGround)
                                    {
                                        //then connected to piece on ground

                                        p.isCon = true;
                                    }
                                }

                                else
                                {
                                    p.isCon = false;
                                }

                                //if touching build point on ground
                            }
                        }
                    }
                }
                
            }
        }
    }
}
