using System.Collections;
using System.Collections.Generic;
/*using Unity.VisualScripting;*/
using UnityEngine;

public class VehicleMove : MonoBehaviour
{
    public string vehicleCategory;
    public bool canMove = false;
    public float distance = 0;
    [Header("VEHICLE MOVE SPEED")]
    public float moveSpeed = 0;    
    [Header("WHEELS ROTATE SPEED")]
    public float wheelSpeed = 0;

    private bool canRotateWheel = false; 
    private Transform endPoint;

    private List<Transform> wheels = new(); 

    // Start is called before the first frame update
    void Start()
    {
        canMove = false;
        canRotateWheel = false; 

        endPoint = GameObject.Find("End Point").transform;

        // find wheels 
        GetWheels(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            MoveVehicle(); 
        }

        if (endPoint != null)
        {
            Distance_from_Spawner();
        }

        if(canRotateWheel)
        {
            RotateWheels(); 
        }
    }

    private void MoveVehicle()
    {
        this.transform.Translate(moveSpeed * Time.deltaTime * Vector3.left); 
    }

    private void Distance_from_Spawner()
    {
        float vehicleXPosition = this.transform.position.x;
        float endPointXPosition = endPoint.position.x;

        if(vehicleXPosition < endPointXPosition-distance)
        {
            Destroy(this.gameObject);
        }
    }

    private void GetWheels()
    {
        Transform vehicleChild = this.transform.GetChild(0).transform; 
        for(int i =1; i < vehicleChild.childCount; i++)
        {
            Transform wheel = vehicleChild.GetChild(i).transform;
            wheels.Add(wheel);

            if(i == vehicleChild.childCount-1)
            {
                Debug.Log("Wheel rotate"); 
                canRotateWheel = true;
            }
        }
    }

    private void RotateWheels()
    {
        foreach(Transform currentWheel in wheels)
        {
            Debug.Log("Rotating"); 
            currentWheel.transform.Rotate(0,0,wheelSpeed * Time.deltaTime); 
        }
    }
}
