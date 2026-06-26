using UnityEngine;

public class Enemy : Movable, IKillable, IDanger
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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.TryGetComponent(out Enemy e))
			(this as IKillable).Kill();
	}

	private void Flip()
	{
		_direction = (Direction4)(3 - (int)_direction);
	}
}