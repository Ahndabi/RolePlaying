using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover_Test : MonoBehaviour
{
	[SerializeField] float walkSpeed;
	[SerializeField] float runSpeed;
	[SerializeField] float jumpSpeed;
	[SerializeField] float walkStepRange;   // 걷는 발소리 범위
	[SerializeField] float runStepRange;    // 뛰는 발소리 범위

	Animator anim;
	CharacterController controller;
	Vector3 moveDir;
	float ySpeed;
	float curSpeed;
	bool walk;
	float lastStepTime = 0.5f;      // 마지막으로 발소리를 낸 시간이 0.5초

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
	}

	private void Update()
	{
		Move();
		Fall();
	}

	void Move()
	{
		// 아무것도 누르지 않았다면? (벡터값이 0이라면 -> 입력값이 없다는 뜻)
		if (moveDir.magnitude == 0)
		{
			curSpeed = Mathf.Lerp(curSpeed, 0, 0.1f);
			anim.SetFloat("MoveSpeed", curSpeed);
			return; 
		}

		// 캐릭터 시점 (카메라랑 시점이 같이 가야한다. y만 제외하고)
		// 백터를 방향으로 쓰려면 크기가 1인 백터를 사용해줄 필요가 있음 (normalied -> 함수도 있음)
		// *방향이면 normalize를 사용해주기*
		Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
		Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

		// 부드럽게 가려면 Lerp(선형보간) 사용
		if (walk)
			curSpeed = Mathf.Lerp(curSpeed, walkSpeed, 0.1f);
		else     // 뛰고있는 경우
			curSpeed = Mathf.Lerp(curSpeed, runSpeed, 0.1f);

		// MoveSpeed는 현재 속도로 정해주기
		anim.SetFloat("MoveSpeed", curSpeed);

		// 보고 있는 방향을 캐릭터의 방향과 같도록 해주기 (누르고 있는 방향을 캐릭터가 바라보게 함)
		Quaternion lookRotation = Quaternion.LookRotation(forwardVec * moveDir.z + rightVec * moveDir.x);
														//				 누르고 있는 방향

		// 바로 회전하지 않고 천천히 회전 (선형보간. Lerp인데 쿼터니언에도 Lerp가 있음
		transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);


		// 캐릭터 무브
		// z, x입력값으로 움직여주기
		controller.Move((forwardVec * moveDir.z + rightVec * moveDir.x) * curSpeed * Time.deltaTime);


		lastStepTime -= Time.deltaTime;
		if (lastStepTime < 0)    	// 0.5초만큼 다 흘렀다면!
		{
			lastStepTime = 0.5f;	// 다시 0.5초로 지정
			GenerateFootStepSound();
		}
	}

	void OnMove(InputValue value)
	{
		moveDir.x = value.Get<Vector2>().x;
		moveDir.z = value.Get<Vector2>().y;
	}
	
	void Jump()
	{
		ySpeed = jumpSpeed;
		//controller.Move(Vector3.up * jumpSpeed * Time.deltaTime);
	}

	void OnJump(InputValue value)
	{
		Jump();
	}

	void Fall()		// 중력을 만들어주는 함수
	{
		ySpeed += Physics.gravity.y * Time.deltaTime;       // 점점 아래로 속력을 받는다. 
		if (controller.isGrounded && ySpeed < 0)	
			ySpeed = 0;
		controller.Move(Vector3.up * ySpeed * Time.deltaTime);

	}

	void OnWalk(InputValue value)
	{
		walk = value.isPressed;
		// isPressed : Pass Through에서 눌렀을 땐 true, 뗐을 땐 false
	}

	void GenerateFootStepSound()	// 발소리
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, walk ? walkStepRange : runStepRange);
		// walk면 walkStepRange만큼, run이면 runStepRange만큼! (삼항연산자)
		foreach (Collider collider in colliders)
		{
			IListenable_Test listenable = collider.GetComponent<IListenable_Test>();
			listenable?.Listen(transform);		// 그 대상이 IListenable이 있는 대상인지 확인
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, walkStepRange);
		Gizmos.DrawWireSphere(transform.position, runStepRange);
	}
}
