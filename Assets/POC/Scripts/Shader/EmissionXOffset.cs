using UnityEngine;

public class HDRPEmissionOffset : MonoBehaviour
{
    public Renderer targetRenderer;
    public float scrollSpeedX = 0.2f;

    private Material mat;
    private Vector4 emissive_ST;

    void Start()
    {
        mat = targetRenderer.material;

        // Optional: enable emission keyword
        mat.EnableKeyword("_EMISSIVE_COLOR_MAP");

        // Get current emissive map settings
        emissive_ST = mat.GetVector("_EmissiveColorMap_ST");
    }

    void Update()
    {
        // Update only the X offset (4th component)
        emissive_ST.z += scrollSpeedX * Time.deltaTime;

        // Apply new offset
        mat.SetVector("_EmissiveColorMap_ST", emissive_ST);
    }
}
