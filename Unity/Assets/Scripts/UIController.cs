using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	[SerializeField] RectTransform start;
	[SerializeField] RectTransform results;
	[SerializeField] Text resultsText;
	[SerializeField] Text timeText;

	public float Time { set { timeText.text = "TIME LIMIT: " + TimeToString(value); } }

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
