using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
	[SerializeField] bool debug;		// 기즈모 활성화

	// 상호작용이 가능하면 상호작용할 수 있게끔
	[SerializeField] Transform point;	// 어디 지점으로부터
	[SerializeField] float range;		// 어느 영역까지 상호작용할 것인지

	public void Interact()
	{
		// 일단, 범위 안에 있는 충돌체들 확인
		Collider[] colliders = Physics.OverlapSphere(point.position, range);
		
		// 그 충돌체들 중에 Iinteractable 인터페이스가 있다면, 상호작용 하도록함 
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
