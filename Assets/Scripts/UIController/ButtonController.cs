using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonStatusSwitcher : MonoBehaviour
{
    public Button button; // 按钮对象

    private Image image_origin; // 按钮内部的 Image 组件
    public Sprite image_off; // 第一张图片
    public Sprite image_on; // 第二张图片
    private bool isButtonOn = false; // 跟踪当前显示的图片

    public GameObject popup_target; // 弹窗对象
    private bool isPopupOpen = false; // 弹窗是否打开的状态
    private CanvasGroup popupCanvasGroup; // 弹窗的 CanvasGroup 组件
    private Image popupTargetImage; // 弹窗的 Image 组件
    private Material popupMaterial; // 弹窗的材质
    public float fadeDuration = 0.5f; // 渐变动画时长

    void Start()
    {
        // 查找子组件中的 Image
        image_origin = transform.Find("icon")?.GetComponent<Image>();
        if (image_origin == null)
        {
            Debug.LogError("Image component not found in child object named 'icon'.");
            return;
        }

        // 确保 Button 和 Image 都已赋值
        if (button == null || image_off == null || image_on == null)
        {
            Debug.LogError("Button, image_off, or image_on not assigned in the inspector.");
            return;
        }

        // 初始化图片和状态
        image_origin.sprite = image_off;
        isButtonOn = false;
        
        // 获取 CanvasGroup 组件来控制透明度
        popupCanvasGroup = popup_target.GetComponent<CanvasGroup>();
        if (popupCanvasGroup == null)
        {
            popupCanvasGroup = popup_target.AddComponent<CanvasGroup>(); // 如果没有 CanvasGroup 组件，添加一个
        }

        // 设置初始透明度为 0（不可见）
        popupCanvasGroup.alpha = 0;

        // 获取Image组件,获取弹窗的材质
        popupTargetImage = popup_target.GetComponent<Image>();
        if (popupTargetImage == null)
        {
            // popupTargetImage.enabled = false;
            popupMaterial = popupTargetImage.material;
        }

        // 添加按钮点击事件监听器
        button.onClick.AddListener(SwitchImage);
    }

    void SwitchImage()
    {
        // 切换图片和状态
        isButtonOn = !isButtonOn;
        image_origin.sprite = isButtonOn ? image_on : image_off;

        // 切换弹窗显示状态
        isPopupOpen = isButtonOn;
        TogglePopup();
    }

    // 切换弹窗的显示状态
    void TogglePopup()
    {

        // 获取Image组件,获取弹窗的材质
        popupTargetImage = popup_target.GetComponent<Image>();
        if (popupTargetImage != null)
        {
            popupMaterial = popupTargetImage.material; // 确保获取 Image 的材质
        }
        else
        {
            Debug.LogError("Popup target does not have an Image component.");
        }
        
        if (isPopupOpen)
        {
            
            popup_target.SetActive(true);

            // 弹窗从透明到不透明（渐现）
            popupCanvasGroup.DOFade(1, fadeDuration).SetEase(Ease.OutQuad);
            // 使用 DOTween 动画化 _Blur 属性的值
            DOTween.To(() => popupMaterial.GetFloat("_Blur"), x => popupMaterial.SetFloat("_Blur", x), 0.8f, fadeDuration)
                .SetEase(Ease.InOutQuad); // 使用缓动效果 (可根据需要选择不同的效果)
        }
        else
        {
            // 弹窗从不透明到透明（渐隐）
            // popupTargetImage.enabled = false;
            popupCanvasGroup.DOFade(0, fadeDuration).SetEase(Ease.InQuad).OnKill(() => popup_target.SetActive(false));
            DOTween.To(() => popupMaterial.GetFloat("_Blur"), x => popupMaterial.SetFloat("_Blur", x), 0f, fadeDuration)
                .SetEase(Ease.InOutQuad); // 使用缓动效果 (可根据需要选择不同的效果)
        }
    }
}
