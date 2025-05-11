using System.Collections.Generic;
using UnityEngine;

public class PlacedItemManager : MonoBehaviour
{
    public static PlacedItemManager Instance { get; private set; }

    public List<GameObject> PlacedItems { get; private set; } = new List<GameObject>();

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Register(GameObject item)
    {
        PlacedItems.Add(item);
    }

    public void Remove(GameObject item)
    {
        if (PlacedItems.Contains(item))
        {
            PlacedItems.Remove(item);
        }
    }

    public void DestroyAll()
    {
        foreach (GameObject item in PlacedItems)
        {
            if (item != null)
                Destroy(item);
        }
        PlacedItems.Clear();
    }
}
