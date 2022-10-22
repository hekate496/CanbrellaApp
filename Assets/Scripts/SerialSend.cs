using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialSend : MonoBehaviour
{
    public SerialHandler serialHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Y)){
            serialHandler.Write("1");
        }
        if(Input.GetKey(KeyCode.S)){
            serialHandler.Write("s");
        }
        if(Input.GetKey(KeyCode.E)){
            serialHandler.Write("e");
        }
        
    }
}
