using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatAttributes : MonoBehaviour {

    manager ManagerRef;
    UIManager UIman;
    public bool HappyCat = false;
    public int PetValue;
    public int PetValueMax;
    public int ToyValue;
    public int ToyValueMax;
    public int FoodValue;
    public int FoodValueMax;

    public int setLoveValue;
    public int CurrentLove;

  public int PetButtonPressCount;
   public int ToyButtonPressCount;
   public int FoodButtonPressCount;
    public string catName;

    void Start()
    {
        ManagerRef = manager.instance;
        UIman = UIManager.instance;

        setLoveValue = UIman.SetLoveValue;

        int temp1;
        int temp2;
        int temp3;
        
        temp1 = Random.Range(1, setLoveValue);
     //   print("first number" + temp1);
        FoodValueMax = temp1;
        temp2 = Random.Range(1, (setLoveValue - temp1));
        PetValueMax = temp2;
   //     print("second number" + temp2);
        temp3 = setLoveValue - temp2 - temp1;
        ToyValueMax = temp3;
  //      print("third number" + temp3);

    }
    
    void OnEnable()
    {
       
    }

    public void CheckCatHappiness()
    {
        if (CurrentLove == setLoveValue)
        {
            HappyCat = true;
            UIman.DisableLoveAddUI();
        }
    }

    public void PetCountAdd()
    {
        PetButtonPressCount++;
    }
    public void ToyCountAdd()
    {
        ToyButtonPressCount++;
    }
    public void FoodCountAdd()
    {
        FoodButtonPressCount++;
    }

    void Update()
    {

    }

}
