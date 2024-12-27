using UnityEngine;
using UnityEngine.UI;

public class PackButtonController : MonoBehaviour
{
    public Button button; // 按钮对象
    public PopupManager popupManager; // 引用 PopupManager
    public int popupIndex; // 对应弹窗的索引

    void Start()
    {
        // 添加按钮点击事件监听器
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        popupManager.TogglePopup(popupIndex);
    }
}
