using UnityEngine;

public class Flower : MonoBehaviour
{
    private Vector3 initialPosition;   // 初期位置

    private void Start()
    {
        initialPosition = transform.position; // 現在の位置を初期位置として設定
    }

    public void Rotate(int joyValue)
    {   Debug.Log("flowerScriptのRotateが呼ばれたよ");
        Debug.Log("Rotateに来た時joyの値は" + joyValue);

        float joyValueFloat = (float)joyValue;
        // 毎フレーム、オブジェクトをjoyの速度でX軸を中心に回転させる
        transform.Rotate(Vector3.forward * joyValue * Time.deltaTime);
    }
}
