using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;


public class Weather : MonoBehaviour
{
    //https://home.openweathermap.org/myservices から登録した
    //https://api.openweathermap.org/data/2.5/weather?lat=36.064371&lon=140.059760&appid=35f0730fca9034b3a7addce34d8be98a
    //緯度3606.6437 経度14005.9706
    //public Text resultText;
    public string message,ido,keido,URL;
    private string weather;
    public string weatherStr;


    // Start is called before the first frame update
    void Start()
    {
        ido = "36.064371";
        keido = "140.059760";
        URL = "https://api.openweathermap.org/data/2.5/weather?lat=" + ido + "&lon=" + keido + "&appid=35f0730fca9034b3a7addce34d8be98a";
        StartCoroutine("GET");//ここでwebAPIから天気取得
    }
    
    private IEnumerator GET()//天気取得json
    {
        using (var req = UnityWebRequest.Get(URL))
        {
            yield return req.SendWebRequest();
            if (req.isNetworkError)
            {
                //resultText.text = req.error;
                message = req.error;
            }
            else if (req.isHttpError)
            {
                //resultText.text = req.error;
                message = req.error;
            }
            else
            {
                //resultText.text = req.downloadHandler.text;
                message = req.downloadHandler.text;
            }
        }
        Debug.Log(message);
        Cutmessage(message);//json形式をから天気取り出す関数
    }

    public void Cutmessage(string message)//json形式から天気取り出す
    {
        for (int i = 0; i <160;i++)
        {
            if (message[i] == 'i')
            {
                if (message[i + 1] == 'c')
                {
                    for (int j = 7;j<10;j++)
                    {
                        weather += message[i + j];
                    }
                    Debug.Log(weather);
                    LED(weather);//天気によってLED変える関数
                    break;
                }
            }
        }
    }
    public void LED(string weather)//天気によって条件変える iconの値は https://www.sglabs.jp/openweathermap-api/　に記載
    {
        switch (weather)
        {
            //ここで送るデータを処理すればよし
            case "01n":
            case "02n":
            case "01d":
            case "02d":
                weatherStr = "1";
                break;

            case "03n":
            case "04n":
            case "03d":
            case "04d":
                weatherStr = "2";

                break;

            case "09n":
            case "10n":
            case "11n":
            case "09d":
            case "10d":
            case "11d":
                weatherStr = "3";
                break;

            default:
                weatherStr = "0";
                break;
        }
    }


    //{"coord":{"lon":140.0598,"lat":36.0644},"weather":[{"id":800,"main":"Clear","description":"clear sky","icon":"01n"}],"base":"stations","main":{"temp":291.87,"feels_like":291.29,"temp_min":288.28,"temp_max":295.27,"pressure":1024,"humidity":57,"sea_level":1024,"grnd_level":1022},"visibility":10000,"wind":{"speed":3.77,"deg":177,"gust":8.23},"clouds":{"all":1},"dt":1666348124,"sys":{"type":2,"id":2077636,"country":"JP","sunrise":1666299074,"sunset":1666339043},"timezone":32400,"id":2110681,"name":"Tsukuba","cod":200}
}
