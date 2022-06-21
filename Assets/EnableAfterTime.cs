using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct ObjectData
{
    public float time;
    public GameObject gameObject;
}
public class EnableAfterTime : MonoBehaviour
{
    public ObjectData[] objects;
    void Start()
    {
        foreach (ObjectData obj in objects)
        {
            StartCoroutine(Enable(obj));
        }
    }
        
    IEnumerator Enable(ObjectData obj)
    {
        yield return new WaitForSeconds(obj.time);
        obj.gameObject.SetActive(true);
    }
}
