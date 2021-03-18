using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MAHLI : MonoBehaviour
{
    float metersPerDegree = 59288.8889f; // marsCirc/360
    private Vector2 originLatLong;
    public Resources MAHLIfolder;
    public string imagePath;
    Texture2D[] texList;
    //make list of mahli
  

    class MAHLIimages {
 
        public string target;
        public GameObject gameObject;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        Object[] textures = Resources.LoadAll(imagePath, typeof(Texture2D));
        texList = new Texture2D[textures.Length];
        for (int i = 0; i < textures.Length; i++) {
            texList[i] = (Texture2D)textures[i];
        }
        Debug.Log("Textures Loaded: " + texList.Length);
        Debug.Log("Image Path: " + imagePath);
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    //Monitor changes to originLatLong, move readings as needed. 
   
   

    }
    
    

