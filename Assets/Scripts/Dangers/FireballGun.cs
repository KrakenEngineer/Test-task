using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FireballGun : MonoBehaviour
{
	[SerializeField] private Direction4 _direction;
	[SerializeField] private float _period;
	[SerializeField] private float _lifeTime;
	private float _time;
	private Vector3Int _position;

	private void Start()
	{
		_position = transform.position.WorldToTilemap();
		GetComponent<SpriteRenderer>().sprite = Game.GetFireballGunSprite(_direction);
	}

	private void Update()
	{
		_time += Time.deltaTime;
		if (_time >= _period)
		{
			_time -= _period;
			Fireball.Create(_position, _direction, _lifeTime);
		}
	}
}