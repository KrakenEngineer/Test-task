using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
	public const int Size = Game.LevelSize;
	public const float Sqrt2 = 1.418f;
	public const int MaxLen = Size * Size;
	private static int[,] _prices = new int[Size, Size];

	public static void Init()
	{
		for (int i = 0; i < Size; i++)
			for (int j = 0; j < Size; j++)
				_prices[i, j] = 1;
	}

	public static Queue<Vector3Int> FindPath(Vector3Int p1, Vector3Int p2)
	{
		int x1 = p1.x, y1 = p1.y, x2 = p2.x, y2 = p2.y;
		if (InvalidPoints(x1, y1, x2, y2) || p1 == p2)
			return new Queue<Vector3Int>();

		float[,] pathlen = FillPathlen(p1);
		Vector3Int[,] from = FillFrom(p1);
		var queue = new Queue<Vector3Int>();
		queue.Enqueue(new Vector3Int(x1, y1));

		while (queue.Count != 0)
		{
			var current = queue.Dequeue();
			Vector3Int[] tilesAround = TilesAround(current);
			foreach (var tile in tilesAround)
			{
				float dp = PriceBetween(current, tile);
				var curPathlen = pathlen[tile.x, tile.y];
				if (curPathlen == -1)
					queue.Enqueue(tile);
				if (curPathlen != -1 && curPathlen < pathlen[current.x, current.y] + dp)
					continue;
				pathlen[tile.x, tile.y] = pathlen[current.x, current.y] + dp;
				from[tile.x, tile.y] = current;
			}
		}
		float len = pathlen[x2, y2];
		if (len > MaxLen)
			return new Queue<Vector3Int>();

		var path = new List<Vector3Int>();
		var end = new Vector3Int(x2, y2);
		while (end != from[end.x, end.y])
		{
			path.Add(end);
			end = from[end.x, end.y];
		}
		path.Reverse();
		return new Queue<Vector3Int>(path);
	}

	private static float PriceBetween(Vector3Int p1, Vector3Int p2)
	{
		int x1 = p1.x, y1 = p1.y, x2 = p2.x, y2 = p2.y;
		float priceSum = _prices[x1, y1] + _prices[x2, y2] + _prices[x1, y2] + _prices[x2, y1];
		if (p1.x != p2.x && p1.y != p2.y)
			priceSum *= Sqrt2;
		return priceSum / 4;
	}

	private static Vector3Int[] TilesAround(Vector3Int tile)
	{
		var tiles = new Vector3Int[8];
		tiles[0] = tile + Vector3Int.up;
		tiles[1] = tile + Vector3Int.down;
		tiles[2] = tile + Vector3Int.left;
		tiles[3] = tile + Vector3Int.right;
		tiles[4] = tiles[0] + Vector3Int.right;
		tiles[5] = tiles[0] + Vector3Int.left;
		tiles[6] = tiles[1] + Vector3Int.left;
		tiles[7] = tiles[1] + Vector3Int.right;

		var tilesAround = new List<Vector3Int>();
		foreach (var t in tiles)
			if (!OutOfBounds(t.x, t.y))
				tilesAround.Add(t);
		return tilesAround.ToArray();
	}

	private static float[,] FillPathlen(Vector3Int p1)
	{
		var pathlen = new float[Size, Size];
		for (int i = 0; i < Size; i++)
			for (int j = 0; j < Size; j++)
				pathlen[i, j] = -1;
		pathlen[p1.x, p1.y] = 0;
		return pathlen;
	}

	private static Vector3Int[,] FillFrom(Vector3Int p1)
	{
		var from = new Vector3Int[Size, Size];
		for (int i = 0; i < Size; i++)
			for (int j = 0; j < Size; j++)
				from[i, j] = -Vector3Int.one;
		from[p1.x, p1.y] = p1;
		return from;
	}

	private static bool OutOfBounds(int x, int y) =>
		x < 0 || y < 0 || x >= Size || y >= Size;
	private static bool InvalidPoints(int x1, int y1, int x2, int y2) =>
		OutOfBounds(x1, y1) || _prices[x1, y1] != 1 || OutOfBounds(x2, y2) || _prices[x2, y2] != 1;

	public static bool TryToggle(int x, int y)
	{
		if (OutOfBounds(x, y))
			return false;
		_prices[x, y] = _prices[x, y] == 1 ? 3000 : 1;
		return true;
	}
}