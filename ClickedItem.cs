using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickedItem : MonoBehaviour
{
    public ToolTip ToolTipRef;
    public Button ItemButton;
    
    //add listener to each item button
    private void Start()
    {
        ItemButton.onClick.AddListener(ButtonCallBack);
    }

    //Get Clicked button info
    // Pass Clicked Button info to OnGameObjectClicked Method
    private void ButtonCallBack()
    {
        ToolTipRef.OnGameObjectClicked(gameObject);
    }
}
