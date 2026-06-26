using System.Collections;
using UnityEngine;

public interface IKillable
{
	public GameObject gameObject { get; }

	public void Kill()
	{
		MonoBehaviour.Destroy(gameObject);
	}
}

public interface IDanger { }

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Movable : MonoBehaviour
{
	protected bool _canGo { get; private set; }
	[SerializeField] protected float _speed;

	public Vector3Int Position => transform.position.WorldToTilemap();

	protected virtual void Start()
	{
		_canGo = true;
	}

	protected IEnumerator Goto(Vector3Int pos)
	{
		if (!_canGo)
			yield break;

		_canGo = false;
		float t = 0;
		Vector3 start = transform.position;
		Vector3 dest = pos.TilemapToWorld();

		while (t * _speed < 1)
		{
			transform.position = Vector3.Lerp(start, dest, t * _speed);
			t += Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}

		transform.position = dest;
		_canGo = true;
	}
}

public enum Direction4 : byte
{
	LUp,
	LDown,
	RUp,
	RDown
}