using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class manager : MonoBehaviour {
    public enum playstates { PlaneSearch,CatPlacement,InGame};
    public playstates playerStates;
    public GameObject plane;
    public Camera cam;
    public GameObject catobj;
    bool planefound = false;
    Anchor mainAnchor;

    //
  public  Text TestingText;
    public Text PlaneCountText;
    //

    void Awake()
    {
        cam = Camera.main;
    }


    void FindPlane()
    {
        // this may be bad
        List<TrackedPlane> newPlanes = new List<TrackedPlane>();
        Frame.GetNewPlanes(ref newPlanes);
      


            // Iterate over planes found in this frame and instantiate corresponding GameObjects to visualize them.
            for (int i = 0; i < newPlanes.Count; i++)
            {
                // Instantiate a plane visualization prefab and set it to track the new plane.
                GameObject planeObject = Instantiate(plane, Vector3.zero, Quaternion.identity,
                    transform);
                planeObject.GetComponent<PlaneVisualizer>().SetPlane(newPlanes[i]);
            }

        if(newPlanes.Count<2)
        { 
           // planefound = true;
            playerStates = playstates.CatPlacement;
            PlaneCountText.text = "count: " + newPlanes.Count;
            TestingText.text = "plane found";
        }

    }

 
    
    void Update()
    {

        // Skip the update if ARCore is not tracking
        if (Frame.TrackingState != FrameTrackingState.Tracking)
        {
            return;
        }

        switch (playerStates)
        {
            case playstates.PlaneSearch:
                FindPlane();
                
                break;
            case playstates.CatPlacement:
                AnchorCatPlacement();
                break;
            case playstates.InGame:
                break;
            
        }       
    }

    void AnchorCatPlacement()
    {
        // See if user made an touch event
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        TrackableHit hitResult;
        // Create a raycast filter
        TrackableHitFlag raycastFilter = TrackableHitFlag.PlaneWithinBounds | TrackableHitFlag.PlaneWithinPolygon;

        // Create a Raycast with touch position, filters and hitResult object
        if (Session.Raycast(cam.ScreenPointToRay(touch.position), raycastFilter, out hitResult))
        {
            // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
            // world evolves.
            mainAnchor = Session.CreateAnchor(hitResult.Point, Quaternion.identity);
            playerStates = playstates.InGame;
            TestingText.text = "Ready to play";
            // Intanstiate a Deadpool object as a child of the anchor;
            // It's transform will now benefit from the anchor's tracking.
          //  var newcat = Instantiate(catobj, hitResult.Point, Quaternion.identity, mainAnchor.transform);

            // Make the model look at the camera
          //  newcat.transform.LookAt(cam.transform);
           // newcat.transform.rotation = Quaternion.Euler(0.0f, newcat.transform.rotation.eulerAngles.y, newcat.transform.rotation.z);


        }
    }
}
