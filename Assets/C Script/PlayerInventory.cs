using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public List<string> collectedItems = new List<string>();

    public void Collect(string itemName)
    {
        collectedItems.Add(itemName);
        Debug.Log("You now have: " + collectedItems.Count + " item(s)");
    }
}
