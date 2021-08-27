using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;

    public float transitionTime = 1f;

	public void LoadNextLevel(int buildIndex)
	{
		StartCoroutine(LoadLevel(buildIndex));
	}

	public IEnumerator LoadLevel(int buildIndex)
	{
		transition.SetTrigger("Start");

		yield return new WaitForSeconds(transitionTime);

		SceneManager.LoadScene(buildIndex);
	}
}
