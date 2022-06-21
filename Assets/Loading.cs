using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Loading : MonoBehaviour
{
    public GameObject bg;
    List<RawImage> spriteRenderers;
    public float speed = 2.67f;
    public float animLength = 2f;
    private void OnEnable()
    {
        Game.loadingScript = this;
        spriteRenderers = new List<RawImage>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i).GetChild(0);
            for (int j = 0; j < 2; j++)
            {
                Transform edge_or_sth = child.GetChild(j);
                for (int k = 0; k < 4; k++)
                {
                    Transform number = edge_or_sth.GetChild(k);
                    spriteRenderers.Add(number.GetComponent<RawImage>());
                }
            }
        }
    }
    public void EndLoading()
    {

        StartCoroutine(shit());
    }
    IEnumerator shit()
    {
        yield return new WaitForSeconds(speed - animLength / 2);
        bg.SetActive(true);
        yield return new WaitForSeconds(animLength / 2);
        bg.SetActive(false);
        Game.GameInstanceHolder.SetActive(true);
        gameObject.SetActive(false);
    }
}
