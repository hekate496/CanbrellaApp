using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ImageCreator.CreateCrossMark(100);
        ImageCreator.CreateImage(300,200);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
