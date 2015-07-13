using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
	[SerializeField] RectTransform start;
	[SerializeField] RectTransform results;
	[SerializeField] Text resultsText;
	[SerializeField] Text timeLimitText;

	bool flashTimeLimit;

	void Awake()
	{
		StartCoroutine(FlashTimeLimit());
	}

	IEnumerator FlashTimeLimit()
	{
		float factor = 0;
		float direction = 1;
		const float time = 0.2f;

		while (true)
		{
			if (flashTimeLimit)
			{
				factor = Mathf.Clamp01(factor + UnityEngine.Time.deltaTime / time * direction);
				timeLimitText.color = Color.Lerp(Color.white, Color.red, factor);
				
				if (factor == 0 || factor == 1)
				{
					direction = -direction;
				}
			}
			else
			{
				factor = 0;
				direction = 1;
				timeLimitText.color = Color.white;
			}

			yield return new WaitForEndOfFrame();
		}
	}

	public float TimeLimit { set { timeLimitText.text = "TIME LIMIT: " + TimeToString(value); flashTimeLimit = value < 5; } }

	public void ToggleStart(bool toggle)
	{
		start.gameObject.SetActive(toggle);
	}

	public void Success(float time)
	{
		resultsText.text = "FINISHED!\n\nTime: " + TimeToString(time);
		results.gameObject.SetActive(true);
	}
	
	public void Failed(float time)
	{
		if (time < 0)
		{
			resultsText.text = "OUT OF TIME!";
		}
		else
		{
			resultsText.text = "YOU FAILED!";
		}
		results.gameObject.SetActive(true);
	}

	string TimeToString(float time)
	{
		return string.Format("{0:0}:{1:00}.{2:000}", (int)(time / 60f), time % 60f, (time - Mathf.Floor(time)) * 1000f);
	}

	public void OnResultsClicked()
	{
		Application.LoadLevel("Game");
	}
}
