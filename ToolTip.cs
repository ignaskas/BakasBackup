using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    private bool _toolTipToggle;
    private ItemButtons ItemButtonsObj { set; get; }
    public Text ToolTipTextArea;

    // toggle tooltip area when item is clicked change the description of the item based on item clicked
    public void OnGameObjectClicked(GameObject clickedItem)
    {
        _toolTipToggle = !_toolTipToggle;
        switch (clickedItem.tag)
        {
            case "KeyButton":
                ItemButtonsObj = new ItemButtons("this is a key");
                ToolTipToggle(ItemButtonsObj, _toolTipToggle);
                break;
            case "SwordButton":
                ItemButtonsObj = new ItemButtons("this is a Sword");
                ToolTipToggle(ItemButtonsObj, _toolTipToggle);
                break;
            case "StaffButton":
                ItemButtonsObj = new ItemButtons("this is a Staff");
                ToolTipToggle(ItemButtonsObj, _toolTipToggle);
                break;
            case "BlasterButton":
                ItemButtonsObj = new ItemButtons("this is a Blaster");
                ToolTipToggle(ItemButtonsObj, _toolTipToggle);
                break;
            case "FlaskEmptyButton":
                ItemButtonsObj = new ItemButtons("this is a empty flask");
                ToolTipToggle(ItemButtonsObj, _toolTipToggle);
                break;
            case "FlaskFullButton":
                ItemButtonsObj = new ItemButtons("this is a full flask");
                ToolTipToggle(ItemButtonsObj, _toolTipToggle);
                break;
            case "CoinButton":
                ItemButtonsObj = new ItemButtons("this is a bag of coins");
                ToolTipToggle(ItemButtonsObj, _toolTipToggle);
                break;
            case "ArmorButton":
                ItemButtonsObj = new ItemButtons("this is armor");
                ToolTipToggle(ItemButtonsObj, _toolTipToggle);
                break;
            default:
                Debug.Log("Wrong Value past to ToolTips");
                break;
        }
    }
    
    // public void CreatToolTips(GameObject clickedItem)
    // {
    // ItemButtons KeyToolTip = new ItemButtons("this is a key", clickedItem);
    // ItemButtonsObj = KeyToolTip;
    // //     ToolTipToggle(KeyToolTip._GameObject, KeyToolTip._toolTipText, _toolTipToggle);
    // }
    
    //This is stupid wy is this even a class?
    public class ItemButtons
    {
        public string _toolTipText;

        public ItemButtons(string toolTipText)
        {
            this._toolTipText = toolTipText;
        }
    }
    
    private void ToolTipToggle(ItemButtons currentItemToolTip, bool toggle)
    {
        ToolTipTextArea.GetComponent<Text>().text =  currentItemToolTip._toolTipText;
        gameObject.SetActive(toggle);
    }
}
