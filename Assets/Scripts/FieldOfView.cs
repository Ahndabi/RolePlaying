using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class FieldOfView : MonoBehaviour
{
	[SerializeField] float range;
	[SerializeField, Range(0f, 360f)] float angle;
	[SerializeField] LayerMask targetMask;
	[SerializeField] LayerMask obstacleMask;

	private void Update()
	{
		FindTarget();
	}

	void FindTarget()
	{
		// ���� ���� ���ֱ� 
		// 1. ���� �ȿ� �ִ��� Ȯ��
		// Overlap�� ���� �� �ȿ� ������ ������ hittable�� �������� ���
		// targetMask�� �߰����ָ�, �浹ü �߿� targetMask�� �ƴϸ� �� ���� ��! (?)
		Collider[] colliders = Physics.OverlapSphere(transform.position, range, targetMask);
		foreach (Collider collider in colliders)
		{
			// 2. ���� �ȿ� �ִ��� Ȯ��
			// �������� ���⼺
			Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
			// ������ + ��, - ��. -> ������ ���ϴ� �Լ��� Dot��
			if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
				// angle���� ������ (Deg2Rad : ������ ȣ����?���� ��ȯ. Cos�� ȣ������ �Ἥ...)
				continue;       // �� ������ �Ѿ

			// 3. �߰��� ��ֹ��� ������ Ȯ��
			// distToTarget : ���̾�� Ÿ�� �Ÿ������� ���־�� �ϴϱ�~ �� �ڱ����� ���� �� �ʿ� ����
			float distToTarget = Vector3.Distance(transform.position, collider.transform.position);
			if (Physics.Raycast(transform.position, dirTarget, distToTarget, obstacleMask))		// �߰��� ��ֹ��� ���� ���
				continue;       // �� ������ �Ѿ

			Debug.DrawRay(transform.position, dirTarget * distToTarget, Color.red);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);

		Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
		Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);

		Debug.DrawRay(transform.position, rightDir * range, Color.yellow);
		Debug.DrawRay(transform.position, leftDir * range, Color.yellow);
	}

	Vector3 AngleToDir(float angle)
	{
		float radian = angle * Mathf.Deg2Rad;
		return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
	}
}
