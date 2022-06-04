using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HotkeyActivatable
{
    public KeyCode key;
    public GameObject[] parents;
}

public class HotkeyActivate : MonoBehaviour
{
    public HotkeyActivatable[] hotkeyElements;
    void Update()
    {
        foreach (HotkeyActivatable element in hotkeyElements)
        {
            if (Input.GetKeyDown(element.key))
            {
                print(element.key);
                foreach (GameObject parent in element.parents)
                {
                    parent.SetActive(!parent.activeSelf);
                }
            }
        }
    }
}
