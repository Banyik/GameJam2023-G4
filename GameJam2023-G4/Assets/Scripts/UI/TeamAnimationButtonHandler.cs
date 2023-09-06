using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TeamAnimationButtonHandler : MonoBehaviour
{
    public string animationToCheck;
    public Animator animator;

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationToCheck) && !gameObject.GetComponent<Button>().interactable)
        {
            Invoke(nameof(EnableButton), 3);
        }
    }

    void EnableButton()
    {
        gameObject.GetComponent<Button>().interactable = true;
    }

}
