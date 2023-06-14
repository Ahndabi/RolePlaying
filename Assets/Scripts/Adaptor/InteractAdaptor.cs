using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractAdaptor : MonoBehaviour, IInteractable
{
	public UnityEvent OnInteact;

	public void Interact()
	{
		OnInteact?.Invoke();
	}
}
