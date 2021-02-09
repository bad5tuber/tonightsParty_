using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ui doesnt come by default

[RequireComponent(typeof(LineRenderer))]

// allows us to add this variable as internal variable
// and requires us to have this component, thus ensuring
// no errors

public class VRLocomotion : MonoBehaviour
{
    [Header ("Teleport")]
    public Transform xrRig;
    public string handness = "Right";
    [SerializeField] float rotationDegrees = 30.0f;


    [Header ("Curve Details")]
    [SerializeField] public float curveHeight = 1.5f;
    [SerializeField] public int lineResolution = 20;


    [Header("Smooth Reticle")]
    public Transform teleportReticle;
    [SerializeField] public float smoothnessValue = 0.2f;

    [Header("Fade Screen")]
    public RawImage blackScreen;

    // Internal variables
    private LineRenderer lr;
    public bool teleportLock;

    private void Start()
    {
        InitializeLineRenderer();
    }



    private void Update() 
    {
        Rotate();
        HandleRaycast();
    }

    void Rotate()
    {
        if (Input.GetButtonDown(handness + "_Stick_Click"))
        {
            // Determine the rotation direction
            // ? gets boolean value 
            // will provide true or false if we're rotating left (false) or right (true)

            float rot = Input.GetAxis(handness + "_Joystick") > 0 ? rotationDegrees : -rotationDegrees;

            // rotate the user

            xrRig.transform.Rotate(0, rot, 0);
        }
    }

    void HandleRaycast()
    {
        // new ray starts at the controller and moves forward
        Ray ray = new Ray(transform.position, transform.forward);

        // gives info aabout where raycast hit
        RaycastHit hitInfo = new RaycastHit();

        // out allows us to return multiple bits of information
        // by default Raycast() returns true or false
        // out allows us to pass another piece of info
        // if true, it means our raycast hit something
        // if false, it means our raycast did not hit something
        // its basically appending additional data to thes method return
        // for more clarity on what took place

        if(Physics.Raycast(ray, out hitInfo)) 
        {
            lr.enabled = true;
            teleportReticle.gameObject.SetActive(true);

            // if the raycast collider tag is ground
            // i.e. if you're casting to the ground
            // validTarget == true

            bool validTarget = hitInfo.collider.tag == "Ground";

            // if the target is valid (ie is ground)
            // color will be green else gray

            Color lrColor = validTarget ? Color.green : Color.gray;

            // start and end color == lrcolor

            lr.startColor = lrColor;
            lr.endColor = lrColor;

            // LERP TIME

            Vector3 startPoint = transform.position;
            Vector3 desiredPoint = hitInfo.point;

            // initially used end point rather than desired point
            // given that end point creates an inflated velocity,
            // we will smooth it out by looking at where the reticle
            // was in the last frame and subtracting that vector from
            // the desired endpoint. We can then divide that by a
            // smoothness value and multiply by t.dt to smooth out
            // the motion of the raycast and make framerate independent

            Vector3 vecToDesired = desiredPoint - teleportReticle.position;
            Vector3 smootherVecToDesired = (vecToDesired / smoothnessValue) * Time.deltaTime;
            Vector3 endPoint = teleportReticle.position + smootherVecToDesired;

            teleportReticle.position = desiredPoint;

            Vector3 vecFromStartToEnd = desiredPoint - startPoint;
            Vector3 halfVecFromStartToEnd = vecFromStartToEnd/2;

            Vector3 midPoint = startPoint + halfVecFromStartToEnd;
            midPoint.y += curveHeight;

            // For loop that loops through each point in curve

            for(int i = 0; i < lineResolution; i++)
            {
                // this basically divides the line into fractions based
                // on value of lineResolution
                // eg lr of 4 would return .25, .5, .75
                // convert to a float given return will be fraction

                float t = i / (float)lineResolution;
                Vector3 startToMidLerp = Vector3.Lerp(startPoint, midPoint, t);
                Vector3 midToEndLerp = Vector3.Lerp(midPoint, desiredPoint, t);
                Vector3 curvePos = Vector3.Lerp(startToMidLerp, midToEndLerp, t);

                lr.SetPosition(i, curvePos);
            }

            // set the start of the LR to your position
            // set the end of the LR to the hitInfor var

            // FOR STRAIGHT LINE:
            // lr.SetPosition(0, transform.position);
            // lr.SetPosition(1, hitInfo.point);

            // raycast every frame and also check if you pressed trigger
            // if true, we'll move to where the ray hit the collider

            if(teleportLock == false && validTarget == true && Input.GetButtonDown(handness + "_Trigger"))
            {
                StartCoroutine(FadeTeleport(hitInfo.point));
                // xrRig.position = hitInfo.point;
            }
        }

        else
        {
            lr.enabled = false;
            teleportReticle.gameObject.SetActive(false);
        }
    }
    private void InitializeLineRenderer()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;

        // positionCount = 2 means 1 point at start and end
        // will draw straight line between these two points
        // UPDATE to lineResolution

        lr.positionCount = lineResolution;
    }

    // given we're in a different function, we're getting pos
    // passed in
    private IEnumerator FadeTeleport(Vector3 pos)
    {
        // make sure teleport cant be called again
        teleportLock = true;
        // reset time counter var
        float currentTime = 0;
        // loop until counter is done
        while(currentTime < 1)
        {
            // fade out screen
            blackScreen.color = Color.Lerp(Color.clear, Color.black, currentTime);
            // wait one frame
            yield return null;
            // increment timer
            currentTime += Time.deltaTime;
        }
        // set full black screen
        blackScreen.color = Color.black;
        // move user
        xrRig.transform.position = pos;
        // wait one second
        yield return new WaitForSeconds(1);
        // reset timer again
        currentTime = 0;
        // loop until timer is done again
        while (currentTime <1)
        {
            // fade out screen
            blackScreen.color = Color.Lerp(Color.black, Color.clear, currentTime);
            // wait one frame
            yield return null;
            // increment timer
            currentTime += Time.deltaTime;
        }
        // allow teleporting again
        teleportLock = false;
    }

}
