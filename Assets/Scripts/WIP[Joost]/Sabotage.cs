using UnityEngine;

public class SabotageTool : MonoBehaviour
{
    public void FireSabotage()
    {
        foreach (var item in PlacedItemManager.Instance.PlacedItems)
        {
            if (item != null)
            {
                // 🔥 Example: turn red or mark as burning
                item.GetComponent<Renderer>().material.color = Color.red;
                Debug.Log("🔥 Fire sabotage hit: " + item.name);
            }
        }
    }

    public void BombSabotage()
    {
        foreach (var item in new System.Collections.Generic.List<GameObject>(PlacedItemManager.Instance.PlacedItems))
        {
            if (item != null)
            {
                Debug.Log("💣 Bomb destroyed: " + item.name);
                Destroy(item);
            }
        }
        PlacedItemManager.Instance.PlacedItems.Clear();
    }

    public void TargetSabotage()
    {
        // Example: only destroy tagged target items
        foreach (var item in new System.Collections.Generic.List<GameObject>(PlacedItemManager.Instance.PlacedItems))
        {
            if (item != null && item.CompareTag("Item/Target"))
            {
                Debug.Log("🎯 Target destroyed: " + item.name);
                Destroy(item);
                PlacedItemManager.Instance.Remove(item);
            }
        }
    }

    public void BreakSabotage()
    {
        // Example: disable colliders to simulate break
        foreach (var item in PlacedItemManager.Instance.PlacedItems)
        {
            if (item != null && item.TryGetComponent<Collider>(out var col))
            {
                col.enabled = false;
                Debug.Log("🔨 Break sabotage disabled: " + item.name);
            }
        }
    }
}
