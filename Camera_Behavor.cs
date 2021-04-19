using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = System.Random;

public class Camera_Behavor : MonoBehaviour
{
    public GameObject playerPosition;
    public Camera myCamera;

    private Vector3 _cameraPos;

    public GameObject cameraStopLeft;
    public GameObject cameraStopRight;
    public GameObject cameraStopTop;
    public GameObject cameraStopBot;
    private int number;
    
    private void FixedUpdate()
    {
        _cameraPos = myCamera.transform.position; // store the camera position
        if (cameraStopTop.transform.position.y >= 15.34f | cameraStopBot.transform.position.y <= -15.43f && cameraStopLeft.transform.position.x <= -20.476f | cameraStopRight.transform.position.x >= 20.476f)
        {
            //stop the camera following the player
        }
        else if (cameraStopTop.transform.position.y >= 15.34f | cameraStopBot.transform.position.y <= -15.43f) // this is Y
        {
            _cameraPos.x = playerPosition.transform.position.x; // Change the X position of the camera to player position
            myCamera.transform.position = _cameraPos; // move the camera to the new position
        }else if (cameraStopLeft.transform.position.x <= -20.476f | cameraStopRight.transform.position.x >= 20.476f) //  this is X
        {
            _cameraPos.y = playerPosition.transform.position.y; // Change the Y position of the camera to player position
            myCamera.transform.position = _cameraPos; // move the camera to the new position
        }else
        {
            transform.position = new Vector3(playerPosition.transform.position.x, playerPosition.transform.position.y, transform.position.z); // update camera position based on player position
        }
    }
}