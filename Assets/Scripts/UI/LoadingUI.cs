using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
	[SerializeField] Slider slider;
	Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void FadeIn()
	{
		anim.SetBool("Active", true);
	}

	public void FadeOut()
	{
		anim.SetBool("Active", false);
	}

	public void SetProgress(float progress)
	{
		slider.value = progress;
	}
}
