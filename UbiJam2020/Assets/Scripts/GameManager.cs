using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int spaceCounter = 0;

    void Update()
    {

        //Acheivements call test
        if(Input.GetKeyDown(KeyCode.Space) && spaceCounter <=2)
        {
            if(spaceCounter == 0)
                AchievmentManager.Instance.EarnAchievement("Press space");
            else if(spaceCounter == 1)
                AchievmentManager.Instance.EarnAchievement("Press space2");

            else if(spaceCounter == 2)
                AchievmentManager.Instance.EarnAchievement("Press space3");
            spaceCounter++;

        }
    }
}
