using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MoveUi : MonoBehaviour
{
    public void MoveLeft(){
        this.transform.DOMove(new Vector3(-7f, 0f, 0f), 0.2f);
    }

    public void MoveRigth(){
        this.transform.DOMove(new Vector3(0f, 0f, 0f), 0.2f);
    }
}
