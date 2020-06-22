using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class Tap2018 : MonoBehaviour
{
    public GameObject objectplaced;
    public GameObject specialObject;
    public GameObject placementIndicator;
    private ARRaycastManager ARray;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    public bool SpecialPlacementIsValid = false;


    // Start is called before the first frame update
    // Detect Planes
    void Start()
    {
        ARray = FindObjectOfType<ARRaycastManager>();

    }


    // Trigger to enable special object (aka button)
    public void EnableSpecialObject()
    {
        SpecialPlacementIsValid = true;
    }


    // Update is called once per frame
    // Every frame, update placement indicator and place object

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (placementPoseIsValid && Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (SpecialPlacementIsValid)
            {
                PlaceSpecialObject();
                SpecialPlacementIsValid = false;
            }
            else
            {
                PlaceObject();
            }
        }

    }



    // Instantiate normal object
    private void PlaceObject()
    {
        Instantiate(objectplaced, placementPose.position, placementPose.rotation);
    }


    // Instantiate Special object
    public void PlaceSpecialObject()
    {
        Instantiate(specialObject, placementPose.position, placementPose.rotation);
    }





    // If placement indicator is valid, update its position and rotation
    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }

    }


    // Find camera center and create list of plane objects
    // If there is any plane visible, make placement indicator valid
    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        ARray.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    
}
