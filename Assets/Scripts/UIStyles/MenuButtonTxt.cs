using System.Collections; // 用于 IEnumerator 和协程
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string hexColor = "#FFFFFF"; // 鼠标进入时的目标颜色
    public float colorTransitionTime = 0.5f; // 颜色过渡时间，单位为秒

    private string originalText;        // 保存初始文本内容
    private Color originalColor;        // 保存初始颜色
    private TextMeshProUGUI buttonText; // 按钮的文字组件
    private Coroutine colorCoroutine;   // 当前的颜色过渡协程

    void Start()
    {
        // 获取ButtonTxt下的TextMeshProUGUI组件
        buttonText = transform.Find("ButtonTxt").GetComponent<TextMeshProUGUI>();

        if (buttonText == null)
        {
            Debug.LogError("ButtonTxt组件未找到！");
            return;
        }

        // 保存初始文本和颜色
        originalText = buttonText.text;
        originalColor = buttonText.color;
    }

    // 当鼠标悬浮到按钮上
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ColorUtility.TryParseHtmlString(hexColor, out Color targetColor))
        {
            // 开始颜色平滑过渡
            StartColorTransition(targetColor);
        }
        buttonText.text = "" + originalText + "  <<"; // 添加前缀
    }

    // 当鼠标移出按钮
    public void OnPointerExit(PointerEventData eventData)
    {
        // 恢复原始颜色并平滑过渡
        StartColorTransition(originalColor);
        buttonText.text = originalText; // 恢复原始文本
    }

    // 当鼠标点击按钮
    public void OnPointerClick(PointerEventData eventData)
    {
        // Debug.Log("Button Clicked!");
        // 你可以在这里添加更多点击逻辑
    }

    // 开始颜色过渡协程
    private void StartColorTransition(Color targetColor)
    {
        // 如果已有颜色过渡正在进行，先停止
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
        }
        // 启动新的颜色过渡
        colorCoroutine = StartCoroutine(ColorTransitionCoroutine(targetColor));
    }

    // 颜色过渡协程
    private IEnumerator ColorTransitionCoroutine(Color targetColor)
    {
        float elapsedTime = 0f;
        Color startColor = buttonText.color;

        while (elapsedTime < colorTransitionTime)
        {
            elapsedTime += Time.deltaTime;
            // 插值计算当前颜色
            buttonText.color = Color.Lerp(startColor, targetColor, elapsedTime / colorTransitionTime);
            yield return null;
        }

        // 确保最终颜色为目标颜色
        buttonText.color = targetColor;
    }
}
