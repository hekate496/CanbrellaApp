using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class SerialHandler : MonoBehaviour
{
    // public delegate void SerialDataReceivedEventHandler(string message);
    // public event SerialDataReceivedEventHandler OnDataReceived;
    public delegate void SerialDataReceivedEventHandler(int byteMessage);
    public event SerialDataReceivedEventHandler OnByteDataReceived;

    //ポート名
    //例
    //Linuxでは/dev/ttyUSB0
    //windowsではCOM1
    //Macでは/dev/tty.usbmodem1421など
    public string portName = "COM1";
    public int baudRate    = 9600;

    private SerialPort serialPort_;
    private Thread thread_;
    private bool isRunning_ = false;

    private string message_;
    private int byteMessage_;
    private bool isNewMessageReceived_ = false;

    void Awake()
    {
        Open();
    }

    void Update()
    {
        if (isNewMessageReceived_) {
            //OnDataReceived(message_);
            OnByteDataReceived(byteMessage_);
        }
        isNewMessageReceived_ = false;

        if(Input.GetKeyDown(KeyCode.C)){
            Close();
        }
    }

    void OnDestroy()
    {
        Close();
    }

    private void Open()
    {
        serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
         //または
         //serialPort_ = new SerialPort(portName, baudRate);
        serialPort_.Open();
        serialPort_.ReadTimeout = 100;

        isRunning_ = true;

        //thread_ = new Thread(Read);
        thread_ = new Thread(ReadByte);
        thread_.Start();
    }

    private void Close()
    {
        isNewMessageReceived_ = false;
        isRunning_ = false;

        if (thread_ != null && thread_.IsAlive) {
            thread_.Join();
        }

        if (serialPort_ != null && serialPort_.IsOpen) {
            serialPort_.Close();
            serialPort_.Dispose();
        }
    }

    private void Read()
    {
        while (isRunning_ && serialPort_ != null && serialPort_.IsOpen) {
            try {
                message_ = serialPort_.ReadLine();
                isNewMessageReceived_ = true;
            } catch (System.Exception e) {
                Debug.LogWarning(e.Message);
            }
        }
    }

    private void ReadByte()
    {
        while (isRunning_ && serialPort_ != null && serialPort_.IsOpen) {
            try {
                byteMessage_ = serialPort_.ReadByte();
                isNewMessageReceived_ = true;
            } catch (System.Exception e) {
                Debug.LogWarning(e.Message);
            }
        }
    }

    public void Write(string message)
    {
        try {
            serialPort_.Write(message);
        } catch (System.Exception e) {
            Debug.LogWarning(e.Message);
        }
    }

}