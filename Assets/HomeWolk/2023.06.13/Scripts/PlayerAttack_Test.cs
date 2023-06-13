using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack_Test : MonoBehaviour
{
	[SerializeField] bool debug;	// 기즈모 껐다켰다 해주기
	[SerializeField] int damage;
	[SerializeField] float range;
	[SerializeField, Range(0, 360)] float angle;    // 공격 각도 (0애서 360도로)
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

	void AttackTiming()		// 범위공격 하기
	{
		// 1. 범위 안에 있는지 확인
		// Overlap을 통해 그 안에 공격이 가능 hittable을 가져오는 방식
		Collider[] colliders = Physics.OverlapSphere(transform.position, range);

		foreach (Collider collider in colliders)
		{
			// 2. 각도 안에 있는지 확인
			// 대상까지의 방향성
			Vector3 dirTarget = (collider.transform.position - transform.position).normalized;

			// 내적이 + 앞, - 뒤.  -> 내적을 구하는 함수는 Dot 이다.
			if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
				// angle보다 작으면 (Deg2Rad : 각도를 호도법으로 변환한다. Cos는 호도법을 쓰기 때문에 변환해준 것)
				continue;	// 안 때리고 넘어감

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
		Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);   // left는 -니까 마이너스로

		Debug.DrawRay(transform.position, rightDir * range, Color.yellow);
		Debug.DrawRay(transform.position, leftDir * range, Color.yellow);
	}

	Vector3 AngleToDir(float angle)
	{
		float radian = angle * Mathf.Deg2Rad;
		return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
	}
}
