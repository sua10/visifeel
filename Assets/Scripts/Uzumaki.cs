using UnityEngine;

public class Uzumaki : MonoBehaviour
{ private Vector3 initialPosition;   // 初期位置

    private void Start()
    {
        initialPosition = transform.position; // 現在の位置を初期位置として設定
    }

    public void Rotate(int sadValue)
    {   Debug.Log("uzumakiScriptのRotateが呼ばれたよ");
        Debug.Log("Rotateに来た時sorrowの値は" + sadValue);

        float sadValueFloat = (float)sadValue;
        // 毎フレーム、オブジェクトをjoyの速度でX軸を中心に回転させる
        transform.Rotate(Vector3.forward * sadValue * Time.deltaTime);
    }
}
