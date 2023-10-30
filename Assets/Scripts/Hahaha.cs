using UnityEngine;

public class Hahaha : MonoBehaviour
{
    //public float amplitude = 1.0f;      // 振幅
    public float frequency = 1.0f;     // 周波数
    private Vector3 initialPosition;   // 初期位置

    private void Start()
    {
        initialPosition = transform.position; // 現在の位置を初期位置として設定
    }

   public void Move(int calmValue)
    { 
        Debug.Log("Moveに来た時calmの値は" + calmValue);
        Debug.Log("hahahaScriptのMoveが呼ばれたよ");

        // calmValueをfloatに変換して振幅としてそのまま使用
        float calmValueFloat = (float)calmValue;
        float newY = initialPosition.y + calmValueFloat * Mathf.Sin(frequency * Time.time);

        // 新しい位置を設定
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
   
    }
}