using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PooledObjectIndex : int
{
	Enemy  = 0,
	Bullet = 1,
}

[System.Serializable]
public class ObjectPoolItem
{
	public string name;
	public GameObject objectToPool;
	public int initialAmount;
}

[DisallowMultipleComponent]
public class ObjectPooler : MonoBehaviour
{
	public static ObjectPooler Instance;

	[SerializeField] private List<ObjectPoolItem> _itemsToPool;

	private List<List<GameObject>> _pooledObjects = new List<List<GameObject>>();

	// ================== Methods
	
	void Awake()
	{
		Instance = this;
		
		// Assert that for each PooledObjectIndex value, there is one item in _itemsToPool
		int expected = System.Enum.GetValues(typeof(PooledObjectIndex)).Length;
		if (expected != _itemsToPool.Count) throw new System.Exception("Object pooler received invalid number of items.");

		// For each type of thing to pool
		for (int index = 0; index < _itemsToPool.Count; ++index)
		{
			ObjectPoolItem item = _itemsToPool[index];
			_pooledObjects.Add(new List<GameObject>());

			// Pre-instantiate some instances
			for (int i = 0; i < item.initialAmount; ++i)
			{
				GameObject obj = Instantiate(item.objectToPool) as GameObject;
				obj.SetActive(false);
				_pooledObjects[index].Add(obj);
			}
		}
	}

	// Just gets an instance of a pooled object. It's the caller's responsibility to SetActive(true).
	public GameObject GetPooledObject(PooledObjectIndex pooledObjectIndex)
	{
		int index = (int)pooledObjectIndex;

		// Return if invalid index
		if (index >= _pooledObjects.Count) return null;

		// Check for existing instance
		for (int i = 0; i < _pooledObjects[index].Count; ++i)
		{
			if (!_pooledObjects[index][i].activeInHierarchy)
			{
				return _pooledObjects[index][i];
			}
		}

		// Otherwise, make a new one
		GameObject obj = Instantiate(_itemsToPool[index].objectToPool) as GameObject;
		obj.SetActive(false);
		_pooledObjects[index].Add(obj);

		return obj;
	}
}
