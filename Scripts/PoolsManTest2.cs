using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsManTest2 : MonoBehaviour
{

    public string spawnName = "Cube";
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Swapn(spawnName);
        }
    }

    public void Swapn(string goName)
    {
       var obj =  PooslMgr.Spawn(goName);
        if (!obj.GetComponent<BoxCollider>())
            obj.AddComponent<BoxCollider>();

        obj.transform.position = PoolsManTest.worldMousePosition*100f;
    }
}
