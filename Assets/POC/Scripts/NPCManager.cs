using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [System.Serializable]
    public class NPCData
    {
        public GameObject npcPrefab;
        public Vector3 spawnPosition;
        public Transform[] waypoints;
    }

    [Header("NPC Setup")]
    public NPCData[] npcList;

    public void SpawnNPC()
    {
        foreach (NPCData data in npcList)
        {
            if (data.npcPrefab == null || data.waypoints.Length == 0) continue;

            GameObject npcInstance = Instantiate(data.npcPrefab, data.spawnPosition, Quaternion.identity);
            NPCWaypointRandomBehavior npcScript = npcInstance.GetComponent<NPCWaypointRandomBehavior>();

            if (npcScript != null)
            {
                npcScript.waypoints = data.waypoints;
            }
        }
    }
}
