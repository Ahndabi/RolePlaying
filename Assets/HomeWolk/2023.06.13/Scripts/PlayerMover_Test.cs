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
	[SerializeField] float walkStepRange;   // �ȴ� �߼Ҹ� ����
	[SerializeField] float runStepRange;    // �ٴ� �߼Ҹ� ����

	Animator anim;
	CharacterController controller;
	Vector3 moveDir;
	float ySpeed;
	float curSpeed;
	bool walk;
	float lastStepTime = 0.5f;      // ���������� �߼Ҹ��� �� �ð��� 0.5��

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
		// �ƹ��͵� ������ �ʾҴٸ�? (���Ͱ��� 0�̶�� -> �Է°��� ���ٴ� ��)
		if (moveDir.magnitude == 0)
		{
			curSpeed = Mathf.Lerp(curSpeed, 0, 0.1f);
			anim.SetFloat("MoveSpeed", curSpeed);
			return; 
		}

		// ĳ���� ���� (ī�޶�� ������ ���� �����Ѵ�. y�� �����ϰ�)
		// ���͸� �������� ������ ũ�Ⱑ 1�� ���͸� ������� �ʿ䰡 ���� (normalied -> �Լ��� ����)
		// *�����̸� normalize�� ������ֱ�*
		Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
		Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

		// �ε巴�� ������ Lerp(��������) ���
		if (walk)
			curSpeed = Mathf.Lerp(curSpeed, walkSpeed, 0.1f);
		else     // �ٰ��ִ� ���
			curSpeed = Mathf.Lerp(curSpeed, runSpeed, 0.1f);

		// MoveSpeed�� ���� �ӵ��� �����ֱ�
		anim.SetFloat("MoveSpeed", curSpeed);

		// ���� �ִ� ������ ĳ������ ����� ������ ���ֱ� (������ �ִ� ������ ĳ���Ͱ� �ٶ󺸰� ��)
		Quaternion lookRotation = Quaternion.LookRotation(forwardVec * moveDir.z + rightVec * moveDir.x);
														//				 ������ �ִ� ����

		// �ٷ� ȸ������ �ʰ� õõ�� ȸ�� (��������. Lerp�ε� ���ʹϾ𿡵� Lerp�� ����
		transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);


		// ĳ���� ����
		// z, x�Է°����� �������ֱ�
		controller.Move((forwardVec * moveDir.z + rightVec * moveDir.x) * curSpeed * Time.deltaTime);


		lastStepTime -= Time.deltaTime;
		if (lastStepTime < 0)    	// 0.5�ʸ�ŭ �� �귶�ٸ�!
		{
			lastStepTime = 0.5f;	// �ٽ� 0.5�ʷ� ����
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

	void Fall()		// �߷��� ������ִ� �Լ�
	{
		ySpeed += Physics.gravity.y * Time.deltaTime;       // ���� �Ʒ��� �ӷ��� �޴´�. 
		if (controller.isGrounded && ySpeed < 0)	
			ySpeed = 0;
		controller.Move(Vector3.up * ySpeed * Time.deltaTime);

	}

	void OnWalk(InputValue value)
	{
		walk = value.isPressed;
		// isPressed : Pass Through���� ������ �� true, ���� �� false
	}

	void GenerateFootStepSound()	// �߼Ҹ�
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, walk ? walkStepRange : runStepRange);
		// walk�� walkStepRange��ŭ, run�̸� runStepRange��ŭ! (���׿�����)
		foreach (Collider collider in colliders)
		{
			IListenable_Test listenable = collider.GetComponent<IListenable_Test>();
			listenable?.Listen(transform);		// �� ����� IListenable�� �ִ� ������� Ȯ��
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, walkStepRange);
		Gizmos.DrawWireSphere(transform.position, runStepRange);
	}
}
