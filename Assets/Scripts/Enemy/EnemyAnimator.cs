using UnityEngine;
using System.Collections;

public class EnemyAnimator : MonoBehaviour
{
    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Run Forward", true);
    }
}