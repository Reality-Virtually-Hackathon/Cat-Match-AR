using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;
    public Slider mainSlider;
    CatAttributes CurrentCatStats;
    public int TimerCount;
    public int TimerLimit;
    public List<GameObject> UIPlayObjects = new List<GameObject>();
    public int FeelingsIncValue = 5;

    public void EnableGamePlayUI()
    {
        for (int i = 0; i < UIPlayObjects.Count; i++)
        {
            UIPlayObjects[i].SetActive(true);
        }
    }

  public  void ChangeCat()
    {
        manager.instance.ChangeOutCat(1);
        CurrentCatStats = manager.instance.mainCat.GetComponent<CatAttributes>();
        mainSlider.value = CurrentCatStats.CurrentLove;
    }

   public void AddPetting()
    {
        if(CurrentCatStats.PetValue<CurrentCatStats.PetValueMax)
        {
            CurrentCatStats.PetValue+= FeelingsIncValue;
            mainSlider.value+= FeelingsIncValue;
            CurrentCatStats.CurrentLove = (int)mainSlider.value;
        }
        else
        {
            CurrentCatStats.PetValue -= FeelingsIncValue;
            mainSlider.value -= FeelingsIncValue;
            CurrentCatStats.CurrentLove = (int)mainSlider.value;
        }
    }

   public void AddFood()
    {
        if(CurrentCatStats.FoodValue<CurrentCatStats.FoodValueMax)
        {
            CurrentCatStats.FoodValue+= FeelingsIncValue;
            mainSlider.value+=FeelingsIncValue;
            CurrentCatStats.CurrentLove = (int)mainSlider.value;
        }
        else
        {
            CurrentCatStats.FoodValue -= FeelingsIncValue;
            mainSlider.value -= FeelingsIncValue;
            CurrentCatStats.CurrentLove = (int)mainSlider.value;
        }
    }

   public void AddToy()
    {
        if (CurrentCatStats.ToyValue < CurrentCatStats.ToyValueMax)
        {
            CurrentCatStats.ToyValue+=FeelingsIncValue;
            mainSlider.value+= FeelingsIncValue;
            CurrentCatStats.CurrentLove = (int)mainSlider.value;
        }
        else
        {
            CurrentCatStats.ToyValue -= FeelingsIncValue;
            mainSlider.value -= FeelingsIncValue;
            CurrentCatStats.CurrentLove = (int)mainSlider.value;
        }
    }


    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(manager.instance.playerStates == manager.playstates.InGame)
        {

        }
    }
    
}
