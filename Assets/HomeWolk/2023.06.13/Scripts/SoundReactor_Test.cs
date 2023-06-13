using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReactor_Test : MonoBehaviour, IListenable_Test
{
	public void Listen(Transform trans)
	{
		transform.LookAt(trans.position);	// 소리를 들었을 때 거기를 바라보도록
	}
}
