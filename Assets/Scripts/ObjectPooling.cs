using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] private GameObject _preFabs;
    [SerializeField] private int _amount = 0;

    private Queue<GameObject> _spawnObjects;
    private Transform _parentTransform = null;

    public void SpawnObjectPool()
    {
        if (_spawnObjects == null || _spawnObjects.Count == 0)
        {
            _spawnObjects = new Queue<GameObject>();
        }

        if (_spawnObjects.Count >= _amount) return;

        if (!_parentTransform)
        {
            _parentTransform = new GameObject(name).transform;
        }

        while (_spawnObjects.Count < _amount)
        {
            GameObject obj = Instantiate(_preFabs, _parentTransform);
            obj.SetActive(false);
            _spawnObjects.Enqueue(obj);
        }
    }

    public GameObject GetPoolledObject(Vector3 position, Quaternion rotation)
    {
        if (_spawnObjects == null || _spawnObjects.Count == 0)
        {
            SpawnObjectPool();
            Debug.LogWarning($"<color=red> Considere spawning it at the beginning of the game.</color>");
        }

        GameObject obj = _spawnObjects.Dequeue();
        _spawnObjects.Enqueue(obj);

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void RecylcePool()
    {
        foreach (var item in _spawnObjects)
        {
            item.SetActive(false);
        }
    }

}

