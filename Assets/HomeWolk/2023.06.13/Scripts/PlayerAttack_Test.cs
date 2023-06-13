using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack_Test : MonoBehaviour
{
	[SerializeField] bool debug;	// ����� �����״� ���ֱ�
	[SerializeField] int damage;
	[SerializeField] float range;
	[SerializeField, Range(0, 360)] float angle;    // ���� ���� (0�ּ� 360����)
	Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void OnAttack(InputValue value)
	{
		Attack();
	}

	public void Attack()
	{
		anim.SetTrigger("Attack");
	}

	void AttackTiming()		// �������� �ϱ�
	{
		// 1. ���� �ȿ� �ִ��� Ȯ��
		// Overlap�� ���� �� �ȿ� ������ ���� hittable�� �������� ���
		Collider[] colliders = Physics.OverlapSphere(transform.position, range);

		foreach (Collider collider in colliders)
		{
			// 2. ���� �ȿ� �ִ��� Ȯ��
			// �������� ���⼺
			Vector3 dirTarget = (collider.transform.position - transform.position).normalized;

			// ������ + ��, - ��.  -> ������ ���ϴ� �Լ��� Dot �̴�.
			if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
				// angle���� ������ (Deg2Rad : ������ ȣ�������� ��ȯ�Ѵ�. Cos�� ȣ������ ���� ������ ��ȯ���� ��)
				continue;	// �� ������ �Ѿ

			IHitable_Test hittable = collider.GetComponent<IHitable_Test>();
			hittable?.TakeHit(damage);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if(!debug) 
			return;

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);

		Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
		Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);   // left�� -�ϱ� ���̳ʽ���

		Debug.DrawRay(transform.position, rightDir * range, Color.yellow);
		Debug.DrawRay(transform.position, leftDir * range, Color.yellow);
	}

	Vector3 AngleToDir(float angle)
	{
		float radian = angle * Mathf.Deg2Rad;
		return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
	}
}
