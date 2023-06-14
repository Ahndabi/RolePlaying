using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractAdaptor_Test : MonoBehaviour, IInteractable_Test
{
	public UnityEvent OnInteract;
	public void Interact()
	{
		OnInteract?.Invoke();
	}
}
