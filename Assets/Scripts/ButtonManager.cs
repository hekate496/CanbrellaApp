using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonManager : MonoBehaviour
{
    [SerializeField] Button[] activityButtons;
    [SerializeField] FileManager fileManager;
    [SerializeField] MoveUi moveUi;


    void Start(){
        int i; 
        for(i=0; i<5; ++i){
            var count = i;
            activityButtons[i].onClick.AddListener(() => buttonClick(count));
        }
    }

    void buttonClick(int num)
    {
        Debug.Log($"{num}をクリックしました");
        fileManager.DrawActivityText(num);
        moveUi.MoveLeft();
    }
}   
