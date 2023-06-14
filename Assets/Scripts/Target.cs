using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IHitable
{
	public void TakeHit(int damage)
	{
		Destroy(gameObject);
		// 데미지 받으면 죽는 거로 구현~
	}
}
