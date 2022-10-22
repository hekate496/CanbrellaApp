using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveUI : MonoBehaviour
{
    public void GoActivity(){
        this.transform.DOMove(new Vector3(-7f, 0f, 0f), 0.2f);
    }

    public void GoSetting(){
        this.transform.DOMove(new Vector3(7f, 0f, 0f), 0.2f);
    }

    public void BackHome(){
        this.transform.DOMove(new Vector3(0f, 0f, 0f), 0.2f);
    }
}
