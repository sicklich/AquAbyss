// 简单的跟随脚本（附加到相机上）
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 角色的位置

    private Vector3 offset;

    void Start()
    {
        // 设置相机的初始偏移量（相对位置）
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // 更新相机位置，使其跟随角色
        transform.position = player.position + offset;
    }
}