using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class Weapon : MonoBehaviour
{
   public int BuyCost { get; set; }
   public bool IsUnLock { get; set; }
   public int currentLevel{ get; set; }
   public int CurrentLevel
   {
      get
      {
            
         currentLevel = ES3.Load<int>(this.GetType().Name +"Level");
         return currentLevel;

      }
      set
      {
         currentLevel = value;
         ES3.Save<int>(this.GetType().Name +"Level",currentLevel);
      }
   }
   public List<int> levelList{ get; set; }
   public List<int> costList{ get; set; }
   public Image image {get; set; }
   public List<float> inkScaleList{ get; set; }
   public List<int> inkAmoList{ get; set; }
   public List<float> spitFireList{ get; set; }
   public List<int> AbilityLevel{ get; set; }

   public abstract void UnLock();
   public abstract void Upgrade();
   public abstract void Save();
   public abstract void Load();
   public abstract void Setup();
}
