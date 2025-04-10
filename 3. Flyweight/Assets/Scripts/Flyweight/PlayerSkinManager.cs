using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    private Animator animator;
    private RuntimeAnimatorController originalController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator is null)
            return;

        originalController = animator.runtimeAnimatorController;
    }

    public void ChangeSkin(string skinPath)
    {
        SkinFlyweight skin = SkinFactory.GetSkin(skinPath);
        if (skin is not null)
            animator.runtimeAnimatorController = skin.GetController();
    }

    public void ResetToOriginalSkin()
    {
        if (originalController is not null)
            animator.runtimeAnimatorController = originalController;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ResetToOriginalSkin();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeSkin("Skins/Skin2_Controller");
       
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeSkin("Skins/Skin3_Controller");
    }
}
