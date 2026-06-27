using UnityEngine;

public class Enemy : Movable, IDanger
{
	[SerializeField] private Direction4 _direction;

	private void Update()
	{
		if (!_canGo)
			return;

		var dest = Position + _direction.ToVector();
		if (Game.HasObstacleAt(dest))
		{
			Flip();
			return;
		}
		StartCoroutine(Goto(dest));
	}

	private void Flip()
	{
		_direction = (Direction4)(3 - (int)_direction);
	}
}