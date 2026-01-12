using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fnPlayer : MonoBehaviour
{

    public float score = 0f;         // The player's score
    public float scoreMultiplier = 1f; // Base multiplier, you can tweak


    private Vector3 accelBaseline;
    private Vector3 accelBaseline2;

    private Vector3 tiltBaseline;
    private Vector3 tiltBaseline2;

    public float deadZone;


    public float tiltThresh;

    public float jumpThresh;
    public float jumpForce;
    public bool jumped;
    public bool isGrounded;

    public Rigidbody rb;

    public float accelMagnitude;

    public GameObject cam;

    // public bool usingRune;

    public GameObject ascendObj;

    public Camera fpsCam;


    public Vector3 rayOrigin1; // = transform.position + new Vector3(0, 1, 0); //fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));    // fpsCam.transform.position; //

    public RaycastHit hit1;



    public GameObject[] builds; //wall, floor, ramp, cone
    public int currentBuild;

    public bool isBuilding;

    public float scrollThreshold = 0.5f;  // Amount of scroll required to switch weapon
    public float scrollAccumulator = 0f; // Accumulates scroll input

    public GameObject buildpoint;

    public GameObject currentBuildPiece;

    public Vector3 buildPointOffset;

    public GameObject orientation;


    public Collider pickaxe;


    public Collider currentPieceCol;

    public bool CanPlace;

    public static bool CanPlaceBuild;



    public Renderer BuildRend;

    public Material red;
    public Material white;
    public Material clear;

    public float placeInterval = 0.1f;
    public float lastPlace;


    public float conPointMaxDist;
    public GameObject closestConPoint;

    public bool usingNewSnap;


    public List<GameObject> oldPieces;


    public GameObject[] conPoints;

    public int touchingCount;

    public TextMeshProUGUI buildText;

    public string[] texts;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent falling over

        pickaxe.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        CanPlace = CanPlaceBuild;

        BuildMenu();

        BuildSnap();

        //  CameraRotation();

        if (!isBuilding)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                //simulate pickaxe swing to break build in front of player

                //pickaxe.enabled = true;

               // gen.pickaxing = true;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                pickaxe.enabled = false;
            }
        }

        for (int i = 0; i < oldPieces.Count - 1; ++i)
        {
            if (oldPieces[i] != null)
            {
                if (!oldPieces[i].activeSelf)
                {

                    Destroy(oldPieces[i]);
                }
            }
        }


        buildText.SetText(texts[currentBuild]);

        if (isBuilding)
        {
            buildText.gameObject.SetActive(true);
        }

        else
        {
            buildText.gameObject.SetActive(false);
        }


        if (transform.position.y < -5)
        {
            //reload scene

            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }

    public void BuildMenu()
    {

        //switch into and out of build mode

        if (Input.GetKeyDown(KeyCode.Q))
        {
            isBuilding = !isBuilding;

            if (isBuilding)
            {
                SpawnCurrentBuildFromSwitch();
            }

            if (!isBuilding)
            {
                //remove current build piece
                currentBuildPiece.SetActive(false);
                currentBuildPiece = null;
            }
        }

        if (isBuilding)
        {
            //scroll wheel to switch build


            ScrollBuild();



            PlaceBuild();


            //material colours
            if (CanPlace)
            {
                //currentBuildPiece.GetComponent<Renderer>().material = white;
                currentBuildPiece.GetComponent<Renderer>().material = clear;
            }

            else
            {
                currentBuildPiece.GetComponent<Renderer>().material = red;
            }


            //red material if can't place


            //Physics.OverlapBox(currentPieceCol.bounds)

            //

            // BuildPieceTriggerEnter(currentPieceCol);

            // BuildPieceTriggerStay(currentPieceCol);

            // BuildPieceTriggerExit(currentPieceCol);


        }
    }

    /*  if (currentBuild < builds.Length - 1)
            {
                //if scroll up, currentBuild++;
            }

            if (currentBuild > 0)
            {
                //if scroll down, currentBuild--;
            }
    */

    public void ScrollBuild()
    {
        scrollAccumulator += Input.GetAxis("Mouse ScrollWheel");

        //  Debug.Log("Status: scrolling, " + scrollAccumulator);

        if (scrollAccumulator <= -scrollThreshold) // Scrolled forward enough
        {
            if (currentBuild < builds.Length - 1)
            {
                if (builds[currentBuild + 1] != null)
                {
                    currentBuild++;

                    SpawnCurrentBuild();
                }
            }

            else if (currentBuild == builds.Length - 1)
            {
                currentBuild = 0;

                SpawnCurrentBuild();
            }


            scrollAccumulator = 0f; // Reset 
        }

        else if (scrollAccumulator >= scrollThreshold) // Scrolled backward enough
        {
            if (currentBuild > 0)
            {
                currentBuild--;

                SpawnCurrentBuild();
            }

            else if (currentBuild == 0)
            {
                //currentBuild = builds.Length - 1;

                currentBuild = 3;

                SpawnCurrentBuild();
            }

            scrollAccumulator = 0f; // Reset 
        }
    }


    public void SpawnCurrentBuild()
    {
        // Destroy(currentBuildPiece); // = null;

        oldPieces.Add(currentBuildPiece);


        currentBuildPiece.SetActive(false);
        currentBuildPiece = null;
        currentBuildPiece = Instantiate(builds[currentBuild], buildpoint.transform); //+ 0.66
        currentBuildPiece.transform.parent = buildpoint.transform;

        currentBuildPiece.transform.localPosition = Vector3.zero;


        //change to trigger
        //currentBuildPiece.GetComponent<Collider>().enabled = false;

        currentBuildPiece.GetComponent<Collider>().isTrigger = true;
        currentPieceCol = currentBuildPiece.GetComponent<Collider>();


        BuildPiece piece = currentBuildPiece.gameObject.GetComponent<BuildPiece>();

        conPoints = piece.conPoints;
    }

    public void SpawnCurrentBuildFromSwitch()
    {
        // Destroy(currentBuildPiece); // = null;
        //  currentBuildPiece.SetActive(false);
        //  currentBuildPiece = null;


        oldPieces.Add(currentBuildPiece);

        currentBuildPiece = Instantiate(builds[currentBuild], buildpoint.transform);
        currentBuildPiece.transform.parent = buildpoint.transform;

        currentBuildPiece.transform.localPosition = Vector3.zero;


        // currentBuildPiece.GetComponent<Collider>().enabled = false;

        currentBuildPiece.GetComponent<Collider>().isTrigger = true;
        currentPieceCol = currentBuildPiece.GetComponent<Collider>();

        //grid snaps


        BuildPiece piece = currentBuildPiece.gameObject.GetComponent<BuildPiece>();

        conPoints = piece.conPoints;
    }



    public void PlaceBuild()
    {
        if (Input.GetKey(KeyCode.Mouse0) && CanPlace) //Change to GetKeyDown to remove turbo building
        {
            if (lastPlace + placeInterval < Time.time)
            {
                //set collider to active

                //currentBuildPiece.GetComponent<Renderer>().material = white;
                //  currentBuildPiece.GetComponent<Collider>().enabled = true;


                currentBuildPiece.GetComponent<Collider>().isTrigger = false;
                currentBuildPiece.transform.parent = null;
                currentBuildPiece = null;

                SpawnCurrentBuildFromSwitch();

                CanPlace = false;

                lastPlace = Time.time;
            }
        }
    }



    public void BuildSnap()
    {

        //need to add snapping to connection point if in range

        //find all con points in range

        //get closest, then snap

        //if con points in range is 0, then run main snap

        List<conPoint> potentialConPoints = new List<conPoint>();

        //get objects in range + add to list above

        //potentialConPoints.Add(FindObjectOfType<conPoint>)

        conPoint[] point = FindObjectsOfType<conPoint>();


        // potentialConPoints.Add(point);

        potentialConPoints = point.ToList();

        //filter by distance

        List<GameObject> Conpoints = new List<GameObject>();


        float distToClosest = 10000f;
        float distToPoint;


        NearConPoint();


        for (int i = 0; i < potentialConPoints.Count; i++)
        {
            distToPoint = Vector3.Distance(transform.position, potentialConPoints[i].transform.position);

            //Debug.Log(distToPoint);

            if (distToPoint < conPointMaxDist)
            {
                Conpoints.Add(potentialConPoints[i].gameObject);
            }

            if (distToPoint < distToClosest)
            {
                //new closest

                distToClosest = distToPoint;

                closestConPoint = potentialConPoints[i].gameObject;
            }
        }

        if (usingNewSnap) //uses con points
        {

            if (Conpoints.Count > 0) //snap to closest
            {
                //closestConPoint (+offset?)

                Debug.Log("Has close point");

                Vector3 posNew = orientation.transform.forward * 2;

                Vector3 currentPos = orientation.transform.position + posNew;



                // Snap to global grid
                float gridSize = 4f;

                Vector3 snappedPos = closestConPoint.transform.position;    //new Vector3(Mathf.Round(currentPos.x / gridSize) * gridSize, Mathf.Round(currentPos.y / gridSize) * gridSize, Mathf.Round(currentPos.z / gridSize) * gridSize);

                buildpoint.transform.position = snappedPos;

                //Buildpoint rotation - snap to closest 90
                Vector3 euler = orientation.transform.eulerAngles;
                float snappedY = Mathf.Round(euler.y / 90f) * 90f;
                buildpoint.transform.rotation = Quaternion.Euler(0, snappedY, 0);


            }

            else
            {
                //regular snap
                RegSnap();

                Debug.Log("Does not have close point");
            }
        }

        else //no con points - first version
        {
            Vector3 posNew = orientation.transform.forward * 2;

            Vector3 currentPos = orientation.transform.position + posNew;

            // Snap to global grid
            float gridSize = 4f;

            //if name is Wall(Clone)
            Vector3 snappedPos = new Vector3(Mathf.Round(currentPos.x / gridSize) * gridSize, Mathf.Round(currentPos.y / gridSize) * gridSize, Mathf.Round(currentPos.z / gridSize) * gridSize);

            //else, adjust snap

            if (isBuilding)
            {
                if (currentBuildPiece.name == "Ramp(Clone)")
                {
                    Debug.Log("Ramp snapping");

                    snappedPos = new Vector3((Mathf.Round(currentPos.x / gridSize) * gridSize) + 2, Mathf.Round(currentPos.y / gridSize) * gridSize, (Mathf.Round(currentPos.z / gridSize) * gridSize));// + 2);

                    //   snappedPos = new Vector3((Mathf.Round(currentPos.x / gridSize) * gridSize) + gridSize/2, Mathf.Round(currentPos.y / gridSize) * gridSize, Mathf.Round(currentPos.z / gridSize) * gridSize);

                }

                if (currentBuildPiece.name == "Floor(Clone)")
                {
                    Debug.Log("Floor snapping");

                    snappedPos = new Vector3((Mathf.Round(currentPos.x / gridSize) * gridSize) + 2, (Mathf.Round(currentPos.y / gridSize) * gridSize) - 2f, (Mathf.Round(currentPos.z / gridSize) * gridSize));// + 2);

                    //   snappedPos = new Vector3((Mathf.Round(currentPos.x / gridSize) * gridSize) + gridSize/2, Mathf.Round(currentPos.y / gridSize) * gridSize, Mathf.Round(currentPos.z / gridSize) * gridSize);

                }

                if (currentBuildPiece.name == "Cone (2)(Clone)") //Cone (2)(Clone)
                {
                    Debug.Log("Cone snapping");

                    snappedPos = new Vector3((Mathf.Round(currentPos.x / gridSize) * gridSize) + 2, (Mathf.Round(currentPos.y / gridSize) * gridSize) - 1.2f, (Mathf.Round(currentPos.z / gridSize) * gridSize));// + 2);

                    //   snappedPos = new Vector3((Mathf.Round(currentPos.x / gridSize) * gridSize) + gridSize/2, Mathf.Round(currentPos.y / gridSize) * gridSize, Mathf.Round(currentPos.z / gridSize) * gridSize);


                    //need to shift cone up when on ground

                    if (currentBuildPiece.transform.position.y < 2)
                    {
                        snappedPos = new Vector3((Mathf.Round(currentPos.x / gridSize) * gridSize) + 2, (Mathf.Round(currentPos.y / gridSize) * gridSize) - 0.6f, (Mathf.Round(currentPos.z / gridSize) * gridSize));// + 2);

                    }
                }


                if (currentBuildPiece.name == "Wall(Clone)")
                {
                    Debug.Log("Wall snapping");


                    //  135 + (to 180) and - 180 – -135 is bad
                    // - 45 to 45 is bad

                    //if greater than 135, less than -135, or > -45 and -less than 45
                    //if (orientation.transform.rotation.y < 0)
                    //if (orientation.transform.rotation.y < -135 || orientation.transform.rotation.y > 135 || (orientation.transform.rotation.y > -45 && orientation.transform.rotation.y < 45))


                    // convert Y to signed (-180..180)
                    float y = orientation.transform.eulerAngles.y;


                    if (y > 180f)
                    {
                        y -= 360f;
                    }

                    if (y < -135f || y > 135f || (y > -45f && y < 45f))
                    {

                        Debug.Log("Bad grid");

                        snappedPos = new Vector3((Mathf.Round(currentPos.x / gridSize) * gridSize) + 2, (Mathf.Round(currentPos.y / gridSize) * gridSize), (Mathf.Round(currentPos.z / gridSize) * gridSize) + 2);
                    }


                    //   snappedPos = new Vector3((Mathf.Round(currentPos.x / gridSize) * gridSize) + gridSize/2, Mathf.Round(currentPos.y / gridSize) * gridSize, Mathf.Round(currentPos.z / gridSize) * gridSize);

                }
            }

            //add cone snapping when have cone piece

            buildpoint.transform.position = snappedPos;

            //Buildpoint rotation - snap to closest 90
            Vector3 euler = orientation.transform.eulerAngles;
            float snappedY = Mathf.Round(euler.y / 90f) * 90f;
            buildpoint.transform.rotation = Quaternion.Euler(0, snappedY, 0);

        }
    }

    public void RegSnap()
    {
        Vector3 posNew = orientation.transform.forward * 2;

        Vector3 currentPos = orientation.transform.position + posNew;



        // Snap to global grid
        float gridSize = 4f;

        Vector3 snappedPos = new Vector3(Mathf.Round(currentPos.x / gridSize) * gridSize, Mathf.Round(currentPos.y / gridSize) * gridSize, Mathf.Round(currentPos.z / gridSize) * gridSize);

        buildpoint.transform.position = snappedPos;

        //Buildpoint rotation - snap to closest 90
        Vector3 euler = orientation.transform.eulerAngles;
        float snappedY = Mathf.Round(euler.y / 90f) * 90f;
        buildpoint.transform.rotation = Quaternion.Euler(0, snappedY, 0);
    }

    public void CloseSnap()
    {
        //to fix offset

        //either + ofsset to snappedPos in snapping function

        //or change build prefabs to add inbult offset for specific pieces


        //^poss need to change colliders of each to diff size or shape - poss cube? so can fill space of grid block


    }

    public void NearConPoint()
    {
        //Build currently being placed checks its connection point collision areas, if a build that has been placed is inside at least one of them, can place

        //If build placed in none of them, can’t place

        // bool canPlace = true;

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


        //lower than 2 automatically can place as on ground

        if (transform.position.y > 2)
        {
            if (touchingCount > 0) // at least 1
            {
                //can place

                CanPlaceBuild = true;
            }

            else
            {
                //can't place

                CanPlaceBuild = false;
            }
        }

        else // less than or at 2
        {
            //wall or floor, keep same
            if (currentBuild == 0 || currentBuild == 1)
            {
                CanPlaceBuild = true;
            }

            else //ramp or cone
            {
                if (currentBuildPiece != null)
                {
                    Collider col = currentBuildPiece.GetComponent<Collider>();

                    BuildPiece p = currentBuildPiece.GetComponent<BuildPiece>();

                    if (p.containsInTrigger) //contains build inside the trigger zone
                    {
                        CanPlaceBuild = false;
                    }
                    else
                    {
                        CanPlaceBuild = true;
                    }
                }
            }

            //change so wall and floor stay as above


            //ramp and cone use their colliders to detect whether a placed ramp or cone is inside them


        }
    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        jumped = false;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;

        //cam.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }


    private int buildOverlapCount = 0;

}

/*
    public void BuildPieceTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "build")
        {
            buildOverlapCount++;
            CanPlace = false;
        }
    }

    public void BuildPieceTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "build")
        {
            if (buildOverlapCount > 0)
            {
                buildOverlapCount -= 1;
            }
            //buildOverlapCount = Mathf.Max(0, buildOverlapCount - 1);

            if (buildOverlapCount == 0)
            {
                CanPlace = true;
            }

            else if (buildOverlapCount > 0)
            {
                CanPlace = false;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }




}

*/