using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System;

public class PopupManager : MonoBehaviour
{
    public GameObject[] popupsArray; // 所有弹窗引用
    public GameObject[] buttonsArray; // 所有按钮引用（用于状态控制）

    private int activePopupIndex = -1; // 当前激活的弹窗索引，-1 表示无激活弹窗

    public GameObject button_target; // 按键对象

    public GameObject popup_target; // 弹窗对象
    private CanvasGroup popupCanvasGroup; // 弹窗的 CanvasGroup 组件
    private Image popupTargetImage; // 弹窗的 Image 组件
    private Material popupMaterial; // 弹窗的材质
    public float fadeDuration = 0.5f; // 渐变动画时长

    public void EquipPopwinObject(int index)
    {
        // 获取指定弹窗的GameObject
        popup_target = popupsArray[index];
        
        // 获取 CanvasGroup 组件来控制透明度
        popupCanvasGroup = popup_target.GetComponent<CanvasGroup>();
        if (popupCanvasGroup == null)
        {
            popupCanvasGroup = popup_target.AddComponent<CanvasGroup>(); // 如果没有 CanvasGroup 组件，添加一个
            // 设置初始透明度为 0（不可见）
            popupCanvasGroup.alpha = 0;
        }else{
            popupCanvasGroup.alpha = 0;
        }
        // 获取 CanvasGroup 组件来控制透明度
        
        // 获取Image组件,获取弹窗的材质
        popupTargetImage = popup_target.GetComponent<Image>();
        if (popupTargetImage == null)
        {
            Debug.Log("无Image");
        }else{
            popupMaterial = popupTargetImage.material;
        }
        // 获取Image组件,获取弹窗的材质
    }

    public void TogglePopup(int index){

        if(index == -1 && activePopupIndex != index){
            // 关闭弹窗
            // 触发按键图标变化
            ToggleButtonIconSwitch(activePopupIndex);
            ClosePopup(activePopupIndex);
            // 关闭弹窗，则索引归位
            activePopupIndex = -1;

        }else if(activePopupIndex == index && index != -1){
            // 关闭弹窗
            // 触发按键图标变化
            ToggleButtonIconSwitch(index);

            // 关闭弹窗
            ClosePopup(index);
            // 关闭弹窗，则索引归位
            activePopupIndex = -1;

        }else if(activePopupIndex == -1 && index != -1){
            // 直接打开弹窗
            EquipPopwinObject(index);
            // 触发按键图标变化
            ToggleButtonIconSwitch(index);

            // 打开新的弹窗
            OpenPopup(index);
            // 弹窗打开，索引变化
            activePopupIndex = index;
        }else if(activePopupIndex != -1 && activePopupIndex != index){
            // 首先关闭弹窗
            EquipPopwinObject(activePopupIndex);
            // 触发按键图标变化
            ToggleButtonIconSwitch(activePopupIndex);
            ToggleButtonIconSwitch(index);
            
            fadeDuration = fadeDuration/2;

            // 关闭弹窗
            ClosePopup(activePopupIndex);
            // 关闭弹窗，则索引归位
            activePopupIndex = -1;

            // 然后打开新弹窗
            StartCoroutine(DelayOpen(fadeDuration, index));
        }
    }

    private IEnumerator DelayOpen(float delayTimes, int index)
    {
        yield return new WaitForSeconds(delayTimes);

        EquipPopwinObject(index);

        // 打开新的弹窗
        OpenPopup(index);
        // 弹窗打开，索引变化
        activePopupIndex = index;

        fadeDuration = fadeDuration*2;
    }

    public void ToggleButtonIconSwitch(int index)
    {
        // 获取指定按键的GameObject
        button_target = buttonsArray[index];
        // 触发按键图标变色
        button_target.GetComponent<ButtonIconController>().SwitchImage();
    }

    private void OpenPopup(int index)
    {
        Debug.Log("Opening popup with index: " + index);

        // 
        popup_target.SetActive(true); // 激活展示
        // buttonsArray[index].GetComponent<UnityEngine.UI.Button>().interactable = false; // 禁用按钮以表示激活状态
        
        // 弹窗从透明到不透明（渐显）
        popupCanvasGroup.alpha = 0f;

        popupCanvasGroup.DOFade(1f, fadeDuration)
        .SetEase(Ease.OutQuad);

        // 设置初始的 _Blur 值为 0
        popupMaterial.SetFloat("_Blur", 0f);
        // 使用 DOTween 动画化 _Blur 属性的值
        DOTween.To(() => popupMaterial.GetFloat("_Blur"), x => popupMaterial.SetFloat("_Blur", x), 0.8f, fadeDuration)
        .SetEase(Ease.InOutQuad); // 使用缓动效果 (可根据需要选择不同的效果)
    }

    private void ClosePopup(int index)
    {
        Debug.Log("关闭弹窗");
        // buttonsArray[index].GetComponent<UnityEngine.UI.Button>().interactable = true; // 恢复按钮状态

        // 弹窗从不透明到透明（渐隐）
        // popupTargetImage.enabled = false;
        popupCanvasGroup.alpha = 1f;
        popupCanvasGroup.DOFade(0, fadeDuration)
        .SetEase(Ease.InQuad)
        .OnKill(() => popup_target.SetActive(false));

        DOTween.To(() => popupMaterial.GetFloat("_Blur"), x => popupMaterial.SetFloat("_Blur", x), 0f, fadeDuration)
        .SetEase(Ease.InOutQuad); // 使用缓动效果 (可根据需要选择不同的效果)
    }
}
