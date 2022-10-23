using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System.IO;
using System.Text;
using System;

public class ImageController : MonoBehaviour
{
    [SerializeField] private RawImage img;
    private int width = 300;
    private int height = 200;

    // BitMapFileを読み込み画像を表示する
    public void SetPicture(string filePath){
        
        gameObject.SetActive(true);
        
        // RGB565フォーマットで1ピクセルずつ計算
        Texture2D tex = new Texture2D (width, height, TextureFormat.RGB565, false);
        img.texture = tex;

        using (StreamReader streamReader = new StreamReader (filePath, Encoding.UTF8)) {

                string byteLine = "";
                int a = 256;
                for (int row = 0; row < height; row++)
                {
                    byteLine = streamReader.ReadLine();
                    string[] byteStr = byteLine.Split(',');

                    for (int col = 0; col < width; col++)
                    {
                        // stting2つから16bitの値を得る
                        string lowStr = byteStr[col*2 + 0];
                        string upStr = byteStr[col*2 + 1];
                        byte lowByte = (byte)Int32.Parse(lowStr);
                        byte upByte = (byte)Int32.Parse(upStr);
                        int value = (upByte << 8) | (lowByte);

                        // 16bitのうち、上位5bitが赤、真ん中の6bitが緑、下位5bitが青
                        int r = (((value & 0xf800) >> 11) << 3);
                        int g = (((value & 0x07E0) >> 5)  << 2);
                        int b = ((value & 0x001f) << 3);
                        
                        tex.SetPixel(col, row, new Color32((byte)r, (byte)g, (byte)b, (byte)a));
                    }
                }

                tex.Apply();
            }
    }

    public void SetNullPicture(){
        gameObject.SetActive(false);
    }

}
