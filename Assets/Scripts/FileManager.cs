using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class FileManager : MonoBehaviour
{
    //ビルドしてAndroidアプリにするときはTrueに、PCでの開発中はFlaseにする
    [SerializeField] private bool BuildForAndroid; 

    [SerializeField] private GameObject[] activityButtons;
    
    [SerializeField] private GameObject[] activityButtonTexts;
    [SerializeField] private GameObject[] activityInfoTexts;
    [SerializeField] private ImageController imageController;

    private TextMeshProUGUI textMeshProUGUI;
    private string dataPath;
    private string activityFilePath;
    private string bitMapFilePath;
    public string[] ActivityFileTexts;


    void Start()
    {
        // 読み書きを行うテキストファイルにアクセスするためのパス
        // WindowsとAndroidでパスの指定方法が異なるのでBuildForAndroidであわせる
        if(BuildForAndroid){
            // Android用のパス
            // /storage/emulated/0/Android/data/co,.DefaultCompany.com.unity.template.mobile2D/files
            dataPath = Application.persistentDataPath;
        }else{
            // Windows用のパス
            // /CanbrellaApp/Assets
            dataPath = Application.dataPath;
        }

        activityFilePath = dataPath + @"/Datas/ActivityFile.txt";
        bitMapFilePath = dataPath + @"/Datas/BitMapFile.txt";

        // ActivityFileを読み込む
        ActivityFileTexts = File.ReadAllLines (activityFilePath);

        // 最初はActivityボタンを非表示にしておく
        for(int i=0; i<6; ++i){
            activityButtons[i].SetActive(false);
        } 

        // Home画面で押したボタンに応じてActivity画面の内容を変更
        for(int i=0; i<5; ++i){
            // 別の変数を介さないと上手くいかないのでcountを消さないように
            int count = i;
            activityButtons[i].GetComponent<Button>().onClick.AddListener(() => DrawActivityScreen(count));
        }

    }

    void Update()
    {
        // RキーでActivityFileを読み込む
        if(Input.GetKeyDown(KeyCode.R)){
            ActivityFileTexts = File.ReadAllLines (activityFilePath);
        }

        // ActivityFileの内容をHome画面に反映
        for(int i=0; i<ActivityFileTexts.Length && i<6; ++i){
            // 必要なボタンだけを表示
            activityButtons[i].SetActive(true);

            // ボタン上のテキストを更新
            textMeshProUGUI = activityButtonTexts[i].GetComponent<TextMeshProUGUI>();
            if(i < 5){
                textMeshProUGUI.SetText(string.Format("{0}  {1}  {2}", ActivityFileTexts[i].Split(',')[0], ActivityFileTexts[i].Split(',')[1], ActivityFileTexts[i].Split(',')[2]));
            }else{
                textMeshProUGUI.SetText(">> See More Activities");
            }

            // safetyがdangerのときはボタンを赤く
            if(ActivityFileTexts[i].Split(',')[2] == "danger"){
                activityButtons[i].GetComponent<Image>().color = new Color32(255, 93, 93, 255); 
            }
        }

        
    }

    // ActivityFileの末尾にテキスト追加
    void WriteActivityFile(string path, string str){
        File.AppendAllText(activityFilePath, str);
    }


    public void WriteBitMapFile(string str){
        File.AppendAllText(bitMapFilePath, str);
    }

    // ActivityFileの内容をActivity画面に反映
    private void DrawActivityScreen(int num){

        textMeshProUGUI = activityInfoTexts[0].GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.SetText(string.Format("Date  :    {0}", ActivityFileTexts[num].Split(',')[0]));

        textMeshProUGUI = activityInfoTexts[1].GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.SetText(string.Format("Time  :    {0}", ActivityFileTexts[num].Split(',')[1]));

        textMeshProUGUI = activityInfoTexts[2].GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.SetText(string.Format("Safety  :    {0}", ActivityFileTexts[num].Split(',')[2]));

        textMeshProUGUI = activityInfoTexts[3].GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.SetText(string.Format("GPS    :    {0}  ; {1}", ActivityFileTexts[num].Split(',')[3], ActivityFileTexts[num].Split(',')[4]));

        // safe   : 写真は表示しない
        // danger : 適切な写真を表示する
        if(ActivityFileTexts[num].Split(',')[2] == "safe"){
            imageController.SetNullPicture();
        }else{
            string bitMapPath = dataPath + @"/Datas/BitMapFile" + ActivityFileTexts[num].Split(',')[5] + ".txt";
            imageController.SetPicture(bitMapPath);
        }

    }

    

    
}


