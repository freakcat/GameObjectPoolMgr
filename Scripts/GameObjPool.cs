using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对象池
/// </summary>
public class GameObjPool
{
    private Queue<GameObject> _gameObjects = new Queue<GameObject>();

    private GameObject _prefab;


    public GameObjPool(GameObject prefab)
    {
        _prefab = prefab;
    }
    public int Count => _gameObjects.Count;
    /// <summary>
    /// 开始对象个数
    /// </summary>
    public int startNumber = 5;

    /// <summary>
    /// 当对象池空了，添加的个数
    /// </summary>
    public int addNumber = 1;

    /// <summary>
    /// 初始化对象池
    /// </summary>
    public void Initialize()
    {
        AddInstance(startNumber);
    }

    public void AddInstance(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var gObj =  GameObject.Instantiate(_prefab,PooslMgr.Instance.transform);
            gObj.name = _prefab.name;
            gObj.SetActive(false);
            Push(gObj);
        }
    }

    public GameObject Pop()
    {
        if (_gameObjects.Count <= 0) { AddInstance(addNumber); };
        var gObj = _gameObjects.Dequeue();
        gObj.SetActive(true);
        gObj.transform.SetParent(null);
        return gObj;
    }


    public void Push(GameObject obj)
    {
        obj.transform.SetParent(PooslMgr.Instance.transform);
        obj.SetActive(false);
       
        _gameObjects.Enqueue(obj);
    }


    
}
