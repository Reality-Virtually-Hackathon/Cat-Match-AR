using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class manager : MonoBehaviour {
    public static manager instance;
    public enum playstates { GameStart,PlaneSearch,CatPlacement,InGame,EndGame};
    public playstates CurrentGameState;
    public GameObject plane;
    public Camera cam;
    public GameObject catobj;
    bool planefound = false;
    Anchor mainAnchor;
    public GameObject mainCat;
    List<TrackedPlane> areaPlanes = new List<TrackedPlane>();
    public List<GameObject> cats = new List<GameObject>();
    int currentCatValue=0;
    public int chosenCat;
    public GameObject startImage;
    //
    public  Text TestingText;
    public Text PlaneCountText;
    public Text chosencatText;
    public GameObject catSprite;

    //

        // choose new cat
    public void ChangeOutCat(int direction)
    {
        if (CurrentGameState == playstates.InGame)
        {
            if (direction == 1)
            {
                if (currentCatValue == 2)
                {
                    currentCatValue = 0;
                    cats[2].SetActive(false);
                    cats[0].SetActive(true);
                    mainCat = cats[0].gameObject;
                    cats[0].transform.position = mainAnchor.transform.position;
                    cats[0].transform.parent = mainAnchor.transform;
                    cats[0].transform.rotation = Quaternion.identity;
                    
                }
                else
                {
                    cats[currentCatValue].SetActive(false);
                    currentCatValue++;
                    cats[currentCatValue].SetActive(true);
                    mainCat = cats[currentCatValue].gameObject;
                    cats[currentCatValue].transform.position = mainAnchor.transform.position;
                    cats[currentCatValue].transform.parent = mainAnchor.transform;
                    cats[currentCatValue].transform.rotation = Quaternion.identity;                }
                mainCat.transform.position = mainAnchor.transform.position;
            }else
            {
                if (currentCatValue == 0)
                {

                    cats[currentCatValue].SetActive(false);
                    currentCatValue = 2;
                    cats[currentCatValue].SetActive(true);
                    mainCat = cats[currentCatValue].gameObject;
                    cats[currentCatValue].transform.position = mainAnchor.transform.position;
                    cats[currentCatValue].transform.parent = mainAnchor.transform;
                    cats[currentCatValue].transform.rotation = Quaternion.identity;
                    
                }
                else
                {
                    cats[currentCatValue].SetActive(false);
                    currentCatValue--;
                    cats[currentCatValue].SetActive(true);
                    mainCat = cats[currentCatValue].gameObject;
                    cats[currentCatValue].transform.position = mainAnchor.transform.position;
                    cats[currentCatValue].transform.parent = mainAnchor.transform;
                    cats[currentCatValue].transform.rotation = Quaternion.identity;
                    
                }
                mainCat.transform.position = mainAnchor.transform.position;
            }
        }
        UIManager.instance.setAttributes();
        chosencatText.text = currentCatValue.ToString();
            //mainCat.GetComponent<CatAttributes>().CurrentLove.ToString();
    }
    //reset getting the plane
  public void ResetGettingPlane()
    {
        for (int i = 0; i < areaPlanes.Count; i++)
        {
            areaPlanes.RemoveAt(0);
        }
        CurrentGameState = playstates.PlaneSearch;
        cats[currentCatValue].SetActive(false);
   
    }
    void Awake()
    {
        cam = Camera.main;
        instance = this;
        PlaneCountText.text = "starting ";
        StartCoroutine(GameStart());

    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);
        startImage.SetActive(false);
     //   CurrentGameState = playstates.PlaneSearch;
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
            CurrentGameState = playstates.CatPlacement;
            PlaneCountText.text = "count: " + areaPlanes.Count;
            TestingText.text = "plane found";
        }
    }

 
    
    void Update()
    {
       // chosencatText.text = "curr: "+ currentCatValue;
        // Skip the update if ARCore is not tracking
        if (Frame.TrackingState != FrameTrackingState.Tracking)
        {
            return;
        }

        switch (CurrentGameState)
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
            case playstates.EndGame:
                    for(int i=0;i<cats.Count;i++)
                {
                    CatAttributes personality = cats[i].GetComponent<CatAttributes>();
                    if(personality.HappyCat)
                    {
                        chosenCat = i;
                        break;
                    }
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
                CurrentGameState = playstates.InGame;
                TestingText.text = "Ready to play";
                UIManager.instance.EnableGamePlayUI();
               
                //change to work with new cats
                cats[0].gameObject.SetActive(true);
                cats[0].transform.position = hitResult.Point;
                cats[0].transform.parent = mainAnchor.transform;
                cats[0].transform.rotation = Quaternion.identity;
                mainCat = cats[0].gameObject;
                UIManager.instance.setAttributes();
                UIManager.instance.StartTimer();
            }            
        }
    }
}
