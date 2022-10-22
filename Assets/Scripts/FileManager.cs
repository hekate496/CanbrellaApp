using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using System.Text;

public class FileManager : MonoBehaviour
{
    [SerializeField] GameObject[] activityButtons;
    [SerializeField] GameObject[] activityButtonTexts;
    [SerializeField] GameObject[] activityInformationText;

    private TextMeshProUGUI textMeshProUGUI;
    private string canbrellaActivityPath;
    private string bitMapFilePath;
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

        if(Input.GetKeyDown(KeyCode.W)){
            WriteActivityFile();
        }
    }

    void ReadActivityFile(){
        canbrellaActivityPath = Application.dataPath + @"\Detas\CanbrellaActivity.txt";
        allCanbrellaActivityText = File.ReadAllLines (canbrellaActivityPath);
        foreach (var text in allCanbrellaActivityText) {
			Debug.Log ("各行表示： " + text);
		}
        Debug.Log("行数： " + allCanbrellaActivityText.Length);
    }

    void WriteActivityFile(){
        canbrellaActivityPath = Application.dataPath + @"\Detas\CanbrellaActivity.txt";
        File.AppendAllText(canbrellaActivityPath, "fuga");
        Debug.Log("Saved");
    }

    public void WriteBitMapFile(string str){
        bitMapFilePath = Application.dataPath + @"\Detas\BitMapFile.txt";
        File.AppendAllText(bitMapFilePath, str);
        Debug.Log("Saved");
    }

    public void DrawActivityText(int num){
        string[] InformationName = {"Date  ", "Time  ", "Safety"};

        int i;
        for(i=0; i<3; ++i){
            textMeshProUGUI = activityInformationText[i].GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.SetText(string.Format("{0}  :    {1}", InformationName[i], allCanbrellaActivityText[num].Split(',')[i]));
        }
        textMeshProUGUI = activityInformationText[3].GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.SetText(string.Format("GPS    :    {0}  ; {1}", allCanbrellaActivityText[num].Split(',')[3], allCanbrellaActivityText[num].Split(',')[4]));

    }

    
}


