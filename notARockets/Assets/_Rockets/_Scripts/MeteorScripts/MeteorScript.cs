using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    
    public GameObject ShadowPrefab;

    Ray directionRay;
    RaycastHit MeteorFallOnPlace;
    int mask = 8;

    private Material mat;
    private Vector4 Transparancy;

    public float distance = 150f;

    bool shadowFlag;

    void Start()
    {
        mask = LayerMask.GetMask("Meteor");
    }
    void OnEnable()
    {
        Debug.Log("Meteor is awaked!");
        shadowFlag = true;
        ShadowPrefab.SetActive(false);
    }
    void Update()
    {
        

        directionRay.origin = transform.position;
        directionRay.direction = -transform.forward;
       
        if (Physics.Raycast(directionRay, out MeteorFallOnPlace, distance, mask))
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, MeteorFallOnPlace.normal);
            Vector3 fallingPos = MeteorFallOnPlace.point;
            if (MeteorFallOnPlace.collider.CompareTag("PlanetSurface"))
            {
                
                if (shadowFlag)
                {
                    //Debug.Log("SurfaceDetected and Shadow is active!");
                    ShadowPrefab.SetActive(true);
                    
                    
                    
                    shadowFlag = false;
                }

                ShadowPrefab.transform.position = fallingPos;
                ShadowPrefab.transform.rotation = rot;
            }
            
            //Debug.Log("SomethingtDetected!");
        }
        else
        {
            
        }
        
        
    }
}
