using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    public bool collected;
    public string itemName;
    // might need 2d image
    public Image image;

    private void Start()
    {
        gameObject.tag = PlayerScriptHelper.COLLECTIBLE_LAYER;
        gameObject.layer = LayerMask.NameToLayer(PlayerScriptHelper.COLLECTIBLE_LAYER);
        SphereCollider col = gameObject.AddComponent<SphereCollider>();
        col.radius = 1;
    }
}
