using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;		// 이름 겹칠까봐 이렇게 해줌

public class SceneManager : MonoBehaviour
{
	// 씬들을 모두 싱글톤으로 구현하면 문제가 생김
	// 각 씬마다 존재하는 베이스 씬이 필요함
	LoadingUI loadingUI;
	BaseScene curScene;

	// 현재 씬을 가져오고 싶다면!
	public BaseScene CurScene
	{
		get
		{
			// 현재 씬이 없으면 찾아서 주면 되고
			if(curScene == null)
				curScene = GameObject.FindObjectOfType<BaseScene>();

			// 현재 씬이 있다면 찾아놨던 이 현재씬 주면 됨
			return curScene;
		}
	}

	private void Awake()
	{
		LoadingUI ui = Resources.Load<LoadingUI>("UI/LoadingUI");
		loadingUI = Instantiate(ui);
		loadingUI.transform.SetParent(transform);
	}

	public void LoadScene(string sceneName)
	{
		StartCoroutine(LoadingRoutine(sceneName));
	}


	IEnumerator LoadingRoutine(string sceneName)
	{
		loadingUI.FadeOut();
		loadingUI.SetProgress(0f);
		yield return new WaitForSeconds(0.5f);
		Time.timeScale = 0;

		AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
		oper.allowSceneActivation = false;

		// !oper.isDone = 씬 과정이 다 끝나면 true
		while (!oper.isDone)
		{
			loadingUI.SetProgress(Mathf.Lerp(0f, 0.5f, oper.progress));
			yield return null;
		}

		// 추가적인 씬에서 준비할 로딩을 진행하고 넘어가야
		// 몬스터 랜덤배치
		loadingUI.SetProgress(0.6f);
		yield return new WaitForSecondsRealtime(0.5f);	  // timeScale때문에 실제 시간으로!
		// 풀링
		loadingUI.SetProgress(0.7f);
		yield return new WaitForSecondsRealtime(0.5f);
		// 리소스 불러오기
		loadingUI.SetProgress(0.8f);
		yield return new WaitForSecondsRealtime(0.5f);
		// 랜덤 아이템 배치 등
		loadingUI.SetProgress(1f);
		yield return new WaitForSecondsRealtime(0.5f);

		CurScene.LoadAsync();
		while (curScene.progress < 1f)
		{
			loadingUI.SetProgress(Mathf.Lerp(0.5f, 1.0f, CurScene.progress));
			yield return null;
		}

		Time.timeScale = 1f;
		loadingUI.FadeIn();
		yield return new WaitForSeconds(0.5f);
	}
}
