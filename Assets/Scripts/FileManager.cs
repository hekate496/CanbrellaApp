using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
public class FileManager : MonoBehaviour
{
    void Start()
    {
        string path = Application.dataPath + @"\Datas\CanbrellaActivities.txt";
        using (StreamReader streamReader = new StreamReader (path, Encoding.UTF8)) {
                while(!streamReader.EndOfStream) {
                    Debug.Log("ストリームで読み込み：" + streamReader.ReadLine ());
                }
        }
    }
    
}
