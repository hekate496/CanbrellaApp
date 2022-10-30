using UnityEngine;
using UnityEngine.UI;
using SVSBluetooth;
using System.Text;
using System.IO;
using System;


public class BluetoothConnect : MonoBehaviour {
    [SerializeField] AppManager appManager;
    public Image image; // a picture that displays the status of the bluetooth adapter upon request
    public Text textField; // field for displaying messages and events
    
    // 適切にUUIDとADDRを設定
    // 以下は、SPP通信することを示すUUIDと、マイコン側で使うbluetoothモジュールHC-05のアドレス
    const string MY_UUID = "00001101-0000-1000-8000-00805F9B34FB";
    const string MY_ADDR = "00:22:03:01:3E:DE";

    private int lineCount = 0;
    private int byteCount = 0;
    private int commaCount = -1;
    private bool isDanger = false;
    private bool isActivitySign = false;
    private bool isImageSign = false;
    private bool isGpsSign = false;
    public string str;
    [System.NonSerialized] public bool hasNewActivity = false;
    [SerializeField] private Googlemap googlemap;
    [SerializeField] private Weather weather;

    private DateTime dateTime;


    private string[] nameNum;
    private string readPath;
    private string writePath;

    BluetoothForAndroid.BTDevice[] devices;
    string lastConnectedDeviceAddress;

    // subscription and unsubscribe from events. You can read more about events in Documentation.pdf

    private void OnEnable() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;


        BluetoothForAndroid.ReceivedIntMessage += PrintVal1;
        BluetoothForAndroid.ReceivedFloatMessage += PrintVal2;
        BluetoothForAndroid.ReceivedStringMessage += PrintVal3;
        //BluetoothForAndroid.ReceivedByteMessage += PrintVal4;

        // 追加
        BluetoothForAndroid.ReceivedByteMessage += WriteBitMapFile;

        BluetoothForAndroid.BtAdapterEnabled += PrintEvent1;
        BluetoothForAndroid.BtAdapterDisabled += PrintEvent2;
        BluetoothForAndroid.DeviceConnected += PrintEvent3;
        BluetoothForAndroid.DeviceDisconnected += PrintEvent4;
        BluetoothForAndroid.ServerStarted += PrintEvent5;
        BluetoothForAndroid.ServerStopped += PrintEvent6;
        BluetoothForAndroid.AttemptConnectToServer += PrintEvent7;
        BluetoothForAndroid.FailConnectToServer += PrintEvent8;

        BluetoothForAndroid.DeviceSelected += PrintDeviceData;

        
    }
    private void OnDisable() {

        BluetoothForAndroid.ReceivedIntMessage -= PrintVal1;
        BluetoothForAndroid.ReceivedFloatMessage -= PrintVal2;
        BluetoothForAndroid.ReceivedStringMessage -= PrintVal3;
        //BluetoothForAndroid.ReceivedByteMessage -= PrintVal4;

        // 追加
        BluetoothForAndroid.ReceivedByteMessage -= WriteBitMapFile;

        BluetoothForAndroid.BtAdapterEnabled -= PrintEvent1;
        BluetoothForAndroid.BtAdapterDisabled -= PrintEvent2;
        BluetoothForAndroid.DeviceConnected -= PrintEvent3;
        BluetoothForAndroid.DeviceDisconnected -= PrintEvent4;
        BluetoothForAndroid.ServerStarted -= PrintEvent5;
        BluetoothForAndroid.ServerStopped -= PrintEvent6;
        BluetoothForAndroid.AttemptConnectToServer -= PrintEvent7;
        BluetoothForAndroid.FailConnectToServer -= PrintEvent8;

        BluetoothForAndroid.DeviceSelected -= PrintDeviceData;

        
    }

    private void Start() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // so that the screen does not go out
        Initialize(); // plugin initialization
        textField.text += "Initialized \n";

    }



    // Initially, always initialize the plugin.
    public void Initialize() {
        BluetoothForAndroid.Initialize();
        image.color = Color.green;
    }

    // methods for controlling the bluetooth and getting its state
    // public void GetBluetoothStatus() {
    //     if (BluetoothForAndroid.IsBTEnabled()) image.color = Color.green;
    //     else image.color = Color.red;
    // }
    public void EnableBT() {
        BluetoothForAndroid.EnableBT();
    }
    public void DisableBT() {
        BluetoothForAndroid.DisableBT();
    }

    // methods for creating and stopping the server, connecting to the server and disconnecting
    public void CreateServer() {
        BluetoothForAndroid.CreateServer(MY_UUID);
    }
    public void StopServer() {
        BluetoothForAndroid.StopServer();
    }
    public void ConnectToServer() {
        BluetoothForAndroid.ConnectToServer(MY_UUID);
    }
    public void Disconnect() {
        byte[] array = Encoding.UTF8.GetBytes ("d");
        BluetoothForAndroid.WriteMessage (array);

        BluetoothForAndroid.Disconnect();
    }
    public void ConnectToServerByAddress() {
        // if (devices != null) {
        //     if (devices[0].address != "none") BluetoothForAndroid.ConnectToServerByAddress(MY_UUID, devices[0].address);
        // } 
        BluetoothForAndroid.ConnectToServerByAddress(MY_UUID, MY_ADDR);
    }
    public void ConnectToLastServer() {
        if (lastConnectedDeviceAddress != null) BluetoothForAndroid.ConnectToServerByAddress(MY_UUID, lastConnectedDeviceAddress);
    }

    // methods for sending messages of various types
    public void WriteMessage1() {
        BluetoothForAndroid.WriteMessage(15);
    }
    public void WriteMessage2() {
        BluetoothForAndroid.WriteMessage(100.69f);
    }
    public void WriteMessage3() {
        textField.text += "send s : lineCount ";
        textField.text += lineCount + "\n";
        byte[] array = Encoding.UTF8.GetBytes ("hello");
        BluetoothForAndroid.WriteMessage (array); 
    }

    // methods for displaying received messages on the screen
    void PrintVal1(int val) {
        textField.text += val.ToString() + "\n";
    }
    void PrintVal2(float val) {
        textField.text += val.ToString() + "\n";
    }
    void PrintVal3(string val) {
        textField.text += val + "\n";
    }
    void PrintVal4(byte[] val) {
        foreach (var item in val) {
            textField.text += item;
            textField.text += ",";
        }
        textField.text += "\n";
    }    
    public void GetBondedDevices() {
        devices = BluetoothForAndroid.GetBondedDevices();
        if (devices != null) {
            for (int i = 0; i < devices.Length; i++) {
                textField.text += devices[i].name + "   ";
                textField.text += devices[i].address;
                textField.text += "\n";
            }
        }
    }

    // 自分で追加したメソッド
    public void sendGpsSign(){
        textField.text += "send gps sign [g]\n";
        byte[] array = Encoding.UTF8.GetBytes ("g");
        BluetoothForAndroid.WriteMessage (array); 
        isGpsSign = true;
    }

    void WriteBitMapFile(byte[] val){

        textField.text += Encoding.UTF8.GetString(val) + "\n";

        // Activityデータの合図であるかかどうか識別
        for(int i=0; i<10; ++i){
            if(val[i] != 'a'){
                break;
            }
            if(i == 9){
                isActivitySign = true;
                return;
            }
        }

        if(isActivitySign){
            //FileNameNum.txtを読み込んで、それをもとに書き込みファイルのパスを作成
            if(val[0] == 'd'){       
                readPath = appManager.dataPath + @"/Datas/FileNameNum.txt";
                nameNum = File.ReadAllLines(readPath);
                writePath = appManager.dataPath + @"/Datas/BitMapFile" + nameNum[0] + ".txt";
                textField.text += "Make BitMapFile : " + nameNum[0] + "\n";
            }

            //アクティビティデータをファイルに書き込み
            using (StreamWriter writer = new StreamWriter(appManager.activityFilePath, true))
            {
                if(commaCount == -1){
                    dateTime = DateTime.Now;
                    writer.Write(dateTime.ToShortDatePattern());
                    writer.Write(",");
                    writer.Write(dateTime.ToShortTimePattern());
                    writer.Write(",");
                    if(val[0] == 'd'){
                        isDanger = true;
                    }
                    commaCount += 1;
                }

                writer.Write(Encoding.UTF8.GetString(val));
                foreach(var item in val){
                    if(item == ','){
                        commaCount += 1;
                    }
                }
                
                if(commaCount == 3){
                    if(isDanger){
                        writer.Write(nameNum[0]);
                        isDanger = false;
                    }
                    writer.Write("\n");
                    hasNewActivity = true;
                    commaCount = -1;
                    isActivitySign = false;
                    textField.text += "Get Activity Data \n";

                }
                
            }

        }else if(isGpsSign){
            // 現在の位置情報取得
            textField.text += "get gps data and send weather data \n";
            str = Encoding.UTF8.GetString(val);
            textField.text += "gps : " + str + "\n";

            int latInteger = (int)float.Parse(str.Split(',')[0])/100;
            float latFloat = latInteger + (float.Parse(str.Split(',')[0]) - (float)latInteger * 100.0f)/60.0f;
            int lonInteger = (int)float.Parse(str.Split(',')[1])/100;
            float lonFloat = lonInteger + (float.Parse(str.Split(',')[1]) - (float)lonInteger * 100.0f)/60.0f;
            googlemap.lat = latFloat;
            googlemap.lng = lonFloat;
            googlemap.Build();

            byte[] array = Encoding.UTF8.GetBytes ("w");
            BluetoothForAndroid.WriteMessage (array); 

            weather.URL = "https://api.openweathermap.org/data/2.5/weather?lat=" + str.Split(',')[0] + "&lon=" + str.Split(',')[1] + "&appid=35f0730fca9034b3a7addce34d8be98a";
            weather.StartCoroutine("GET");
            
            string weatherStr = "";
            weatherStr = weather.weatherStr;
            byte[] array2 = Encoding.UTF8.GetBytes (weatherStr);
            BluetoothForAndroid.WriteMessage (array2); 

            textField.text += "weather : " + weatherStr + "\n";

            isGpsSign = false;

        }else{
            // 画像データの合図であるかかどうか識別
            for(int i=5; i<10; ++i){
                textField.text += "waiting image sign ";
                if(val[i] != 'p'){
                    break;
                }
                if(i == 9){
                    isImageSign = true;
                }
            }

            if(isImageSign){
                textField.text += "Last lineCount " + lineCount + " : receive " + val[0] + " : ResetCount\n";
                lineCount = 0;
                byteCount = 0;
                isImageSign = false;

                //書き込みファイル作成
                File.Create(writePath);

                //FileNameNum.txtの値を1増やす
                int temp = Int32.Parse(nameNum[0]);
                temp += 1;
                File.WriteAllText(readPath, temp.ToString(), Encoding.UTF8);
            }else{
                lineCount += 1;

                // 画像データの書き込み
                using (StreamWriter writer = new StreamWriter(writePath, true))
                {
                    foreach (var item in val) {
                        writer.Write(item);
                        writer.Write(",");
                        byteCount += 1;
                        if(byteCount == 640){
                            textField.text += lineCount + " ";
                            writer.Write("\n");
                            byteCount = 0;
                        }
                    }
                }
            }
            
        }


        
        
    }

    // methods for displaying events on the screen
    void PrintEvent1() {
        textField.text += "Adapter enabled" + "\n";
    }
    void PrintEvent2() {
        textField.text += "Adapter disabled" + "\n";
    }
    void PrintEvent3() {
        textField.text += "The device is connected" + "\n";
    }
    void PrintEvent4() {
        textField.text += "Device lost connection" + "\n";
    }
    void PrintEvent5() {
        textField.text += "Server is running" + "\n";
    }
    void PrintEvent6() {
        textField.text += "Server stopped" + "\n";
    }
    void PrintEvent7() {
        textField.text += "Attempt to connect to server" + "\n";
    }
    void PrintEvent8() {
        textField.text += "Connection to the server failed" + "\n";
    }
    void PrintDeviceData(string deviceData) {
        string[] btDevice = deviceData.Split(new char[] { ',' });
        textField.text += btDevice[0] + "   ";
        textField.text += btDevice[1] + "\n";
        lastConnectedDeviceAddress = btDevice[1];
    }

    // method for cleaning the log
    public void ClearLog() {
        textField.text = "";
    }
}
