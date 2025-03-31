using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawn : MonoBehaviour
{
    [Header("DISTANCE BETWEEN TWO VEHICLE")]
    public float minDistanceV; 
    public float maxDistanceV;

    [Space]

    [Header("DISTANCE BETWEEN TWO GROUP")]
    public float minDistanceG;
    public float maxDistanceG;

    [Space]

    [Header("VEHICLES")]
    public List<GameObject> vehiclePrefabList = new();

    private List<Transform> vehicleHolderList = new(); // Vehicle holder position , where vehicle spawn by order
    private List<Transform> releasePositions = new(); // Release position , whence each vehicle start moving 

    private List<GameObject> vehicleOfNewGroup = new();// Generated vehicle of new group 
    private List<Transform> releasePositionOfNewGroup = new(); // Choosed release position for each vehicle of new group

    [Space]

    public bool canSpawn = false;
    public bool canCreateGroup = false;

    private Transform playerPosition;
    private Transform vehicleParent; 

    private void Start()
    {
        // Find Vehicle Holders 
        FindVehicleHolders(); 
    }

    private void FindVehicleHolders()
    {
        // First find all vehicle holders 
        Transform ReleasePosition = this.transform.GetChild(1).transform; 
        for (int i = 0; i < ReleasePosition.childCount; i++)
        {
            Transform currentReleasePosition= ReleasePosition.GetChild(i).transform;
            releasePositions.Add(currentReleasePosition);
        }

        // First find all vehicle move release position 
        Transform VehicleHolder = this.transform.GetChild(0).transform; 
        for (int i = 0; i < VehicleHolder.childCount; i++)
        {
            Transform currentVehicleHolder = VehicleHolder.GetChild(i).transform;
            vehicleHolderList.Add(currentVehicleHolder);
        }

        // Find other transform referenece
        playerPosition = GameObject.FindWithTag("Player").transform;
        vehicleParent = GameObject.FindWithTag("Vehicle Parent").transform; 
    }

    // Update is called once per frame
    void Update()
    {
        if(canSpawn)
        {
            if(canCreateGroup)
            {
                canCreateGroup = false;
                GroupOfVehicle(); 
            }
        }

        DistanceFromPlayer(); 
    }

    private void GroupOfVehicle()
    {
        int numbersOfvehicle = Random.Range(1, vehicleHolderList.Count+1);

        for (int i = 0; i < numbersOfvehicle; i++)
        {
            // Create random vehicle for group 
            PickVehicleToSpawn(i); 

            if(i == numbersOfvehicle-1)
            {
                GetReleasePosition(); 
            }
        }
    }

    private void PickVehicleToSpawn(int vehicleHolderNumber)
    {
        // Pick random vehicle from list 
        int randomVehicleNumber = Random.Range(0, vehiclePrefabList.Count);
        GameObject newVehicle = vehiclePrefabList[randomVehicleNumber];

        // Pick holder to hold new vehicle 
        Vector3 spawningPosition = vehicleHolderList[vehicleHolderNumber].position; 

        // Then create it 
        CreateVehicle(newVehicle,spawningPosition); 
    }

    private void CreateVehicle(GameObject vehicle,Vector3 spawnPosition)
    {
        GameObject newVehicle = Instantiate(vehicle,spawnPosition,Quaternion.identity);
        newVehicle.transform.parent = vehicleParent; 
        vehicleOfNewGroup.Add(newVehicle); 
    }

    private void GetReleasePosition()
    {
        foreach(GameObject currentVehicle in vehicleOfNewGroup)
        {
            VehicleMove currentVehicleMovement = currentVehicle.GetComponent<VehicleMove>();    
            if (currentVehicleMovement.vehicleCategory == "a")
            {
                Transform releasePosition = releasePositions[0];
                releasePositionOfNewGroup.Add(releasePosition);
            }
            else if (currentVehicleMovement.vehicleCategory == "b")
            {
                Transform releasePosition = releasePositions[2];
                releasePositionOfNewGroup.Add(releasePosition);
            }
            else if (currentVehicleMovement.vehicleCategory == "c")
            {
                Transform releasePosition = releasePositions[1];
                releasePositionOfNewGroup.Add(releasePosition);
            }
            else if (currentVehicleMovement.vehicleCategory == "d")
            {
                Transform releasePosition = releasePositions[3];
                releasePositionOfNewGroup.Add(releasePosition);
            }
        }

        // Let vehicle move one by one 
        StartCoroutine(LetVehicleMove());
    }

    private IEnumerator LetVehicleMove()
    {
        Debug.Log("Function");
        yield return new WaitForSeconds(0.5f); 

        for (int i = 0; i < vehicleOfNewGroup.Count; i++)
        {
            // Choose vehicle and move it 
            vehicleOfNewGroup[i].transform.position = releasePositionOfNewGroup[i].transform.position;  
            vehicleOfNewGroup[i].GetComponent<VehicleMove>().canMove = true;

            // minDistance 
            if (i < vehicleOfNewGroup.Count - 1)
            {
                float Distance = Random.Range(minDistanceV, maxDistanceV);     
                yield return new WaitForSeconds(Distance);
            }

            if(i == vehicleOfNewGroup.Count - 1)
            {
                StartCoroutine(Restart());
            }
        }
    }

    private IEnumerator Restart()
    {
        // Clear previous group
        vehicleOfNewGroup.Clear();
        releasePositionOfNewGroup.Clear(); 

        // Make distance
        float Distance = Random.Range(minDistanceG,maxDistanceG);
        yield return new WaitForSeconds(Distance);

        //Create another group
        canCreateGroup = true; 
    }

    private void DistanceFromPlayer()
    {
        float spawnerXPos = this.transform.position.x;
        float playerXPos = playerPosition.position.x; ;

        if (playerXPos > spawnerXPos - 20f)
        {
            canSpawn = false;
        }
        else if(playerXPos < spawnerXPos - 70f)
        { 
            canSpawn = false;
        }
        else
        {
            canSpawn = true;
        }
    }
}
