using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池管理
/// </summary>
public class PooslMgr : MonoBehaviour
{
    public static PooslMgr Instance 
    {
        get { 
            if (_instance == null) 
            {
                (new GameObject()).AddComponent<PooslMgr>();
                return _instance; 
            }
            return _instance; 
        } 
    }

    private static PooslMgr _instance;
    [HideInInspector]
    public new Transform transform;

    public List<GameObject> prefabs = new List<GameObject>();

    /// <summary>
    /// 存放所有对象池
    /// </summary>
    private   Dictionary<int, GameObjPool> _gameObjPools = new Dictionary<int, GameObjPool>();
 
    /// <summary>
    /// 存放对象池的名字对应id
    /// </summary>
    private    Dictionary<string, int> _nameOfPools = new Dictionary<string, int>();

 
    /// <summary>
    /// 创建对象池
    /// </summary>
    public static void CreatePool(GameObject prefab)
    {
        var id = prefab.GetInstanceID();
        //已包含该名字的对象池
        if (Instance._gameObjPools.ContainsKey(id)) return;
        Instance._nameOfPools.Add(prefab.name,id);
        GameObjPool pool = new GameObjPool(prefab);
        pool.Initialize();
        Instance._gameObjPools.Add(id,pool);
 
    }


    /// <summary>
    /// 通过prefab 实例ID 获取对应实例
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public static GameObject Spawn(GameObject prefab)
    {
        var pool = new GameObjPool(prefab);
        Instance._gameObjPools.TryGetValue(prefab.GetInstanceID(), out pool);
        return pool.Pop();
    }
    /// <summary>
    /// 通过名字   获取对应实例
    /// </summary>
    /// <param name="goName"></param>
    /// <returns></returns>
    public static GameObject Spawn(string goName)
    {
        var pool = new GameObjPool(null);
        var id = 0;
       var hasId =  Instance._nameOfPools.TryGetValue(goName, out id);
        if (!hasId) return null;
       var hasPool = Instance._gameObjPools.TryGetValue(id, out pool);
        if (!hasPool) return null;
        return pool.Pop();
    }

    /// <summary>
    /// 通过prefab名字回收 预制体实例
    /// </summary>
    /// <param name="prefab"></param>
    public static void Despawn(GameObject prefab)
    {
        var pool = new GameObjPool(prefab);
        var id = 0;
        var hasId =  Instance._nameOfPools.TryGetValue(prefab.name,out id);
        if (!hasId) return;
        var hasPool = Instance._gameObjPools.TryGetValue(id, out pool);
        if (!hasPool) return;
        
        pool.Push(prefab);
    }

 

    /// <summary>
    /// 延时回收
    /// </summary>
    /// <param name="go"></param>
    /// <param name="delayInSeconds"></param>
    public static void DespawnAfterDelay(GameObject go, float delayInSeconds)
    {
        if (go == null)
            return;

        Instance.StartCoroutine(Instance.internalDespawnAfterDelay(go, delayInSeconds));
    }
 

    IEnumerator internalDespawnAfterDelay(GameObject go,float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        Despawn(go);
    }



 
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            Instance.gameObject.name = "GameObjPoolsMan";
            Instance.transform = gameObject.transform;
            DontDestroyOnLoad(gameObject);
        }
    }


    private void Start()
    {
        foreach (var prefab in prefabs)
        {
            CreatePool(prefab);
        }
    }

}
