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
		// ���� �����ϸ� �浹ü�� Ȱ��ȭ��
	}

	public void DisableWeapon()
	{
		coll.enabled = false;
		// ���� ������ �浹ü ��Ȱ��ȭ
	}

	private void OnTriggerEnter(Collider other)
	{
		IHitable hittable = other.GetComponent<IHitable>();
		hittable?.TakeHit(damage);
		// ���ȴµ� �� �ݶ��̴��� hittable�� �ִ� ��� ������ 1 �ֱ�
	}
}
