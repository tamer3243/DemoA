using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler<T> where T: MonoBehaviour, IPooled<T>
{
    private readonly Queue<T> Pool;
    private readonly T _prefab;
    private readonly Transform _parent;

    public readonly List<T> List = new List<T>();


    public ObjectPooler(T prefab, int size)
    {
        Pool = new Queue<T>(size);
        _prefab = prefab;
        _parent = new GameObject().transform;
        for (var i = 0; i < size; i++)
        {
            Pool.Enqueue(Create());
        }
    }


    public ObjectPooler(T prefab, Transform parent, int size)
    {
        Pool = new Queue<T>(size);
        _prefab = prefab;
        _parent = parent;
        for (var i = 0; i < size; i++)
        {
            Pool.Enqueue(Create());
        }
    }

    private T Create()
    {
        var newObj = Object.Instantiate(_prefab, _parent);
        newObj.ReleaseCallback = Release;
        newObj.gameObject.SetActive(false);
        List.Add(newObj);
        return newObj;
    }
    private void Release(T _object)
    {
        _object.gameObject.SetActive(false);
        Pool.Enqueue(_object);
    }


    public T Get()
    {
        if (Pool.Count == 0)
        {
            var newObj = Create();
            Pool.Enqueue(newObj);
        }

        var Obj = Pool.Dequeue();
        Obj.gameObject.SetActive(true);
        return Obj;
    }

    public T Get(Vector3 position)
    {
        if (Pool.Count == 0)
        {
            var newObj = Create();
            Pool.Enqueue(newObj);
        }

        var Obj = Pool.Dequeue();
        Obj.transform.position = position;
        Obj.gameObject.SetActive(true);
        return Obj;
    }


    public T Get(Vector3 position, Quaternion rotation)
    {
        if (Pool.Count == 0)
        {
            var newObj = Create();
            Pool.Enqueue(newObj);
        }

        var Obj = Pool.Dequeue();
        Obj.transform.position = position;
        Obj.transform.rotation = rotation;
        Obj.gameObject.SetActive(true);
        return Obj;
    }


}
