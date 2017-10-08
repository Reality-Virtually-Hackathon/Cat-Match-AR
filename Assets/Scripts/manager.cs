using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class manager : MonoBehaviour {
    public static manager instance;
    public enum playstates { PlaneSearch,CatPlacement,InGame};
    public playstates playerStates;
    public GameObject plane;
    public Camera cam;
    public GameObject catobj;
    bool planefound = false;
    Anchor mainAnchor;
    public GameObject mainCat;
    List<TrackedPlane> areaPlanes = new List<TrackedPlane>();
    public List<GameObject> cats = new List<GameObject>();
    int currentCatValue=0;
    //
    public  Text TestingText;
    public Text PlaneCountText;
    public Text chosencatText;
    public GameObject catSprite;

    //


    public void ChangeOutCat(int direction)
    {
        if (playerStates == playstates.InGame)
        {
            if (direction == 1)
            {
                if (currentCatValue == 2)
                {
                    currentCatValue = 0;
                    cats[2].SetActive(false);
                    cats[0].SetActive(true);

                    cats[0].transform.position = mainAnchor.transform.position;
                    cats[0].transform.parent = mainAnchor.transform;
                    cats[0].transform.rotation = Quaternion.identity;
                    mainCat = cats[0].gameObject;
                }
                else
                {
                    cats[currentCatValue].SetActive(false);
                    currentCatValue++;
                    cats[currentCatValue].SetActive(true);

                    cats[currentCatValue].transform.position = mainAnchor.transform.position;
                    cats[currentCatValue].transform.parent = mainAnchor.transform;
                    cats[currentCatValue].transform.rotation = Quaternion.identity;
                    mainCat = cats[currentCatValue].gameObject;

                }
                mainCat.transform.position = mainAnchor.transform.position;
            }

        }


    }

  public  void ResetGettingPlane()
    {
        for (int i = 0; i < areaPlanes.Count; i++)
        {
            areaPlanes.RemoveAt(0);
        }
        playerStates = playstates.PlaneSearch;
        cats[currentCatValue].SetActive(false);
   
        //  Destroy(mainCat);
    }
    void Awake()
    {
        cam = Camera.main;
        instance = this;
        PlaneCountText.text = "starting ";
    }


    void FindPlane()
    {
        PlaneCountText.text = "count: " + areaPlanes.Count;

        Frame.GetNewPlanes(ref areaPlanes);
      
            // Iterate over planes found in this frame and instantiate corresponding GameObjects to visualize them.
            for (int i = 0; i < areaPlanes.Count; i++)
            {
                // Instantiate a plane visualization prefab and set it to track the new plane.
                GameObject planeObject = Instantiate(plane, Vector3.zero, Quaternion.identity,
                    transform);
                planeObject.GetComponent<PlaneVisualizer>().SetPlane(areaPlanes[i]);
            }

        if(areaPlanes.Count>0)
        { 
           // planefound = true;
            playerStates = playstates.CatPlacement;
            PlaneCountText.text = "count: " + areaPlanes.Count;
            TestingText.text = "plane found";
        }
    }

 
    
    void Update()
    {
        chosencatText.text = "curr: "+ currentCatValue;
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
                if(mainCat!=null)
                {
                    mainCat.transform.LookAt(cam.transform);
                    mainCat.transform.rotation = Quaternion.Euler(0.0f, mainCat.transform.rotation.eulerAngles.y, mainCat.transform.rotation.z);
                }
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
            if(hitResult.Plane == areaPlanes[0])
            {
                mainAnchor = Session.CreateAnchor(hitResult.Point, Quaternion.identity);
                playerStates = playstates.InGame;
                TestingText.text = "Ready to play";
                UIManager.instance.EnableGamePlayUI();
                //change to work with new cats
                cats[0].gameObject.SetActive(true);
                cats[0].transform.position = hitResult.Point;
                cats[0].transform.parent = mainAnchor.transform;
                cats[0].transform.rotation = Quaternion.identity;
                mainCat = cats[0].gameObject;
            }            
        }
    }
}
