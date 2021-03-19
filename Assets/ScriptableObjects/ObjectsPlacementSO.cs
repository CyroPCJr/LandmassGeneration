using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPlacement", menuName = "Placement")]
public class ObjectsPlacementSO : ScriptableObject
{
    [SerializeField] private int _amount = 100;

    public Elemets[] ElementsList;

    private Dictionary<string, Queue<GameObject>> _objectDictionary;
    private Queue<GameObject> _spawnObjects;
    private GameObject _parentMasterRoot = null;
    public void CreateObjectPooling()
    {
        if (_objectDictionary == null || _objectDictionary.Count == 0)
        {
            _objectDictionary = new Dictionary<string, Queue<GameObject>>();
        }

        if (_parentMasterRoot == null)
        {
            _parentMasterRoot = new GameObject("Object Pooler");
            foreach (var elements in ElementsList)
            {
                Transform elemetParent = new GameObject(elements.Name).transform;
                foreach (var prefabs in elements.prefabs)
                {
                    Transform prefabsParent = new GameObject(prefabs.name).transform;
                    _spawnObjects = new Queue<GameObject>();
                    for (int i = 0; i < _amount; ++i)
                    {
                        GameObject obj = Instantiate(prefabs);
                        obj.SetActive(false);
                        obj.transform.SetParent(prefabsParent);
                        prefabsParent.SetParent(elemetParent);
                        _spawnObjects.Enqueue(obj);
                    }
                    elemetParent.SetParent(_parentMasterRoot.transform);
                    _objectDictionary.Add(prefabs.name, _spawnObjects);
                }
            }
        }
    }

    public GameObject GetObject(string objectName, Vector3 position, Quaternion rotation)
    {
        if (_objectDictionary.TryGetValue(objectName, out var value))
        {
            var obj = value.Dequeue();
            value.Enqueue(obj);
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            return obj;

        }
        return null;
    }

    public GameObject GetRandomObject(Vector3 position, Quaternion rotation)
    {
        string pickupName = "";
        foreach (var item in ElementsList)
        {
            pickupName = item.prefabs[Random.Range(0, item.prefabs.Length)].name;
        }
        return GetObject(pickupName, position, rotation);
    }

    public void RecyclePool()
    {
        if (_objectDictionary != null)
        {
            foreach (var item in _objectDictionary)
            {
                foreach (var elemets in item.Value)
                {
                    if (elemets.activeInHierarchy)
                    {
                        elemets.SetActive(false);
                    }
                }
            }

        }
    }


    [System.Serializable]
    public class Elemets
    {
        public string Name;
        [Range(1, 100)]
        public int density = 1;
        public GameObject[] prefabs;

        public bool CanPlace => Random.Range(0, 100) < density;

        public GameObject GetRandom() => prefabs[Random.Range(0, prefabs.Length)];
    }

}
