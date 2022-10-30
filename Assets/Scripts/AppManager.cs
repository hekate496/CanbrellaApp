using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class AppManager : MonoBehaviour
{
    //ビルドしてAndroidアプリにするときはTrueに、Windowsでの開発中はFlaseにする
    [SerializeField] private bool BuildForAndroid; 

    [SerializeField] private GameObject[] activityButtons;
    [SerializeField] private GameObject[] activityButtonTexts;
    [SerializeField] private GameObject[] activityInfoTexts;
    [SerializeField] private ImageController imageController;
    [SerializeField] private Googlemap googlemap;
    [SerializeField] private BluetoothConnect bluetoothConnect;


    [System.NonSerialized] public string dataPath;
    [System.NonSerialized] public string activityFilePath;
    [System.NonSerialized] public string bitMapFilePath;
    private TextMeshProUGUI textMeshProUGUI;
    private string[] ActivityFileTexts;


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
        for(int i=0; i<5; ++i){
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
        // 新しい書き込みがあったら更新
        if(bluetoothConnect.hasNewActivity){
            ActivityFileTexts = File.ReadAllLines (activityFilePath);
            bluetoothConnect.hasNewActivity = false;
        }

        // ActivityFileの内容をHome画面に反映
        for(int i=0; i<ActivityFileTexts.Length && i<5; ++i){
            // 必要なボタンだけを表示
            activityButtons[i].SetActive(true);

            // ボタン上のテキストを更新
            int underNum = ActivityFileTexts.Length-1 - i;
            textMeshProUGUI = activityButtonTexts[i].GetComponent<TextMeshProUGUI>();
            if(i < 5){
                textMeshProUGUI.SetText(string.Format("{0}  {1}  {2}", ActivityFileTexts[underNum].Split(',')[0], ActivityFileTexts[underNum].Split(',')[1], ActivityFileTexts[underNum].Split(',')[2]));
            }else{
                textMeshProUGUI.SetText("GPS");
            }

            // safetyがdangerのときはボタンを赤く
            if(ActivityFileTexts[underNum].Split(',')[2] == "danger"){
                activityButtons[i].GetComponent<Image>().color = new Color32(255, 93, 93, 255); 
            }else{
                activityButtons[i].GetComponent<Image>().color = new Color32(104, 219, 120, 255);
            }
        }

        
    }

    // ActivityFileの内容をActivity画面に反映
    private void DrawActivityScreen(int num){
        int underNum = ActivityFileTexts.Length -1 -num;

        textMeshProUGUI = activityInfoTexts[0].GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.SetText(string.Format("Date  :    {0}", ActivityFileTexts[underNum].Split(',')[0]));

        textMeshProUGUI = activityInfoTexts[1].GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.SetText(string.Format("Time  :    {0}", ActivityFileTexts[underNum].Split(',')[1]));

        textMeshProUGUI = activityInfoTexts[2].GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.SetText(string.Format("Safety  :    {0}", ActivityFileTexts[underNum].Split(',')[2]));

        // GoogleMap用の緯度経度を更新
        int latInteger = (int)float.Parse(ActivityFileTexts[underNum].Split(',')[3])/100;
        float latFloat = latInteger + (float.Parse(ActivityFileTexts[underNum].Split(',')[3]) - (float)latInteger * 100.0f)/60.0f;
        int lonInteger = (int)float.Parse(ActivityFileTexts[underNum].Split(',')[4])/100;
        float lonFloat = lonInteger + (float.Parse(ActivityFileTexts[underNum].Split(',')[4]) - (float)lonInteger * 100.0f)/60.0f;
        googlemap.lat = latFloat;//float.Parse(ActivityFileTexts[underNum].Split(',')[3]);////
        googlemap.lng = lonFloat;//float.Parse(ActivityFileTexts[underNum].Split(',')[4]);////
        googlemap.Build();

        // safe   : 写真は表示しない
        // danger : 適切な写真を表示する
        if(ActivityFileTexts[underNum].Split(',')[2] == "safe"){
            imageController.SetNullPicture();
        }else{
            string bitMapPath = dataPath + @"/Datas/BitMapFile" + ActivityFileTexts[underNum].Split(',')[5] + ".txt";
            imageController.SetPicture(bitMapPath);
        }

    }

    

    
}


