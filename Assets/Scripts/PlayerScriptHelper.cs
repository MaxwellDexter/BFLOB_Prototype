using UnityEngine;

public class PlayerScriptHelper
{
    public const string DEFAULT_LAYER = "Default";
    public const string COLLECTIBLE_LAYER = "Collectible";
    public const string HOOK_LAYER = "Hook";

    public static int GetHittableLayers()
    {
        return LayerMask.GetMask(DEFAULT_LAYER, COLLECTIBLE_LAYER, HOOK_LAYER);
    }

    public static int GetHittableLayersNegated()
    {
        return ~GetHittableLayers();
    }
}
