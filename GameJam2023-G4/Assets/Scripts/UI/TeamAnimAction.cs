using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamAnimAction : MonoBehaviour
{
    public Animator anim;

    public void StartAction()
    {
        anim.SetTrigger("Press");
    }
}
