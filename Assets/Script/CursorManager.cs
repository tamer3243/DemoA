using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // khóa chuột ở giữa màn hình
        Cursor.visible = false;                   // ẩn chuột
    }

    void Update()
    {
        // Nhấn Escape để hiện chuột và unlock
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
           
            Cursor.visible = !Cursor.visible;
        }

 
    }
}