using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveTest : MonoBehaviour
{
    private bool isMoved = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)){
            if(!isMoved){
                Move();
            }else{
                Back();
            }
        }
    }

    public void Move(){
        isMoved = true;
        this.transform.DOMove(new Vector3(-7f, 0f, 0f), 0.2f);
    }

    public void Back(){
        isMoved = false;
        this.transform.DOMove(new Vector3(0f, 0f, 0f), 0.2f);
    }
}

