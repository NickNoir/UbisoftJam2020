using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public string itemName;

    public Sprite sprite;

    public enum CATEGORY
    {
        GREEN,
        BLUE,
        RED
    }

    public CATEGORY itemCatergory = CATEGORY.BLUE;

    [SerializeField]private Color c_color;
    
    private void Start()
    {
        switch(itemCatergory)
        {
            case CATEGORY.GREEN:
            break;
            case CATEGORY.BLUE:
            break;
            case CATEGORY.RED:
            break;
        }
    }


    internal Color getColor => c_color;

    internal void HideItem()
    {
        gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }


}
