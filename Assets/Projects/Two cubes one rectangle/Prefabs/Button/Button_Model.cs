using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Button_Model : MonoBehaviour
{
	public event Action Pressed_Event;
	public UnityEvent Pressed_UnityEvent;

	// This is running on server AND client
	// Do we want that?
    private void OnTriggerEnter(Collider other)
    {
	    Pressed_Event?.Invoke();
	    Pressed_UnityEvent?.Invoke();
    }
}
