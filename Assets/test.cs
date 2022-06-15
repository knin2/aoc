using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public int z;
    public int w;
    Mesh ret;
    Vector3[] verts;
    int[] tris;
    void Start()
    {
        verts = new Vector3[]
        {
            new Vector3(-w, 0, z),
            new Vector3(-w, w, z),
            new Vector3(w, 0, z)
        };

        tris = new int[]
        {
            0, 1, 2
        };
        MeshUtil.mrenderer = gameObject;

        ret = MeshUtil.CreateMesh(tris, verts);
    }
    private void Update()
    {
        verts = new Vector3[]
        {
            new Vector3(-w, 0, z),
            new Vector3(-w, w, z),
            new Vector3(w, 0, z)
        };

        tris = new int[]
        {
            0, 1, 2
        };
        ret = MeshUtil.CreateMesh(tris, verts);
        MeshUtil.Render(ret);
    }
}
