using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public string targetSceneName = "GameProgress"; // 目标场景名称

    void Start()
    {
        // 获取按钮组件并绑定点击事件
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("SceneSwitcher 脚本需要挂载在一个包含 Button 组件的对象上！");
        }
    }

    // 点击按钮后执行场景切换
    void OnButtonClick()
    {
        // 加载目标场景
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("目标场景名称未设置！");
        }
    }
}
