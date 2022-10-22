using System.Drawing;
using System.Drawing.Imaging;
using UnityEngine;

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
}