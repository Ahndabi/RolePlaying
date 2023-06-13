using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Test : MonoBehaviour
{
	[SerializeField] int damage;

	Collider coll;

	private void Awake()
	{
		coll = GetComponent<Collider>();
	}

	public void EnableWeapon()
	{
		coll.enabled = true;
		// 공격 시작하면 충돌체가 활성화됨
	}

	public void DisableWeapon()
	{
		coll.enabled = false;
		// 공격 끝나면 충돌체 비활성화
	}

	private void OnTriggerEnter(Collider other)
	{
		IHitable hittable = other.GetComponent<IHitable>();
		hittable?.TakeHit(damage);
		// 때렸는데 그 콜라이더가 hittable이 있는 경우 데미지 1 주기
	}
}
