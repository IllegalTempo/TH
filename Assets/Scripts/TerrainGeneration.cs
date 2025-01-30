using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


public class TerrainGeneration : MonoBehaviour
{
    Mesh mesh;

    public const int xsize = 150, ysize = 150;
    private Vector3[] vertices;


    public GameObject[] TreePrefab;
    public GameObject Grasslayer;
    public Transform Treelayer;

    private float grassheight = 2;
    private float treeheight = 3;
    
    public void Generate(int seed)
    {
        UnityEngine.Random.InitState(seed);
        Mesh mesh = new Mesh();
        Mesh grasslayer = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        Grasslayer.GetComponent<MeshFilter>().mesh = grasslayer;
        mesh.name = "generatedmesh";
        grasslayer.name = "grasslayer";

        vertices = new Vector3[(xsize+1) * (ysize+1)];
        Vector2[] uv = new Vector2[vertices.Length];
        for (int y = 0, i = 0; y <= ysize; y++)
        {
            for (int x = 0; x <= xsize; x++, i++)
            {
                float height = Mathf.PerlinNoise((x + transform.position.x)*0.05f, (y + transform.position.y) * 0.05f) * 5;
                if (x == xsize || y == ysize || y == 0||x==0)
                {
                    height = 2.5f;
                }
                Vector3 vertpos = new Vector3(x,height, y);
                vertices[i] = vertpos;

                uv[i] = new Vector2((float)x / xsize, (float)y / ysize);
                if(UnityEngine.Random.Range(0, UnityEngine.Random.Range(200,300)) == 99)
                {
                    Instantiate(TreePrefab[UnityEngine.Random.Range(0,TreePrefab.Length)], vertpos + transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, UnityEngine.Random.Range(0f, 360f), transform.rotation.eulerAngles.z),Treelayer); 
                }
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        grasslayer.vertices = vertices;
        List<int> lowTriangles = new List<int>();
        List<int> highTriangles = new List<int>();

        for (int ti = 0, vi = 0, y = 0; y < ysize; y++, vi++)
        {
            for (int x = 0; x < xsize; x++, ti += 6, vi++)
            {
                int v0 = vi;
                int v1 = vi + 1;
                int v2 = vi + xsize + 1;
                int v3 = vi + xsize + 2;

                int[] tri1 = { v0, v2, v1 }; // First triangle
                int[] tri2 = { v1, v2, v3 }; // Second triangle

                // Determine which submesh each triangle belongs to
                if (vertices[v0].y > grassheight || vertices[v1].y > grassheight || vertices[v2].y > grassheight)
                {
                    highTriangles.AddRange(tri1);
                }
                else
                {
                    lowTriangles.AddRange(tri1);
                }

                if (vertices[v1].y > grassheight || vertices[v2].y > grassheight || vertices[v3].y > grassheight)
                {
                    highTriangles.AddRange(tri2);
                }
                else
                {
                    lowTriangles.AddRange(tri2);
                }
            }
        }
        grasslayer.SetTriangles(highTriangles, 0);
        mesh.triangles = highTriangles.Concat(lowTriangles).ToArray();
        mesh.RecalculateNormals();

        grasslayer.RecalculateNormals();
    }
}
