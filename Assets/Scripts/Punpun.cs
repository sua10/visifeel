using UnityEngine;

public class Punpun : MonoBehaviour
{   //public float amplitude = 1.0f;      // 振幅
    public float frequency = 1.0f;     // 周波数
    private Vector3 initialPosition;   // 初期位置

    private void Start()
    {
        initialPosition = transform.position; // 現在の位置を初期位置として設定
    }

   public void Move(int angerValue)
    { 
        Debug.Log("Moveに来た時angerの値は" + angerValue);
        Debug.Log("punpunScriptのMoveが呼ばれたよ");

        // angerValueをfloatに変換して振幅としてそのまま使用
        float angerValueFloat = (float)angerValue;
        float newY = initialPosition.y + angerValueFloat * Mathf.Sin(frequency * Time.time);

        // 新しい位置を設定
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
   
    }
}
