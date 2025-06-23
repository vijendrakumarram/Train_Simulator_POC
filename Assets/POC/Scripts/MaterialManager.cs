using UnityEngine;
using System.Collections.Generic;

public class MaterialManager : MonoBehaviour
{
    public List<GameObject> models;     // Assign all models in Inspector
    public Material newMaterial;        // Material to set
    public int materialIndexToReplace = 0; // Index of the material slot to update

    public void UpdateSignalMaterial()
    {
        if (newMaterial == null || models == null || models.Count == 0)
        {
            Debug.LogWarning("Assign models and new material.");
            return;
        }

        foreach (GameObject model in models)
        {
            Renderer rend = model.GetComponent<Renderer>();

            if (rend != null)
            {
                Material[] materials = rend.materials;

                if (materialIndexToReplace >= 0 && materialIndexToReplace < materials.Length)
                {
                    materials[materialIndexToReplace] = newMaterial;
                    rend.materials = materials;
                    Debug.Log($"✅ Material updated for: {model.name}");
                }
                else
                {
                    Debug.LogWarning($"❌ Invalid material index for model: {model.name}");
                }
            }
            else
            {
                Debug.LogWarning($"❌ No Renderer found on model: {model.name}");
            }
        }
    }
}
