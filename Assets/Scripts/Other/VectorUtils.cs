using UnityEngine;

public static class VectorUtils
{
	private static Vector3Int[] _Direction4ToVector = {
		Vector3Int.up,
		Vector3Int.left,
		Vector3Int.right,
		Vector3Int.down
	}; 

	public static Vector3Int MouseToTilemap =>
		PlayerCamera.MousePos.WorldToTilemap();

	public static Vector3Int WorldToTilemap(this Vector3 pos) =>
		Builder.Tilemap.WorldToCell(pos);

	public static Vector3 TilemapToWorld(this Vector3Int pos) =>
		Builder.Tilemap.GetCellCenterWorld(pos);

	public static Vector3Int ToVector(this Direction4 d) =>
		_Direction4ToVector[(int)d];

	public static bool Approximately(this Vector3Int v1, Vector3Int v2) =>
		-1 <= v1.x - v2.x && v1.x - v2.x <= 1 &&  -1 <= v1.y - v2.y && v1.y - v2.y <= 1;
}