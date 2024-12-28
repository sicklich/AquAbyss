using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ActionSceneCircleRot : MonoBehaviour
{
    public Button button; // 按钮对象
    public ButtonIconController actionButtonObject;

    public Transform targetObject; // 要移动的对象
    public float moveDistance = 240f; // X 轴移动的距离
    public float moveDuration = 1f; // 移动和旋转的时长

    void Start()
    {
        // 确保有目标对象
        if (actionButtonObject == null)
        {
            Debug.LogError("ButtonIconController is not assigned!");
            return;
        }

        // 订阅按钮状态改变事件
        actionButtonObject.OnButtonStateChanged += HandleButtonStateChanged;
    }

    private void HandleButtonStateChanged(bool isButtonOn)
    {
        RotSceneCircle(isButtonOn); // 根据状态触发动画
    }

    public void RotSceneCircle(bool isButtonOn)
    {
        Debug.Log("场景圆移位");

        if (isButtonOn)
        {
            // 移动到目标位置并同时旋转
            targetObject.DOMoveX(targetObject.position.x + moveDistance, moveDuration)
                .SetEase(Ease.Linear); // 匀速移动

            targetObject.DORotate(new Vector3(0, 0, 360), moveDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear); // 匀速旋转360度
        }
        else
        {
            // 移动到目标位置并同时旋转
            targetObject.DOMoveX(targetObject.position.x - moveDistance, moveDuration)
                .SetEase(Ease.Linear); // 匀速移动

            targetObject.DORotate(new Vector3(0, 0, -360), moveDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear); // 匀速旋转360度
        }
    }
}
