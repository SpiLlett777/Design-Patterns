using System.Collections.Generic;
using UnityEngine;

public class SkinFactory : MonoBehaviour
{
    private static Dictionary<string, SkinFlyweight> skins = new Dictionary<string, SkinFlyweight>();

    public static SkinFlyweight GetSkin(string skinPath)
    {
        if (!skins.ContainsKey(skinPath))
        {
            AnimatorOverrideController controller = Resources.Load<AnimatorOverrideController>(skinPath);
            if (controller is null)
                return null;

            skins[skinPath] = new SkinFlyweight(controller);
        }
        return skins[skinPath];
    }
}
