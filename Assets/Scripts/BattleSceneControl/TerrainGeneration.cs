using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;
using Unity.VisualScripting;
using System.Drawing;
using UnityEngine.Rendering.Universal;
using Unity.Mathematics;
public class TerrainGeneration : MonoBehaviour
{
    Mesh mesh;

    public GameObject[] TreePrefab;
    public GameObject Grasslayer;
    public Transform Treelayer;
    private float grassheight = 3;
    private float treeheight = 3;
    public Material TerrainMat;
    private const float MaxHeight = 10f;
    private int LOD = 1;
    private Vector3[] GenerateGrassPlaneVertices(Vector2 size, out Vector2[] uv, out Vector3[] normal, AnimationCurve hc)
    {
        int xsize = (int)size.x;
        int ysize = (int)size.y;
        int arrlen = (xsize / LOD + 1) * (ysize / LOD + 1);
        
        Vector3[] result = new Vector3[arrlen];
        uv = new Vector2[arrlen];
        normal = new Vector3[arrlen];
        Vector2 spawnpos = new Vector2(transform.position.x,transform.position.y);
        float baseNoiseScale = 0.05f;
        float offsetX = UnityEngine.Random.Range(0f, 1000f);
        float offsetY = UnityEngine.Random.Range(0f, 1000f);
        for (int y = -ysize / 2, i = 0; y <= ysize / 2; y+=LOD)
        {
            for (int x = -xsize / 2; x <= xsize / 2; x+=LOD, i ++)
            {
                float xGen = (x + spawnpos.x + offsetX);
                float yGen = (y + spawnpos.y + offsetY);
                float perlin = Mathf.PerlinNoise(xGen * baseNoiseScale, yGen * baseNoiseScale);
                float baseNoise = perlin * Mathf.Sin(xGen * yGen * UnityEngine.Random.Range(0, 2f)); 


                float height = hc.Evaluate(baseNoise) * MaxHeight;

                result[i] = new Vector3(x, height, y);

                uv[i] = new Vector2((float)x / xsize, (float)y / ysize);
                normal[i] = Vector3.up;
            }
        }
        return result;
    }
    //private Vector2[] GenerateUV(Vector2 size, float scale)
    //{
    //    int xsize = (int)size.x;
    //    int ysize = (int)size.y;
    //    Vector2[] uvs = new Vector2[(xsize + 1) * (ysize + 1)];

    //    for (int y = 0; y <= ysize; y++)
    //    {
    //        for (int x = 0; x <= xsize; x++)
    //        {
    //            int index = y * (xsize + 1) + x;
    //            uvs[index] = new Vector2((float)x / xsize, (float)y / ysize);
    //        }
    //    }
    //    return uvs;
    //}
    Vector3 supposedposition;
    private void OnEnable()
    {
        supposedposition = new Vector3(transform.position.x, 0, transform.position.z);
    }
    private void Update()
    {

    }
    //private int GetSubmesh(float height)
    //{
    //    for (int i = 0; i < HeightSubmeshBoundaries.Length; i++)
    //    {
    //        if (height < HeightSubmeshBoundaries[i]*MaxHeight)
    //        {
    //            return i - 1;
    //        }
    //    }
    //    return 0;
    //}
    //private bool GetBoundarySubmesh(float height)
    //{
    //    for(int i = 1;  i < HeightSubmeshBoundaries.Length;i++)
    //    {
    //        if (Math.Abs(HeightSubmeshBoundaries[i]-height) < 0.01f)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    public void Generate(int seed, GridSystem.ChunkType t, Vector2 size, AnimationCurve heightc)
    {
        int xsize = (int)size.x/LOD;
        int ysize = (int)size.y / LOD;
        TerrainMat.SetFloat("_MaxHeight", MaxHeight);
        TerrainMat.SetFloat("_MinHeight",0);

        //indexesofSubmesh = new int[HeightSubmeshBoundaries.Length];
        //for (int i = 0; i < HeightSubmeshBoundaries.Length; i++)
        //{
        //    AllSubmeshesTriangles.Add(new int[xsize * ysize * 6]);

        //}
        Mesh mesh = new Mesh();


        GetComponent<MeshFilter>().mesh = mesh;
        
        GetComponent<MeshCollider>().sharedMesh = mesh;
        mesh.name = "generatedmesh";

        Vector3[] vertices = new Vector3[0];
        Vector2[] uv = new Vector2[0];
        Vector3[] normals = new Vector3[0];
        switch (t)
        {
            case GridSystem.ChunkType.GrassPlane:
                vertices = GenerateGrassPlaneVertices(size, out uv, out normals, heightc);
                break;
        }


        mesh.vertices = vertices;
        #region commented
        //Connect vertices and turn them into triangle
        //vi is going through the list

        //int ti = 0;

        //for (int x = 0, vi = (xsize +1) * (ysize) * 2; x < xsize * 2 - 1; x++, vi++, ti += 6)
        //{
        //    tris[ti] = vi;
        //    tris[ti + 2] = vi + 2;
        //    tris[ti + 1] = vi + 1;

        //    tris[ti + 3] = vi + 2;
        //    tris[ti + 5] = vi + 3;
        //    tris[ti + 4] = vi + 1;
        //}
        //for (int x = 0, vi = 0; x < xsize * 2 - 1; x++, vi++, ti += 6)
        //{
        //    tris[ti] = vi;
        //    tris[ti + 1] = vi + 2;
        //    tris[ti + 2] = vi + 1;

        //    tris[ti + 3] = vi + 2;
        //    tris[ti + 4] = vi + 3;
        //    tris[ti + 5] = vi + 1;
        //}








        //for (int y = 0, vi = 0; y < ysize; y++, vi += 2 * xsize + 2, ti += 6)
        //{
        //    tris[ti] = vi;
        //    tris[ti + 2] = vi + (xsize + 1) * 2;

        //    tris[ti + 1] = vi + 1;

        //    tris[ti + 3] = vi + 2 * (xsize + 1);
        //    tris[ti + 5] = vi + 2 * (xsize + 1) + 1;
        //    tris[ti + 4] = vi + 1;

        //}
        //for (int y = 0, vi = 2 * xsize; y < ysize; y++, vi += 2 * xsize + 2, ti += 6)
        //{
        //    tris[ti] = vi;
        //    tris[ti + 1] = vi + (xsize + 1) * 2;

        //    tris[ti + 2] = vi + 1;

        //    tris[ti + 3] = vi + 2 * (xsize + 1);
        //    tris[ti + 4] = vi + 2 * (xsize + 1) + 1;
        //    tris[ti + 5] = vi + 1;

        //}


        //for (int y = 0, vi = 1; y < ysize; y++, vi += 2)
        //{
        //    for (int x = 0; x < xsize; x++, vi += 2, ti += 6)
        //    {
        //        tris[ti] = vi;
        //        tris[ti + 2] = vi + (xsize + 1) * 2;
        //        tris[ti + 1] = vi + (1) * 2;

        //        tris[ti + 3] = vi + (1) * 2;
        //        tris[ti + 5] = vi + (xsize + 1) * 2;
        //        tris[ti + 4] = vi + (xsize + 2) * 2;


        //    }
        //}
        #endregion
        int ti = 0;
        int[] trigs = new int[xsize * ysize * 6];
        for (int y = 0, vi = 0; y < ysize; y++, vi ++)
        {
            for (int x = 0; x < xsize; x++, vi ++, ti += 6)
            {
                //float height = mesh.vertices[vi].y;
                //int index = GetSubmesh(height);
                //Debug.Log(index);
                //int vtid = indexesofSubmesh[index];
                //AllSubmeshesTriangles[index][vtid] = vi;
                //AllSubmeshesTriangles[index][vtid + 1] = vi+ (xsize + 1);
                //AllSubmeshesTriangles[index][vtid + 2] = vi + (1);

                ////if (GetBoundarySubmesh(height) && index > 0)
                ////{
                ////    index--;
                ////}

                //AllSubmeshesTriangles[index][vtid + 3] = vi + (1);
                //AllSubmeshesTriangles[index][vtid + 4] = vi + (xsize + 1);
                //AllSubmeshesTriangles[index][vtid + 5] = vi + (xsize + 2);
                //indexesofSubmesh[index] += 6;

                trigs[ti] = vi;
                trigs[ti + 1] = vi + 1 + xsize;
                trigs[ti + 2] = vi + 1;
                trigs[ti + 3] = vi + 1;
                trigs[ti + 4] = vi + 1 + xsize;
                trigs[ti + 5] = vi + 2 + xsize;
                

            }
        }




        mesh.subMeshCount = 1;
        //for (int i = 0; i < indexesofSubmesh.Length; i++)
        //{
        //    mesh.SetTriangles(AllSubmeshesTriangles[i], 0);

        //}
        mesh.triangles = trigs;
        mesh.uv = uv;
        mesh.normals = normals;
        
        //gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.layer = GameInformation.BuildingLayer;
    }

}
