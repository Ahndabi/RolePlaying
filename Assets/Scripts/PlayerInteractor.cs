using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
	[SerializeField] bool debug;		// ����� Ȱ��ȭ

	// ��ȣ�ۿ��� �����ϸ� ��ȣ�ۿ��� �� �ְԲ�
	[SerializeField] Transform point;	// ��� �������κ���
	[SerializeField] float range;		// ��� �������� ��ȣ�ۿ��� ������

	public void Interact()
	{
		// �ϴ�, ���� �ȿ� �ִ� �浹ü�� Ȯ��
		Collider[] colliders = Physics.OverlapSphere(point.position, range);
		
		// �� �浹ü�� �߿� Iinteractable �������̽��� �ִٸ�, ��ȣ�ۿ� �ϵ����� 
		foreach ( Collider collider in colliders )
		{
			IInteractable interactable = collider.GetComponent<IInteractable>();
			interactable.Interact();
		}
	}

	void OnInteract(InputValue value)
	{
		Interact();
	}

	private void OnDrawGizmosSelected()
	{
		if (!debug)
			return;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(point.position, range);
	}
}
