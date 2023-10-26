using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    
    private Transform _root;
    
    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }    
    }
    
    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }
        _pool[name].Push(poolable);
    }
    
    //2.풀링된 애가 있을까?에 해당
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);
        return _pool[original.name].Pop(parent);
    }

    private void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;
        
        _pool.Add(original.name, pool);
    }

    //Load<T>(string path) 관련
    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;
        return _pool[name].Original;
    }

    public void Clear()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);
        
        _pool.Clear();
    }
}