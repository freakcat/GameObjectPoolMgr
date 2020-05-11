using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PoolsManTest : MonoBehaviour
{
    public GameObject prefab;
    public static Vector3 worldMousePosition;

    private Queue<GameObject> hitGos = new Queue<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
      PooslMgr.CreatePool(prefab);
    }
 
    // Update is called once per frame
    void Update()
    {
 
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        worldMousePosition = ray.direction;
        Debug.DrawRay(ray.origin, ray.direction*1000f, Color.red);
        if (Physics.Raycast(ray,out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform != null)
                {
                    Debug.Log(hit.transform.gameObject.name);
                    PooslMgr.Despawn(hit.transform.gameObject);
                   
                }
 
            }
            hit.transform.GetComponent<MeshRenderer>().material.color = Color.green;
            hitGos.Enqueue(hit.transform.gameObject);
            
            Debug.DrawRay(ray.origin, (hit.point -ray.origin), Color.green);
        }
        else
        {
            if(hitGos.Count>0)
            {
                hitGos.Dequeue().GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }
 
    }
 
}
