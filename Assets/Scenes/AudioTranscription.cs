using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

public class AudioTranscription : MonoBehaviour
{
    public string openaiApiKey;
    public string filePath;
    public string modelName;

    IEnumerator Start()
    {
        string url = "https://api.openai.com/v1/audio/transcriptions";

        WWWForm form = new WWWForm();
        form.AddField("model", modelName);

        byte[] fileBytes = File.ReadAllBytes(filePath);
        form.AddBinaryData("file", fileBytes, Path.GetFileName(filePath), "multipart/form-data");

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + openaiApiKey);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        www.downloadHandler = new DownloadHandlerBuffer();

        foreach (KeyValuePair<string, string> header in headers)
        {
            www.SetRequestHeader(header.Key, header.Value);
        }

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}
