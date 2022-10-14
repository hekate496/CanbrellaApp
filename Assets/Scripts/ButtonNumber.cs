using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonNumber : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int buttonNum;

    public int setButtonNum(){
        return buttonNum;
    }
}
