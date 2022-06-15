using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct BAOC_Triangle
{
    public int x, y, z; //range(points[n]) = (0, 4)
    public int[] points;
    public BAOC_Triangle(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        
        points = new int[3] { x, y, z };
    }
    public static explicit operator BAOC_Triangle(int[] iarr) => new BAOC_Triangle(iarr[0], iarr[1], iarr[2]);

    public int[] ToArray() => new int[] { x, y, z };
}
public class MeshUtil : MonoBehaviour
{
    public static GameObject mrenderer;
    public static Mesh CreateMesh(int[] triangles, Vector3[] vertices)
    {
        //MeshRenderer meshRenderer = renderer.GetComponent<MeshRenderer>();
        //meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        //MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();


        mesh.vertices = vertices;

        mesh.triangles = triangles;

        mesh.RecalculateNormals();


        return mesh;
    }
    public static void Render(Mesh mesh)
    {
        mrenderer.GetComponent<Renderer>().GetComponent<MeshFilter>().mesh = mesh;
    }
}
