using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Painter : MonoBehaviour {
    public string directory;
	// Use this for initialization
	void Start () {
        foreach (Transform child in transform.GetChild(0))
        {
            MeshFilter mf = child.GetComponent<MeshFilter>();
            MeshCollider mc = child.gameObject.AddComponent<MeshCollider>() as MeshCollider;
            mc.sharedMesh = mf.mesh;
            mf.mesh.uv = reverseU(mf.mesh.uv);

            string childlabel = child.name.Split('_')[1].Split('M')[0];
            if (directory == "yuv")
            {
                byte[] bytes = System.IO.File.ReadAllBytes("Assets/yuv.png");
                Texture2D t = new Texture2D(1, 1);
                t.LoadImage(bytes);
                child.GetComponent<Renderer>().material.mainTexture = t;
            }
            else
            {
                //Debug.Log(childlabel + ":" + "Assets/" + directory + "/" + childlabel + "_lg.jpg");
                //Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/" + directory + "/" + childlabel + "_lg.jpg", typeof(Texture2D));
                byte[] bytes = System.IO.File.ReadAllBytes("Assets/" + directory + "/" + childlabel + "_lg.jpg");
                Texture2D t = new Texture2D(1, 1);
                t.LoadImage(bytes);

                child.GetComponent<Renderer>().material.mainTexture = t;
                child.GetComponent<Renderer>().material.SetFloat("_Glossiness", 0.0f);
            }
            //Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/" + directory + "/yuv.png", typeof(Texture2D));
           

            //child.GetComponent<Renderer>().
        }
    }
	
	// Update is called once per frame
	void Update () {
	
    }
    public Vector2[] reverseU(Vector2[] inVec)
    {
        Vector2[] outVec = new Vector2[inVec.Length];
        for (int i = 0; i < inVec.Length; i++)
        {
            outVec[i] = new Vector2(1 - inVec[i].x, inVec[i].y);
        }
        return outVec;
    }
}
