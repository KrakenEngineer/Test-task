using System.Collections.Generic;
using UnityEngine;

public class Player : Movable, IKillable
{
	public static Player Instance { get; private set; }
	private Queue<Vector3Int> _path = new Queue<Vector3Int>();
	private Vector3Int _destination;

	private uint _framesBetweenPathfinds;
	private uint _framesSincePathfind;

	protected override void Start()
	{
		base.Start();
		_destination = Position;
		_framesBetweenPathfinds = (uint)Mathf.FloorToInt(30 / _speed);
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			_destination = VectorUtils.MouseToTilemap;
			_path = PathFinder.FindPath(Position, _destination);
			_framesSincePathfind = 0;
		}

		if (_framesSincePathfind == _framesBetweenPathfinds)
		{
			_path = PathFinder.FindPath(Position, _destination);
			_framesSincePathfind = 0;
		}

		_framesSincePathfind++;
		if (_path.Count == 0 && Position != _destination)
			_destination = Position;
		if (_path.Count == 0 || !_canGo)
			return;
		
		var dest = _path.Dequeue();
		StartCoroutine(Goto(dest));
	}

	private void OnValidate()
	{
		Instance = this;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out IDanger danger))
			Game.Lose();
		if (collision.gameObject.CompareTag("Finish"))
			Game.Win();
	}
}