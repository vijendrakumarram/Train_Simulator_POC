using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    float deltaTime = 0.0f;

    public TextMeshProUGUI m_TextMeshPro;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int width = Screen.width, height = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(10, 10, width, height * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 2 / 50;
        style.normal.textColor = Color.white;

        float fps = 1.0f / deltaTime;
        string text = $"FPS: {Mathf.Ceil(fps)}";
        m_TextMeshPro.text = text;

        //GUI.Label(rect, text, style);
    }
}
