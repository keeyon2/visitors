/*==============================================================================
            Copyright (c) 2012 QUALCOMM Austria Research Center GmbH.
            All Rights Reserved.
            Qualcomm Confidential and Proprietary
==============================================================================*/

using UnityEngine;

// A custom handler that implements the ITrackableEventHandler interface.
public class DefaultTrackableEventHandler : MonoBehaviour,
                                            ITrackableEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES
 
    private TrackableBehaviour mTrackableBehaviour;
    
    #endregion // PRIVATE_MEMBER_VARIABLES



    #region UNTIY_MONOBEHAVIOUR_METHODS
    
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        OnTrackingLost();
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS



    #region PUBLIC_METHODS

    // Implementation of the ITrackableEventHandler function called when the
    // tracking state changes.
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS



    #region PRIVATE_METHODS


    private void OnTrackingFound()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>();

        // Enable rendering:
        foreach (Renderer component in rendererComponents) {
            component.enabled = true;
        }
		
// Automatically get out of play mode
		GameController controller = null;
		GameControllerNetworking networkingController = null;
		
		controller = (GameController)GameObject.Find("ViewObject").GetComponent("GameController");
		if (controller == null) // If in networking mode
		networkingController = (GameControllerNetworking)GameObject.Find("ViewObject").GetComponent("GameControllerNetworking");

		if (controller != null)
			controller.SwitchToPlayingView();
		else if (networkingController != null)
			networkingController.SwitchToPlayingView();
		else
			Debug.Log("Controller null in OnTrackingFound");
		
        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
    }


    private void OnTrackingLost()
    {
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>();

        // Disable rendering:
        foreach (Renderer component in rendererComponents) 
		{
            component.enabled = false;
        }
		
		// Automatically get out of play mode
		GameController controller = null;
		GameControllerNetworking networkingController = null;
		
		controller = (GameController)GameObject.Find("ViewObject").GetComponent("GameController");
		 // If in networking mode
		if (controller == null)
		{
			print("Nathan is over 9000");
			networkingController = (GameControllerNetworking)GameObject.Find("ViewObject").GetComponent("GameControllerNetworking");
		}
		if (controller != null)
			controller.SwitchToSearchingView();
		else if (networkingController != null)
			networkingController.SwitchToSearchingView();
		else
			Debug.Log("Controller null in OnTrackingLost");
		
        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
    }

    #endregion // PRIVATE_METHODS
}
