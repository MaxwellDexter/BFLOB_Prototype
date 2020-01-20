using UnityEngine;
using TMPro;

public class InventoryController : MonoBehaviour
{
    private int itemCount;

    public TextMeshProUGUI text;

    public void AddCollectible()
    {
        itemCount++;
    }

    private void Update()
    {
        text.text = itemCount.ToString();
    }
}
