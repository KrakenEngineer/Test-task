using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Builder : MonoBehaviour
{
	public static Tilemap Tilemap { get; private set; }
	[SerializeField] private TileBase _obstacle;

	private void Awake()
	{
		Tilemap = GetComponent<Tilemap>();
	}

	private void Update()
	{
		if (!Input.GetMouseButtonDown(1))
			return;

		Vector3Int mousePos = VectorUtils.MouseToTilemap + new Vector3Int(0, 0, 5);
		if (CanToggleAt(mousePos))
			Toggle(mousePos);
	}

	private bool CanToggleAt(Vector3Int pos)
	{
		var positions = FindObjectsByType<Movable>(FindObjectsSortMode.None).Select(m => m.Position);
		return Game.HasObstacleAt(pos) || !positions.Where(p => p.Approximately(pos)).Any();
	}

	private void Toggle(Vector3Int pos)
	{
		if (!PathFinder.TryToggle(pos.x, pos.y))
			return;
		Game.TryToggle(pos.x, pos.y);
		var tile = Tilemap.GetTile(pos) == null ? _obstacle : null;
		Tilemap.SetTile(pos, tile);
	}
}