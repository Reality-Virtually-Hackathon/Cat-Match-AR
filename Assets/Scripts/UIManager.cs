using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;
    manager manRef;
    public Slider mainSlider;
    CatAttributes CurrentCatStats;
    public int TimerCount;
    public int TimerLimit;
    public Text TimerText;

    bool GamePlayUIEnabled = false;
    public List<GameObject> UIPlayObjects = new List<GameObject>();
    public List<GameObject> UILoveObjects = new List<GameObject>();
    public List<GameObject> UIEndGameObjects = new List<GameObject>();
    public int FeelingsIncValue = 5;
    public GameObject EndingGameUI;
    public int SetLoveValue = 100;

    public Text chosenCatName;
    public List<Text> chosenCatStat = new List<Text>();
  
    public void EnableGamePlayUI()
    {
       
        GamePlayUIEnabled = true;
        for (int i = 0; i < UIPlayObjects.Count; i++)
        {
            UIPlayObjects[i].SetActive(true);
        }
    }

    public void setAttributes()
    {
        CurrentCatStats = manager.instance.mainCat.GetComponent<CatAttributes>();
    }

    public void DisableGamePlayUI()
    {
        for (int i = 0; i < UIPlayObjects.Count; i++)
        {
            UIPlayObjects[i].SetActive(false);
        }
        GamePlayUIEnabled = false;
    }

    public void DisableLoveAddUI()
    {
        for (int i = 0; i < UIPlayObjects.Count; i++)
        {
            UILoveObjects[i].SetActive(false);
        }
    }

    public void EnableLoveAddUI()
    {
        for (int i = 0; i < UIPlayObjects.Count; i++)
        {
            UILoveObjects[i].SetActive(true);
        }
    }

    public void ChangeCatLeft()
    {
        manager.instance.ChangeOutCat(0);

       // CurrentCatStats = manager.instance.mainCat.GetComponent<CatAttributes>();


        //     SoundManager.instance.happyCatSound();
        if (CurrentCatStats.HappyCat)
        {
            DisableLoveAddUI();
        }
        else
        {
            EnableLoveAddUI();
        }
        mainSlider.value = CurrentCatStats.CurrentLove;
    }
    public void ChangeCatRight()
    {
        
        manager.instance.ChangeOutCat(1);

      //  CurrentCatStats = manager.instance.mainCat.GetComponent<CatAttributes>();
        //   SoundManager.instance.happyCatSound();
        if (CurrentCatStats.HappyCat)
        {
            DisableLoveAddUI();
        }
        else
        {
            EnableLoveAddUI();
        }
        mainSlider.value = CurrentCatStats.CurrentLove;
    }

   public void AddPetting()
    {

        if(CurrentCatStats.PetValue<CurrentCatStats.PetValueMax)
        {
            CurrentCatStats.PetValue+= FeelingsIncValue;
            mainSlider.value+= FeelingsIncValue;
       //     SoundManager.instance.happyCatSound();
        }
        else
        {
       //     SoundManager.instance.hungryCatSound();
            CurrentCatStats.PetValue -= FeelingsIncValue;
            mainSlider.value -= FeelingsIncValue;
           
        }
        CurrentCatStats.CurrentLove = (int)mainSlider.value;
        CurrentCatStats.PetCountAdd();
        CurrentCatStats.CheckCatHappiness();
    }

   public void AddFood()
    {
        if(CurrentCatStats.FoodValue<CurrentCatStats.FoodValueMax)
        {
            CurrentCatStats.FoodValue+= FeelingsIncValue;
            mainSlider.value+=FeelingsIncValue;
       ///     SoundManager.instance.hungryCatSound();
        
        }
        else
        {
        //    SoundManager.instance.hungryCatSound();
            CurrentCatStats.FoodValue -= FeelingsIncValue;
            mainSlider.value -= FeelingsIncValue;
          
        }

        CurrentCatStats.CurrentLove = (int)mainSlider.value;
        CurrentCatStats.FoodCountAdd();
        CurrentCatStats.CheckCatHappiness();
    }

   public void AddToy()
    {
        if (CurrentCatStats.ToyValue < CurrentCatStats.ToyValueMax)
        {
            CurrentCatStats.ToyValue+=FeelingsIncValue;
            mainSlider.value+= FeelingsIncValue;
       //     SoundManager.instance.happyCatSound();
        }
        else
        {
        //    SoundManager.instance.angryCatSound();
            CurrentCatStats.ToyValue -= FeelingsIncValue;
            mainSlider.value -= FeelingsIncValue;
           
        }
        CurrentCatStats.CurrentLove = (int)mainSlider.value;
        CurrentCatStats.ToyCountAdd();
        CurrentCatStats.CheckCatHappiness();
    }


    void Awake()
    {
        instance = this;
        manRef = GetComponent<manager>();
        mainSlider.maxValue = SetLoveValue;
    }

    void Update()
    {
 
    }
    
    public void StartTimer()
    {
        StartCoroutine(CountDown());
    }   
    
    IEnumerator CountDown()
    {
        TimerCount = TimerLimit;
        for (int i = TimerLimit; i > 0; i--)
        {
            TimerText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        DisableGamePlayUI();
        manager.instance.CurrentGameState = manager.playstates.EndGame;
        int count = manager.instance.cats.Count;
        for(int i=0;i<count;i++)
        {
            manager.instance.cats[i].SetActive(false);
        }
        yield return new WaitForSeconds(2f);
        EnableEndGameUI();
    } 

    void EnableEndGameUI()
    {
        EndingGameUI.SetActive(true);
        manager.instance.cats[manager.instance.chosenCat].SetActive(true);
        CatAttributes stats = manager.instance.cats[manager.instance.chosenCat].GetComponent<CatAttributes>();
        chosenCatStat[0].text = stats.PetButtonPressCount.ToString();
        chosenCatStat[1].text = stats.ToyButtonPressCount.ToString();
        chosenCatStat[2].text = stats.FoodButtonPressCount.ToString();
        chosenCatName.text = stats.catName;
    }

}
