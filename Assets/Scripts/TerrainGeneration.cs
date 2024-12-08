using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    Mesh mesh;

    public int xsize = 100, ysize = 100;
    private Vector3[] vertices;
    private int[] triangles;
    private void Awake()
    {
    }
    void Start()
    {
        Generate();
    }
    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "generatedmesh";
        vertices = new Vector3[(xsize+1) * (ysize+1)];

        for(int y=0,i = 0; y <= ysize;y++)
        {
            for (int x = 0; x <= xsize; x++,i++)
            {
                vertices[i] = new Vector3(x,Mathf.Sin(Mathf.PerlinNoise(x*.07f,y*.07f) ) * 5,y);
            }
        }
        mesh.vertices = vertices;


        triangles = new int[6*xsize*ysize];
        for (int ti = 0, vi = 0, y = 0; y < ysize; y++, vi++)
        {
            for (int x = 0; x < xsize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xsize + 1;
                triangles[ti + 5] = vi + xsize + 2;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
