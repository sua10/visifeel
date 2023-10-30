using UnityEngine;

public class Neiron: MonoBehaviour
{
   //public float amplitude = 1.0f;      // 振幅
    public float frequency = 1.0f;     // 周波数
    private Vector3 initialPosition;   // 初期位置

    private void Start()
    {
        initialPosition = transform.position; // 現在の位置を初期位置として設定
    }

   public void Move(int energyValue)
    { 
        Debug.Log("neironScriptのMoveが呼ばれたよ");
        Debug.Log("Moveに来た時のenergyの値は" + energyValue);

        // calmValueをfloatに変換して振幅としてそのまま使用
        float energyValueFloat = (float)energyValue*10;
        float newY = initialPosition.y + energyValueFloat * Mathf.Sin(frequency * Time.time);

        // 新しい位置を設定
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
   
    }
}
