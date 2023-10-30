using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class EmpathManager : MonoBehaviour
{
    [SerializeField]
    private string m_subscriptionKey = string.Empty;

    [SerializeField]
    private Text empathResult;

    private AudioClip micClip;
    private float[] microphoneBuffer;
    private int position;
    private bool isRecording;
    public int maxRecordingTime;
    private const int samplingFrequency = 11025;
    private string micDeviceName;

    //自分で追加したやつ
    static bool Hahaha_isMoving = false; // hahahaが上下運動中かどうかのフラグ
    static bool Punpun_isMoving = false; // punpunが上下運動中かどうかのフラグ
    static bool Flower_isMoving = false; // flowerが上下運動中かどうかのフラグ
    static bool Uzumaki_isMoving = false; // uzumakiが上下運動中かどうかのフラグ
    static bool Neiron_isMoving = false; // neironが上下運動中かどうかのフラグ

    private Hahaha hahahaScript; // Hahaha オブジェクトのスクリプト
    private Punpun punpunScript; // Punpun オブジェクトのスクリプト
    private Flower flowerScript; // Flower オブジェクトのスクリプト
    private Uzumaki uzumakiScript;//Uzumaki オブジェクトのスクリプト
    private Neiron neironScript;//neiron オブジェクトのスクリプト

    private int calm;
    private int anger;
    private int joy;
    private int sorrow;
    private int energy;
    //

    private void Start()
    {
        // Hahaha オブジェクトと Punpun オブジェクトのスクリプトを取得
        hahahaScript = FindObjectOfType<Hahaha>();
        punpunScript = FindObjectOfType<Punpun>();
        flowerScript = FindObjectOfType<Flower>();
        uzumakiScript = FindObjectOfType<Uzumaki>();
        neironScript = FindObjectOfType<Neiron>();
    }


    public void ButtonClicked()
    {
        if (!isRecording)
        {
            StartCoroutine(RecordingForEmpathAPI());
        }
    }

    public IEnumerator RecordingForEmpathAPI()
    {
        RecordingStart();

        yield return new WaitForSeconds(maxRecordingTime);

        RecordingStop();

        yield return null;
    }

    public void RecordingStart()
    {
        StartCoroutine(WavRecording(micDeviceName, maxRecordingTime, samplingFrequency));
    }

    public void RecordingStop()
    {
        isRecording = false;
        position = Microphone.GetPosition(null);
        Microphone.End(micDeviceName);
        Debug.Log("Recording end");
        byte[] empathByte = WavUtility.FromAudioClip(micClip);
        StartCoroutine(Upload(empathByte));
    }

    IEnumerator Upload(byte[] wavbyte)
    {
        WWWForm form = new WWWForm();
        form.AddField("apikey", m_subscriptionKey);
        form.AddBinaryData("wav", wavbyte);
        string receivedJson = null;

        using (UnityWebRequest www = UnityWebRequest.Post("https://api.webempath.net/v2/analyzeWav", form))     {         yield return www.SendWebRequest();          if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)         {             Debug.Log(www.error);         }         else         {             receivedJson = www.downloadHandler.text;             Debug.Log(receivedJson);         }     }      EmpathData empathData = ConvertEmpathToJson(receivedJson);     empathResult.text = ConvertEmpathDataToString(empathData);
    }


    public IEnumerator WavRecording(string micDeviceName, int maxRecordingTime, int samplingFrequency)
    {
        Debug.Log("Recording start");
        //Recording開始
        isRecording = true;

        //Buffer
        microphoneBuffer = new float[maxRecordingTime * samplingFrequency];
        //録音開始
        micClip = Microphone.Start(deviceName: micDeviceName, loop: false,
                                   lengthSec: maxRecordingTime, frequency: samplingFrequency);
        yield return null;
    }

    public EmpathData ConvertEmpathToJson(string json)
    {
        Debug.AssertFormat(!string.IsNullOrEmpty(json), "Jsonの取得に失敗しています。");

        EmpathData empathData = null;

        try
        {
            empathData = JsonUtility.FromJson<EmpathData>(json);
        }
        catch (System.Exception i_exception)
        {
            Debug.LogWarningFormat("Jsonをクラスへ変換することに失敗しました。exception={0}", i_exception);
            empathData = null;
        }
        return empathData;
    }

    public string ConvertEmpathDataToString(EmpathData empathData)
    {
        string result;
        if(empathData.error == 0)
        {
            calm = empathData.calm;
            anger = empathData.anger;
            joy = empathData.joy;
            sorrow = empathData.sorrow;
            energy = empathData.energy;

            Hahaha_isMoving = true;
            Punpun_isMoving = true;
            Flower_isMoving = true;
            Uzumaki_isMoving = true;
            Neiron_isMoving = true;
            //確認中
            Debug.Log("calmは"+calm);
            Debug.Log("Flower_isMoving = " + Flower_isMoving);

            //テキスト出力
            result = " [calm : " + calm + "] ,[anger : " + anger + "], [joy : " + joy + "], [sorrow : " + sorrow + "], [energy : " + energy + "]";
        }
        else
        {
            int error = empathData.error;
            string msg = empathData.msg;
            result = "error : " + error +
                     "\nmsg : " + msg;
        }
        return result;
    }

    private void Update()
    {  
        Debug.Log("Updateが呼ばれたよ");

        // Hahaha オブジェクトを制御
        if (Hahaha_isMoving && hahahaScript != null)
        {
            Debug.Log("hahahaScriptのMoveを呼ぶね");
            Debug.Log("Update calmは"+calm);

            hahahaScript.Move(calm);

            //3秒後にStopHahahaMovementメソッドを呼び出して動きを停止
            Invoke("StopHahahaMovement", 8.0f);
        }
        
        // Punpun オブジェクトを制御
        if (Punpun_isMoving && punpunScript != null)
        {
            Debug.Log("punpunScriptのMoveを呼ぶね");
            Debug.Log("Update angerは"+anger);

            punpunScript.Move(anger);

            //3秒後にStopPunpunMovementメソッドを呼び出して動きを停止
            Invoke("StopPunpunMovement", 8.0f);
        }
        
        // Flower オブジェクトを制御
        if (Flower_isMoving && flowerScript != null)
        {
            Debug.Log("FlowerScriptのRotateを呼ぶね");//出てこない
            Debug.Log("Update joyは"+joy);

            flowerScript.Rotate(joy);

            //3秒後にStopHahahaMovementメソッドを呼び出して動きを停止
            Invoke("StopFlowerMovement", 8.0f);
        }

         // Uzumaki オブジェクトを制御
        if (Uzumaki_isMoving && uzumakiScript != null)
        {
            Debug.Log("UzumakiScriptのRotateを呼ぶね");//出てこない
            Debug.Log("Update sorrowは"+sorrow);

            uzumakiScript.Rotate(sorrow);

            //3秒後にStopHahahaMovementメソッドを呼び出して動きを停止
            Invoke("StopUzumakiMovement", 8.0f);
        }

        // Neiron オブジェクトを制御
        if (Neiron_isMoving && neironScript != null)
        {
            Debug.Log("neironScriptのMoveを呼ぶね");
            Debug.Log("Update energyは"+energy);

            neironScript.Move(energy);

            //3秒後にStopHahahaMovementメソッドを呼び出して動きを停止
            Invoke("StopNeironMovement", 8.0f);
        }
    }

    private void StopHahahaMovement()
    {
        Debug.Log("StopHahahaMovementが呼ばれたよ");
        Hahaha_isMoving = false;
    }

    private void StopPunpunMovement()
    {
        Debug.Log("StopPunpunMovementが呼ばれたよ");
        Punpun_isMoving = false;
    }

    private void StopFlowerMovement()
    {
        Debug.Log("StopFlowerMovementが呼ばれたよ");
        Flower_isMoving = false;
    }

    private void StopUzumakiMovement()
    {
        Debug.Log("StopUzumakiMovementが呼ばれたよ");
        Uzumaki_isMoving = false;
    }
    private void StopNeironMovement()
    {
        Debug.Log("StopNeironMovementが呼ばれたよ");
        Neiron_isMoving = false;
    }

}