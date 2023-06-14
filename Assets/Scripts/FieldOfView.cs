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
		// 범위 공격 해주기 
		// 1. 범위 안에 있는지 확인
		// Overlap을 통해 그 안에 공격이 가능한 hittable을 가져오는 방식
		// targetMask를 추가해주면, 충돌체 중에 targetMask가 아니면 안 잡을 것! (?)
		Collider[] colliders = Physics.OverlapSphere(transform.position, range, targetMask);
		foreach (Collider collider in colliders)
		{
			// 2. 각도 안에 있는지 확인
			// 대상까지의 방향성
			Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
			// 내적이 + 앞, - 뒤. -> 내적을 구하는 함수는 Dot임
			if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
				// angle보다 작으면 (Deg2Rad : 각도를 호도법?으로 변환. Cos은 호도법을 써서...)
				continue;       // 안 때리고 넘어감

			// 3. 중간에 장애물이 없는지 확인
			// distToTarget : 레이어는 타겟 거리까지만 쏴주어야 하니까~ 그 뒤까지는 굳이 쏠 필요 없지
			float distToTarget = Vector3.Distance(transform.position, collider.transform.position);
			if (Physics.Raycast(transform.position, dirTarget, distToTarget, obstacleMask))		// 중간에 장애물이 있을 경우
				continue;       // 안 때리고 넘어감

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
