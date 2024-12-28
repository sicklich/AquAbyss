using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonIconController : MonoBehaviour
{
    public Button button; // 按钮对象

    private Image image_origin; // 按钮内部的 Image 组件
    public Sprite image_off; // Off图片
    public Sprite image_on; // On图片
    // public bool isButtonOn = false; // 跟踪当前显示的图片

    private bool _isButtonOn;
    public bool isButtonOn
    {
        get => _isButtonOn;
        set
        {
            if (_isButtonOn != value)
            {
                _isButtonOn = value;
                OnButtonStateChanged?.Invoke(_isButtonOn); // 触发事件
            }
        }
    }

    // 定义状态改变事件
    public event System.Action<bool> OnButtonStateChanged;

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

        // 添加按钮点击事件监听器 - 由于按键切换图标事件在 PopupManager 中被具体调用，故关闭此处的点击事件功能
        // button.onClick.AddListener(SwitchImage);
    }

    public void SwitchImage()
    {
        Debug.Log("切换图标");
        
        // 切换图片和状态
        isButtonOn = !isButtonOn;
        image_origin.sprite = isButtonOn ? image_on : image_off;
    }
}
