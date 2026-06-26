using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
	public static Camera MainCamera { get; private set; }
	public static Vector3 MousePos => MainCamera.ScreenToWorldPoint(Input.mousePosition);

	private void Awake()
	{
		MainCamera = GetComponent<Camera>();
	}
}