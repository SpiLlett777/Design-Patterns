using UnityEngine;

public class SkinFlyweight
{
    private AnimatorOverrideController skinController;

    public SkinFlyweight(AnimatorOverrideController controller) => skinController = controller;

    public AnimatorOverrideController GetController() => skinController;
}
