﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]

// The script is named PickupParent, and inherits from MonoBehaviour
// This classname ("script") MUST match the name of the file. 

public class PickupParent : MonoBehaviour {

	// Declare your variables at the top!
	// Public variables show up in the Unity editor.
	// But, once you change it in the UI, changing it in the code doesn't tweak it anymore.

    public SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device device;

    public bool objectHeld = false;

    //Used to manually reset a specific ball's position for testing purposes
    //public Transform ball;


    // Use this for *initialization*
    void Awake () {

		// GetComponent is a unity specific function
		// Means this.gameObject.GetComponent
		// This is how we grab our left or right controller

        trackedObj = GetComponent<SteamVR_TrackedObject>();
	
	}
	
	// Update is called once per frame
	void Update () {

		//trackedObj references the actual controller

        device = SteamVR_Controller.Input((int)trackedObj.index);

        ////Reset ball position for testing
        //if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        //{
        //    ball.transform.position = Vector3.zero;
        //    ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;s
        //}

    }

	// OnTriggerStay is a special Unity function
	// It will fire whenever our collider is inside another object

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Throwable"))
        {
            //Debug.Log("Is colliding with hand");
			// "GetPressDown" => determines behavior we want
			// "Trigger" => where we select what actual button that corresponds with

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                //Debug.Log("You are touching down the trigger on an object");
				// isKinematic -> the object can affect others, but they can't affect it (like, dislodge it from our hand)
                col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                col.gameObject.transform.SetParent(gameObject.transform);

                //center held object same location every time
                //col.gameObject.transform.position = gameObject.transform.position;

                objectHeld = true; //is pretty much redundant
            }
            if (objectHeld == true)
            {
                if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                {
					// This seems to work like "console log" in JS 
					// And it expressly for debugging
                    Debug.Log("You have released the trigger");

                    col.gameObject.transform.SetParent(null);
                    col.attachedRigidbody.isKinematic = false;

        
                    objectHeld = false;

                    TossObject(col.attachedRigidbody);
                //col.gameObject.GetComponent<Rigidbody>().velocity = device.velocity;
                //col.gameObject.GetComponent<Rigidbody>().angularVelocity = device.angularVelocity;



                }
            }
        }                     
    }

   //applies force to an object equal to the controller velocity * a multiplier constant
    void TossObject(Rigidbody rigidBody)
    {
 
		rigidBody.velocity = device.velocity;

		// Angular Velocity has to do with spin...
         rigidBody.angularVelocity = device.angularVelocity;
      
    }
}


//Difference between a collider and a trigger...
// Trigger doesn't care about the physics of the other object
// Collider has normal physics