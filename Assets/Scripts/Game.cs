using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public static Game Instance { get; private set; }
	public static Object FirebalPrefab => Instance._fireball;
	public static GameState State { get; private set; }

	public const int LevelSize = 30;
	private static bool[,] _level = new bool[LevelSize, LevelSize];

	[SerializeField] private Object _fireball;
	[SerializeField] private Sprite[] _fireballGunSprites;

	private static Player _player;

	private void OnValidate()
	{
		if (Instance != null && Instance != this)
			throw new System.Exception("There is only ONE game");
		Instance = this;
	}

	private void Start()
	{
		Instance = this;
		PathFinder.Init();
		Time.timeScale = 1;
		State = GameState.Play;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(0);
	}

	public static Sprite GetFireballGunSprite(Direction4 direction) =>
		Instance._fireballGunSprites[(int)direction];

	public static void Win()
	{
		if (State != GameState.Play)
			return;
		State = GameState.Won;
		((IKillable)Player.Instance)?.Kill();
		UI.Win();
		Time.timeScale = 0;
	}

	public static void Lose()
	{
		if (State != GameState.Play)
			return;
		State = GameState.Lost;
		((IKillable)Player.Instance)?.Kill();
		UI.Lose();
		Time.timeScale = 0;
	}

	public static bool HasObstacleAt(Vector3Int position) =>
		OutOfBounds(position.x, position.y) || _level[position.x, position.y];

	public static bool TryToggle(int x, int y)
	{
		if (OutOfBounds(x, y))
			return false;
		_level[x, y] ^= true;
		return true;
	}

	private static bool OutOfBounds(int x, int y) =>
		x < 0 || y < 0 || x >= LevelSize || y >= LevelSize;
}

public enum GameState
{
	Play,
	Won,
	Lost
}