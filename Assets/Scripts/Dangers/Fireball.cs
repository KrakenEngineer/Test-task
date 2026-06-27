using UnityEngine;

public class Fireball : Movable, IKillable, IDanger
{
	[SerializeField] private Direction4 _direction;
	private float _lifetime;

	public static void Create(Vector3Int pos, Direction4 direction, float life)
	{
		Object fireball = Instantiate(Game.FirebalPrefab, pos.TilemapToWorld(), Quaternion.identity);
		var result = (fireball as GameObject).GetComponent<Fireball>();
		result._direction = direction;
		result._lifetime = life;
	}

	private void Update()
	{
		_lifetime -= Time.deltaTime;
		if (!_canGo)
			return;

		var dest = Position + _direction.ToVector();
		StartCoroutine(Goto(dest));
		if (_lifetime <= 0)
			(this as IKillable).Kill();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent(out IKillable k))
			k.Kill();
		if (!collision.gameObject.TryGetComponent(out Fireball f) && !collision.gameObject.TryGetComponent(out Player p))
			(this as IKillable).Kill();
	}

	public void Kill() => Destroy(gameObject);
}