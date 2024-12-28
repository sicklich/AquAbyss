using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GradientBackground : MonoBehaviour
{
    public Color startColor = Color.black; // 起始颜色（实色部分）
    public Color endColor = new Color(0, 0, 0, 0); // 渐变结束颜色（透明）
    [Range(0f, 1f)] public float gradientStartPercentage = 0.5f; // 渐变开始的百分比（0.0 - 1.0）

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        UpdateGradient();
    }

    private void UpdateGradient()
    {
        Texture2D gradientTexture = new Texture2D(256, 1);
        gradientTexture.wrapMode = TextureWrapMode.Clamp;

        // 计算渐变起始点的像素位置
        int gradientStartPixel = Mathf.RoundToInt(gradientStartPercentage * 255);

        for (int i = 0; i < 256; i++)
        {
            Color pixelColor;

            if (i < gradientStartPixel)
            {
                // 渐变前的纯色部分
                pixelColor = startColor;
            }
            else
            {
                // 渐变部分
                float t = (i - gradientStartPixel) / (255f - gradientStartPixel);
                pixelColor = Color.Lerp(startColor, endColor, t);
            }

            gradientTexture.SetPixel(i, 0, pixelColor);
        }
        gradientTexture.Apply();

        Sprite gradientSprite = Sprite.Create(
            gradientTexture,
            new Rect(0, 0, gradientTexture.width, gradientTexture.height),
            new Vector2(0.5f, 0.5f)
        );

        image.sprite = gradientSprite;
        image.color = Color.white; // 确保颜色为白色，不影响纹理
        image.type = Image.Type.Simple;
    }
}
