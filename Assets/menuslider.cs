using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuslider : MonoBehaviour
{
    public GameObject PanelMenu;
    public void ShowHideMenu()
    {
        if (PanelMenu != null)
        {
            Animator animator = PanelMenu.GetComponent<Animator>();
            if (animator != null)
            {
                bool isActive = animator.GetBool("show");
                animator.SetBool("show", !isActive);
            }
        }
    }
}
