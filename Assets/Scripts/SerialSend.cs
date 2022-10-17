using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialSend : MonoBehaviour
{
    public SerialHandler serialHandler;

    void Update()
    {
        char[] message = new char[] {'h'};
        byte[] messageByte = new byte[] {90, 80 ,50};

        if(Input.GetKeyDown(KeyCode.Y)){
            serialHandler.WriteChar(message);
            //serialHandler.WriteByte(messageByte);
        }
        
    }
}
