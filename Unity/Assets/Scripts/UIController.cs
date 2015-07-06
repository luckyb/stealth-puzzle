using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	[SerializeField] RectTransform results;
	[SerializeField] Text resultsText;

	public void Success()
	{
		resultsText.text = "SUCCESS";
		results.gameObject.SetActive(true);
	}
	
	public void Failed()
	{
		resultsText.text = "FAILED";
		results.gameObject.SetActive(true);
	}

	public void OnResultsClicked()
	{
		Application.LoadLevel("Game");
	}
}
