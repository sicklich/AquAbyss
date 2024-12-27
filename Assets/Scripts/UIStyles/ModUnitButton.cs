using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModUnitEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string borderColorHex = "#FF0000"; // 鼠标进入时边框目标颜色（HEX）
    public float offsetY = 10f;              // 鼠标进入时的向上偏移值
    public float transitionTime = 0.5f;      // 过渡动画时间

    private RectTransform rectTransform;     // 用于偏移
    private Outline outline;                 // 用于边框变色
    private Vector3 originalPosition;        // 原始位置
    private Color originalOutlineColor;      // 原始边框颜色
    private bool isLocked = false;           // 是否锁定状态
    private Coroutine activeCoroutine;       // 当前运行的协程

    void Start()
    {
        // 获取RectTransform和Outline组件
        rectTransform = GetComponent<RectTransform>();
        outline = GetComponent<Outline>();

        if (rectTransform == null || outline == null)
        {
            Debug.LogError("RectTransform或Outline组件未找到！");
            return;
        }

        // 保存初始状态
        originalPosition = rectTransform.localPosition;
        originalOutlineColor = outline.effectColor; // 获取初始边框颜色
    }

    // 当鼠标悬停到对象上时
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLocked) return; // 锁定状态不响应鼠标悬停

        if (ColorUtility.TryParseHtmlString(borderColorHex, out Color targetColor))
        {
            // 启动协程进行偏移和变色
            StartTransition(originalPosition + new Vector3(0, offsetY, 0), targetColor);
        }
    }

    // 当鼠标移出对象时
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isLocked) return; // 锁定状态不响应鼠标移出

        // 启动协程恢复位置和颜色
        StartTransition(originalPosition, originalOutlineColor);
    }

    // 当鼠标点击对象时
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLocked)
        {
            // 解锁：恢复初始位置和颜色
            isLocked = false;
            StartTransition(originalPosition, originalOutlineColor);
        }
        else
        {
            // 锁定：保持当前的最终位置和颜色
            isLocked = true;
            if (ColorUtility.TryParseHtmlString(borderColorHex, out Color targetColor))
            {
                StartTransition(originalPosition + new Vector3(0, offsetY, 0), targetColor);
            }
        }
    }

    // 开始偏移和变色的协程
    private void StartTransition(Vector3 targetPosition, Color targetOutlineColor)
    {
        // 如果已有动画正在进行，先停止
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
        activeCoroutine = StartCoroutine(TransitionEffect(targetPosition, targetOutlineColor));
    }

    // 偏移和变色的协程
    private IEnumerator TransitionEffect(Vector3 targetPosition, Color targetOutlineColor)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = rectTransform.localPosition;
        Color startOutlineColor = outline.effectColor;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / transitionTime;

            // 插值计算当前位置和边框颜色
            rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            outline.effectColor = Color.Lerp(startOutlineColor, targetOutlineColor, progress);

            yield return null;
        }

        // 确保最终位置和颜色为目标值
        rectTransform.localPosition = targetPosition;
        outline.effectColor = targetOutlineColor;
    }
}
