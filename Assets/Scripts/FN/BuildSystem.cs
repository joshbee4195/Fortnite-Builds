using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{

    public Transform camChild;

    RaycastHit hit;


    Transform floorBuild;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(camChild.position, camChild.forward, out hit, 6f))
        {
            floorBuild.position = new Vector3(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.y), Mathf.RoundToInt(hit.point.z));
        }
    }

    public void BuildSnapOLD()
    {

        //need to snap to current closest while following player

        //var currentPos = buildpoint.transform.position;

        //  Debug.Log(orientation.transform.rotation.y * 180);

        //  Debug.Log(RoundToNearestAbs(orientation.transform.eulerAngles.y, 90));


        //need to snap buildpoint to 90 degree intervals


        /*
        Vector3 pos = orientation.transform.position;

        Vector3 posNew = orientation.transform.forward * 2;

        var currentPos = orientation.transform.position + posNew;//(orientation.transform.forward);





        buildpoint.transform.position = new Vector3(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y), Mathf.Round(currentPos.z));

        */

        /*
        // Position snapping 
        Vector3 posNew = orientation.transform.forward * 2;
        var currentPos = orientation.transform.position + posNew;

        //  buildpoint.transform.position = new Vector3(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y), Mathf.Round(currentPos.z));

        // Snap position to nearest 90 units
        buildpoint.transform.position = new Vector3(RoundToNearestAbs(currentPos.x, 90f), Mathf.Round(currentPos.y), RoundToNearestAbs(currentPos.z, 90f));

        */

        /*

        // Snap buildpoint position to a grid (optional, for grid placement)
        Vector3 posNew = orientation.transform.forward * 2;
        var currentPos = orientation.transform.position + posNew;
        buildpoint.transform.position = currentPos; // Or snap to a smaller grid if desired

        // Snap buildpoint rotation to nearest 90 degrees on Y axis
        Vector3 euler = orientation.transform.eulerAngles;
        float snappedY = Mathf.Round(euler.y / 90f) * 90f;
        buildpoint.transform.rotation = Quaternion.Euler(0, snappedY, 0);

        

        // Calculate the intended build point position in world space
        Vector3 posNew = orientation.transform.forward * 2;
        var currentPos = orientation.transform.position + posNew;

        // Snap to a global grid (e.g., 1 unit grid)
        float gridSize = 1f; // Change to 0.5f or 2f for different grid sizes
        Vector3 snappedPos = new Vector3(
            Mathf.Round(currentPos.x / gridSize) * gridSize,
            Mathf.Round(currentPos.y / gridSize) * gridSize,
            Mathf.Round(currentPos.z / gridSize) * gridSize
        );

        buildpoint.transform.position = snappedPos;

        // Snap buildpoint rotation to nearest 90 degrees on Y axis (still world-aligned)
        Vector3 euler = orientation.transform.eulerAngles;
        float snappedY = Mathf.Round(euler.y / 90f) * 90f;
        buildpoint.transform.rotation = Quaternion.Euler(0, snappedY, 0);

        // Rotation snapping 
        //  Vector3 euler = orientation.transform.eulerAngles;
        // float snappedY = RoundToNearestAbs(euler.y, 90f); // snap Y axis

        //  Debug.Log(snappedY);
        // buildpoint.transform.rotation = Quaternion.Euler(0, snappedY, 0);

        //Debug.Log("Rounded: " + RoundToNearest(137, 90));
        // float snappedAngle = RoundToNearest(angle, 90f);

        float RoundToNearest(float value, float nearest)
        {

            float valueInt;
            float valueFinal;

            valueInt = (Mathf.Round(value / nearest) * nearest);

            if (valueInt > 300)
            {
                valueFinal = 0;
            }

            else
            {
                valueFinal = valueInt;
            }

            return valueFinal;

        }

        float RoundToNearestAbs(float value, float nearest)
        {

            float valueInt;
            float valueFinal;

            valueInt = Mathf.Round(Mathf.Abs(value) / nearest) * nearest;

            if (valueInt > 300)
            {
                valueFinal = 0;
            }

            else
            {
                valueFinal = valueInt;
            }

            return valueFinal;
        }


        /*
         * 
         * Vector3 currentPos = transform.position;
transform.position = Vector3( Mathf.Round( currentPos.x / gridSize.x ),
                              Mathf.Round( currentPos.y / gridSize.y ),
                              Mathf.Round( currentPos.z / gridSize.z ) );
         * */

        //rotation locked to nearest 90 degrees

        //currently snaps 15-16 times

        //if snap 4 times less, 1 every 90?

        // buildpoint.transform.rotation = orientation.transform.rotation;

    }

    /*
     *   private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        
     * */




    /*
    public void BuildPieceTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "build")
        {
            CanPlace = false;
        }
    }

    public void BuildPieceTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "build")
        {
            CanPlace = false;
        }

        else
        {
            CanPlace = true;
        }
    }




    public void BuildPieceTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "build")
        {
            CanPlace = true;
        }
    }

    */






    //movement + cam look (3rd player)

    //move

    /*
    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
    {
        transform.position = transform.position + (transform.forward * scoreMultiplier);
    }

    if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
    {
        transform.position = transform.position + (-transform.forward * scoreMultiplier);
    }

    //rotate

    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
    {

        transform.position = transform.position + (-transform.right * scoreMultiplier);
        // transform.eulerAngles += new Vector3(0, -1, 0);
    }

    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
    {

        transform.position = transform.position + (transform.right * scoreMultiplier);
        // transform.eulerAngles += new Vector3(0, 1, 0);
    }



    //jump
    if (Input.GetKeyDown(KeyCode.Space))
    {
        if (!jumped)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Debug.Log("jump");

            jumped = true;
        }
    }

    //mouse look


    */

}
