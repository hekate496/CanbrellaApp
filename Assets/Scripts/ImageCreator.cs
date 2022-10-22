using System.Drawing;
using System.Drawing.Imaging;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class ImageCreator
{
    /// <summary>
    /// バツ印の画像を生成する。
    /// </summary>
    /// <param name="size">画像の縦・横のサイズ。px数を指定する。</param>
    public static void CreateCrossMark(int size)
    {
        Bitmap bmp = new Bitmap(size, size);

        // 全ピクセルに色付け
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (row == col || row + col == size - 1)
                {
                    bmp.SetPixel(col, row, System.Drawing.Color.White);
                }
                else
                {
                    bmp.SetPixel(col, row, System.Drawing.Color.Black);
                }
            }
        }

        bmp.Save(Application.dataPath + @"\Images\cross_mark.png", ImageFormat.Png);
    }

    public static void CreateImage(int width, int height)
    {
        string filePath = Application.dataPath + @"\Detas\BitMapFile.txt";
        Bitmap bmp = new Bitmap(width, height);
 
		using (StreamReader streamReader = new StreamReader (filePath, Encoding.UTF8)) {
            // for (int row = 0; row < height; row++)
            // {
            //     for (int col = 0; col < width; col++)
            //     {
            //         System.Drawing.Color c = System.Drawing.Color.FromArgb(0, 0, 0);
            //         bmp.SetPixel(col, row, c);
            //     }
            // }

            string readStr = "";
            for (int row = 0; row < height; row++)
            {
                readStr = streamReader.ReadLine();
                for (int col = 0; col < width; col++)
                {
                    // if(col % 16 == 0){
                    //    readStr = streamReader.ReadLine();
                    // }
                    // string lstr =readStr.Split(',')[(col%16)*2 + 0];
                    // string ustr =readStr.Split(',')[(col%16)*2 + 1];
                    
                    string lstr =readStr.Split(',')[col*2 + 0];
                    string ustr =readStr.Split(',')[col*2 + 1];
                    byte lbyte = (byte)Int32.Parse(lstr);
                    byte ubyte = (byte)Int32.Parse(ustr);
                    Debug.Log("readBitMap : " + lbyte + " " + ubyte); 
                    int value = (ubyte << 8) | (lbyte);
                    int r = (((value & 0xf800) >> 11) << 3);
                    int g = (((value & 0x07E0) >> 5)  << 2);
                    int b = ((value & 0x001f) << 3);
                    
                    System.Drawing.Color c = System.Drawing.Color.FromArgb(r, g, b);

                    bmp.SetPixel(col, row, c);
                }
            }
		}


        bmp.Save(Application.dataPath + @"\Images\picture.png", ImageFormat.Png);
    }

    public static void ReadBitMapFile(){
        
    }
}