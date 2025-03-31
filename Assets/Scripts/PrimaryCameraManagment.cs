using System;
using System.Collections;
using System.Collections.Generic;
/*using Unity.VisualScripting;*/
using UnityEngine;

public class PrimaryCameraManagment : MonoBehaviour
{
    public List<float> maximumPosition = new();
    public LevelManagment levelManager;
    public List<float> boundryLinePos = new();
    public Transform boundryLine;

    [Header("OFFSETS")]
    public Vector3 insideOffset = Vector3.zero;
    public Vector3 outsideOffset = Vector3.zero;

    // Player componenets referances 
    private Transform protagonistTransform;
    private PlayerLocationSwitch locationSwitchingSystem;

    // Camera's variables 
    private float currentXPosition; 
    private float currentYPosition;
    public bool canFollowProtagonist = false;

    [Header("END POINT")]
    public Transform endPoint;
    public float distanceFromPlayer; 

    // Start is called before the first frame update
    void Start()
    {
        canFollowProtagonist = true; 

        protagonistTransform = GameObject.FindWithTag("Player").transform; 
        locationSwitchingSystem = protagonistTransform.GetComponent<PlayerLocationSwitch>();

        Debug.Log(levelManager.levelNumber);
        boundryLine.position = new Vector2(boundryLinePos[levelManager.levelNumber-1], boundryLine.position.y);
    }

    private void Update()
    {
        // check player current position 
        RangeOfCameraToFollowPlayer();

        // Set position for end point 
        SetEndPoint(); 
    }

    private void LateUpdate()
    {
        FollowPlayerPosition(); 
    }

    private void FollowPlayerPosition()
    {
        if(canFollowProtagonist)
        {
            if (locationSwitchingSystem != null)
            {
                // when player is inside building
                if (!locationSwitchingSystem.isOutside)
                {
                    Vector3 newPosition = new(protagonistTransform.position.x + insideOffset.x,
                                              protagonistTransform.position.y + insideOffset.y, insideOffset.z);
                    AssigningNewPosition(newPosition);
                }
                // when player is outside building
                else
                {
                    Vector3 newPosition = new(protagonistTransform.position.x + outsideOffset.x,
                                              protagonistTransform.position.y + outsideOffset.y, outsideOffset.z);
                    AssigningNewPosition(newPosition);
                }
            }
        }
     /*   else
        {
            if (locationSwitchingSystem != null)
            {
                // when player is inside building
                if (locationSwitchingSystem.isPlayerInHouse)
                {
                    Vector3 newPosition = new(this.transform.position.x + insideOffset.x, protagonistTransform.position.y + insideOffset.y, insideOffset.z);
                    AssigningNewPosition(newPosition);
                }
                // when player is outside building
                else
                {
                    Vector3 newPosition = new(this.transform.position.x + outsideOffset.x, protagonistTransform.position.y + outsideOffset.y, outsideOffset.z);
                    AssigningNewPosition(newPosition);
                }
            }
        }*/
    }

    private void RangeOfCameraToFollowPlayer()
    {
        float currentPlayerXPosition = protagonistTransform.position.x;

        if (locationSwitchingSystem.isOutside)
        {
            // Out of range
            if (currentPlayerXPosition <= 4.2f)
            {
                canFollowProtagonist = false;

                float newXPos = 4.22f; 
                float newYPos = this.transform.position.y; 
                AssigningInstantPosition(newXPos, newYPos);
            }
            else if (currentPlayerXPosition >= maximumPosition[levelManager.levelNumber-1])
            {
                canFollowProtagonist = false;

                float newXPos = maximumPosition[levelManager.levelNumber - 1];
                float newYPos = this.transform.position.y;
                AssigningInstantPosition(newXPos, newYPos);
            }
            else
            {
                canFollowProtagonist = true;
            }
        }
        else
        {
            canFollowProtagonist = true; 
        }
    }
    
    private void AssigningNewPosition(Vector3 _position_)
    {
        this.transform.position = _position_;
    }

    // Change instant position of camera when player transform its location 
    public void AssigningInstantPosition(float newXPosition, float newYPosition)
    {
        Vector3 newPosition = new(newXPosition, newYPosition, outsideOffset.z); 
        AssigningNewPosition(newPosition);  
    }

    private void SetEndPoint()
    {
        Vector2 newPosition = new(protagonistTransform.position.x - distanceFromPlayer, 0);
        endPoint.position = newPosition; 
    }
}
