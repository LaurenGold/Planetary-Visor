using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meshedit : MonoBehaviour {
    public Transform mro;// = new Vector3(0, 255000, 1000);
    int W = 213;
    int H = 188;
    public float f = 2000;
    float hitdistance;
    public int ii;
    public int jj;
    float extradist = 1; 
    // Use this for initialization
    void Start () {
        //Shape();
    }
	
	// Update is called once per frame
	void Update () {
        MoveAndShape(ii,jj);
    }

    void MoveAndShape(int i, int j)
    {
        //Move GameObject
        Vector3 ray_p = mro.transform.TransformPoint(i - W / 2.0f, j - H / 2.0f, f);
        RaycastHit hit;

        if (!Physics.Raycast(new Ray(mro.position,ray_p), out hit, Mathf.Infinity))
            return;

        transform.position = hit.point;
        hitdistance = hit.distance;

        Vector3 rayToVertex;
        float distance = hitdistance + extradist;

        rayToVertex = mro.transform.TransformPoint(i - W / 2.0f - 0.5f, j - H / 2.0f - 0.5f, f);
        Vector3 a = mro.position + Vector3.Normalize(rayToVertex)*distance - transform.position;

        rayToVertex = mro.transform.TransformPoint(i - W / 2.0f + 0.5f, j - H / 2.0f - 0.5f, f);
        Vector3 b = mro.position + Vector3.Normalize(rayToVertex) * distance - transform.position;

        rayToVertex = mro.transform.TransformPoint(i - W / 2.0f + 0.5f, j - H / 2.0f + 0.5f, f);
        Vector3 c = mro.position + Vector3.Normalize(rayToVertex) * distance - transform.position;

        rayToVertex = mro.transform.TransformPoint(i - W / 2.0f - 0.5f, j - H / 2.0f + 0.5f, f);
        Vector3 d = mro.position + Vector3.Normalize(rayToVertex) * distance - transform.position;

        distance = hitdistance - extradist;

        rayToVertex = mro.transform.TransformPoint(i - W / 2.0f + 0.5f, j - H / 2.0f - 0.5f, f);
        Vector3 aa = mro.position + Vector3.Normalize(rayToVertex) * distance - transform.position;

        rayToVertex = mro.transform.TransformPoint(i - W / 2.0f - 0.5f, j - H / 2.0f - 0.5f, f);
        Vector3 bb = mro.position + Vector3.Normalize(rayToVertex) * distance - transform.position;

        rayToVertex = mro.transform.TransformPoint(i - W / 2.0f - 0.5f, j - H / 2.0f + 0.5f, f);
        Vector3 cc = mro.position + Vector3.Normalize(rayToVertex) * distance - transform.position;

        rayToVertex = mro.transform.TransformPoint(i - W / 2.0f + 0.5f, j - H / 2.0f + 0.5f, f);
        Vector3 dd = mro.position + Vector3.Normalize(rayToVertex) * distance - transform.position;


        Vector3[] vertices = { a, b, c, d, aa, bb, cc, dd };

        //    new Vector3 (0, 0, 0),//0 -> 0
        //    new Vector3 (1, 0, 0),//1 -> 1
        //    new Vector3 (1, 0, 1),//6 -> 2
        //    new Vector3 (0, 0, 1),//7 -> 3
        //    new Vector3 (1.5f, 1, -.5f),//2 -> 4
        //    new Vector3 (-.5f, 1, -.5f),//3 -> 5
        //    new Vector3 (-.5f, 1, 1.5f),//4 -> 6
        //    new Vector3 (1.5f, 1, 1.5f),//5 -> 7
        //};

        int[] triangles = {
            0, 4, 1, //front
            0, 5, 4,
            4, 5, 6, //top
            4, 6, 7,
            1, 4, 7, //right
            1, 7, 2,
            0, 3, 6, //left
            0, 6, 5,
            7, 6, 3, //back
            7, 3, 2,
            0, 2, 3, //bottom
            0, 1, 2
        };

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }






    //void Shape()
    //{
    //    int crismwidth = 213;
    //    int crismheight = 188;
    //    int i, j;

    //    Vector3 a, b, c, d;
    //    Vector3 aa, bb, cc, dd;
    //    a = new Vector3(0, 0, 0);
    //    b = new Vector3(1, 0, 0);
    //    c = new Vector3(1, 0, 1);
    //    d = new Vector3(0, 0, 1);

    //    aa = Vector3.MoveTowards(b, mroPos, 3.0f);
    //    bb = Vector3.MoveTowards(a, mroPos, 3.0f);
    //    cc = Vector3.MoveTowards(d, mroPos, 3.0f);
    //    dd = Vector3.MoveTowards(c, mroPos, 3.0f);
    //    Vector3[] vertices = { a, b, c, d, aa, bb, cc, dd };

    //    //    new Vector3 (0, 0, 0),//0 -> 0
    //    //    new Vector3 (1, 0, 0),//1 -> 1
    //    //    new Vector3 (1, 0, 1),//6 -> 2
    //    //    new Vector3 (0, 0, 1),//7 -> 3
    //    //    new Vector3 (1.5f, 1, -.5f),//2 -> 4
    //    //    new Vector3 (-.5f, 1, -.5f),//3 -> 5
    //    //    new Vector3 (-.5f, 1, 1.5f),//4 -> 6
    //    //    new Vector3 (1.5f, 1, 1.5f),//5 -> 7
    //    //};

    //    int[] triangles = {
    //        0, 4, 1, //front
    //        0, 5, 4,
    //        4, 5, 6, //top
    //        4, 6, 7,
    //        1, 4, 7, //right
    //        1, 7, 2,
    //        0, 3, 6, //left
    //        0, 6, 5,
    //        7, 6, 3, //back
    //        7, 3, 2,
    //        0, 2, 3, //bottom
    //        0, 1, 2
    //    };

    //    Mesh mesh = GetComponent<MeshFilter>().mesh;
    //    mesh.Clear();
    //    mesh.vertices = vertices;
    //    mesh.triangles = triangles;
    //    mesh.RecalculateNormals();
    //}
}
