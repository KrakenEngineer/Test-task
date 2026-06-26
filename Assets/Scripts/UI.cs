using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UI : MonoBehaviour
{
	public static UI Instance { get; private set; }
	[SerializeField] private GameObject _winImage;
	[SerializeField] private GameObject _loseImage;

	private void OnValidate()
	{
		if (Instance != null && Instance != this)
			throw new System.Exception("There is only ONE UI");
		Instance = this;
	}

	private void Start()
	{
		_winImage.SetActive(false);
		_loseImage.SetActive(false);
	}

	public static void Win()
	{
		Instance._winImage.SetActive(true);
	}

	public static void Lose()
	{
		Instance._loseImage.SetActive(true);
	}
}