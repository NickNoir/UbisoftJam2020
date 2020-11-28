using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievment
{

    #region Members

    private string name;
    private string description;
    private bool unlocked;
    private int points;

    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public bool Unlocked { get => unlocked; set => unlocked = value; }
    public int Points { get => points; set => points = value; }



    private GameObject achievementRef;
    private List<Achievment> dependecies = new List<Achievment>();


    private string child;
    public string Child { get => child; set => child = value; }

    #endregion


    #region Constractor
    //controactor
    public Achievment(string name , string description, int points , GameObject achivmentRef)
    {

        this.Name = name;
        this.Description = description;
        this.Unlocked = false;
        this.Points = points;
        this.achievementRef = achivmentRef;
    }
    #endregion


    public void AddDependecy(Achievment dependency)     //Add dependencies to some achiments to make things more intresting :D
    {
        dependecies.Add(dependency);
    }

    public bool EarnAchivment()
    {
        if (!Unlocked && !dependecies.Exists(x => x.unlocked == false))
        {
            Image img = achievementRef.transform.GetChild(3).GetComponent<Image>();
            img.sprite  = AchievmentManager.Instance.unlockedSprite;
            img.gameObject.SetActive(true);

            Unlocked = true;

            if (child != null)
            {
                AchievmentManager.Instance.EarnAchievement(child);
            }

            return true;
        }

        return false;
    }
}
