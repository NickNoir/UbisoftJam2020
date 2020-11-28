using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievmentManager : MonoBehaviour
{
    [SerializeField]private GameObject achievementPanel;

    [SerializeField]private GameObject achievementPrefab;

    [SerializeField]private GameObject visualachievement;

    public Sprite unlockedSprite;

    [SerializeField]private Text bunosPoint;

    [SerializeField]private int bonusPoints = 0;

    private Dictionary<string , Achievment> _achievementDict = new Dictionary<string , Achievment>();

    [SerializeField]private float fadeTime = 1.5f;



    #region Singleton
    private static AchievmentManager instance;
    //singleton
    public static AchievmentManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<AchievmentManager>();
            }
            return AchievmentManager.instance;
        }

    }
    #endregion






    private void Awake()
    {
        achievementPanel.gameObject.SetActive(true);
        bonusPoints = 0;

        #region ACHIEVMENTS
        ////Emotions achivments
        ///
        //parent/title/discription/points/dependecy
        CreateAchivment("AchievmentContentList" , "Press space" , " press space 1/1" , 10);
        CreateAchivment("AchievmentContentList" , "Press space2" , " press space 2/2" , 10);
        CreateAchivment("AchievmentContentList" , "Press space3" , " press space 3/3" , 10);
        CreateAchivment("AchievmentContentList" , "Space bar master" , " Space bar master" , 10 ,new string[] { "Press space3" });

        #endregion

    }
    void Start()
    {
        achievementPanel.gameObject.SetActive(false);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            achievementPanel.gameObject.SetActive(!achievementPanel.gameObject.activeSelf);

        }
        bunosPoint.text = "x" + bonusPoints.ToString();
    }

    //call this function to earn achivments
    public void EarnAchievement(string title)
    {
        if (_achievementDict[title].EarnAchivment())
        {
            bonusPoints += _achievementDict[title].Points;

            //DO Something
            GameObject achivment = (GameObject)Instantiate(visualachievement);

            SetAchievmentInfo("EarnCanvas", achivment, title);


            StartCoroutine(FadeAchivment(achivment));
        }
    }

    public void CreateAchivment(string parent, string title, string description, int points, string[] dependecies = null)
    {


        GameObject achivment = (GameObject)Instantiate(achievementPrefab);

        Achievment newAchivment = new Achievment(name, description, points, achivment);

        _achievementDict.Add(title, newAchivment);

        SetAchievmentInfo(parent, achivment, title);

        if (dependecies != null)
        {
            foreach (string achivmentTitle in dependecies)
            {
                Achievment dependecy = _achievementDict[achivmentTitle];
                dependecy.Child = title;
                newAchivment.AddDependecy(dependecy);

                //Dependecy = press space <-- child = press w
                //new achivment = press w --> press space
            }
        }
    }

    public void SetAchievmentInfo(string parent, GameObject achivment, string title)
    {
       // Debug.Log(GameObject.Find(parent).name);
        achivment.transform.SetParent(GameObject.Find(parent).transform);
        achivment.transform.localScale = new Vector3(1, 1, 1);

        achivment.transform.GetChild(0).GetComponent<Text>().text = title.ToString();
        achivment.transform.GetChild(1).GetComponent<Text>().text = _achievementDict[title].Description.ToString();
        achivment.transform.GetChild(2).GetComponent<Text>().text = _achievementDict[title].Points.ToString();
        //achivment.transform.GetChild(3).GetComponent<Sprite>().sprite = set the icon
    }

    private IEnumerator FadeAchivment(GameObject achivment)
    {
        CanvasGroup canvasGroup = achivment.GetComponent<CanvasGroup>();

        float rate = 1.0f / fadeTime;

        int startAlpa = 0;
        int endAlpha = 1;



        for (int i = 0; i < 2; i++)
        {
            float progress = 0f;
            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpa, endAlpha, progress);
                progress += rate * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(fadeTime);

            startAlpa = 1;
            endAlpha = 0;
        }

        Destroy(achivment);
    }

}
