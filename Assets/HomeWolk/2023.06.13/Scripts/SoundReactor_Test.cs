using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReactor_Test : MonoBehaviour, IListenable_Test
{
	public void Listen(Transform trans)
	{
		transform.LookAt(trans.position);	// �Ҹ��� ����� �� �ű⸦ �ٶ󺸵���
	}
}
