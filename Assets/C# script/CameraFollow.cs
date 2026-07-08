using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // 玩家对象引用
    private const float ORTHOGRAPHIC_SIZE = 3f; // 正交大小（固定为3，不可修改）

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (player != null)
        {
            // 跟随玩家位置，保持摄像头原来的Z轴位置
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }

        // 设置正交大小（控制视野大小，固定为3）
        if (cam != null)
        {
            cam.orthographicSize = ORTHOGRAPHIC_SIZE;
        }
    }
}