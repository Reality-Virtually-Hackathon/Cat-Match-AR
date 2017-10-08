using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatAttributes : MonoBehaviour {

    manager ManagerRef;
    UIManager UIman;
   public int PetValue;
    public int PetValueMax;
   public int ToyValue;
    public int ToyValueMax;
   public int FoodValue;
    public int FoodValueMax;
    public  int Hate = 0;
    public int CurrentLove;

    void OnEnable()
    {
        ManagerRef = manager.instance;
        UIman = UIManager.instance;
       // ManagerRef.mainCat = transform.gameObject;
        Hate = 0;
    }

}
