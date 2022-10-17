using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour
{
    [SerializeField] GameObject[] activityButtons;
    [SerializeField] GameObject[] activityButtonTexts;
    [SerializeField] GameObject[] activityInformationText;

    private TextMeshProUGUI textMeshProUGUI;
    private string canbrellaActivityPath;
    public string[] allCanbrellaActivityText;


    void Start()
    {
        ReadActivityFile();

        int i;
        for(i=0; i<6; ++i){
            activityButtons[i].SetActive(false);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            ReadActivityFile();
        }

        if(Input.GetKeyDown(KeyCode.W)){
            WriteActivityFile("hoge");
        }

        //Home画面の表示内容をCanbrellaActivity.txtの内容に応じて変更する
        int i;
        for(i=0; i<allCanbrellaActivityText.Length && i<6; ++i){
            activityButtons[i].SetActive(true);
            textMeshProUGUI = activityButtonTexts[i].GetComponent<TextMeshProUGUI>();

            if(i < 5){
                textMeshProUGUI.SetText(string.Format("{0} {1} {2}", allCanbrellaActivityText[i].Split(',')[0], allCanbrellaActivityText[i].Split(',')[1], allCanbrellaActivityText[i].Split(',')[2]));
            }else{
                textMeshProUGUI.SetText(">> See More Activities");
            }
        }

        
    }

    //CanbrellaActivity.txtの内容を読み込む
    void ReadActivityFile(){
        canbrellaActivityPath = Application.dataPath + @"\Detas\CanbrellaActivity.txt";
        allCanbrellaActivityText = File.ReadAllLines (canbrellaActivityPath);
        foreach (var text in allCanbrellaActivityText) {
			Debug.Log ("各行表示： " + text);
		}
    }

    //CanbrellaActivity.txtに書き込む
    void WriteActivityFile(string str){
        canbrellaActivityPath = Application.dataPath + @"\Detas\CanbrellaActivity.txt";
        File.AppendAllText(canbrellaActivityPath, str);
        Debug.Log("CanbrellaActivity.txt に書き込みました");
    }

    //Activity画面の内容を押したボタンに応じて変更する
    public void DrawActivityText(int buttonNum){
        string[] InformationName = {"Date  ", "Time  ", "Safety"};

        if(buttonNum < 5){
            int i;
            for(i=0; i<3; ++i){
                textMeshProUGUI = activityInformationText[i].GetComponent<TextMeshProUGUI>();
                textMeshProUGUI.SetText(string.Format("{0}  :    {1}", InformationName[i], allCanbrellaActivityText[buttonNum].Split(',')[i]));
            }
            textMeshProUGUI = activityInformationText[3].GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.SetText(string.Format("GPS    :    {0}  ; {1}", allCanbrellaActivityText[buttonNum].Split(',')[3], allCanbrellaActivityText[buttonNum].Split(',')[4]));

        }else if(buttonNum == 5){
            //
            //SeeMoreActivities... を押した時の処理を書く（未完）
            //
        }

    }
    
}


