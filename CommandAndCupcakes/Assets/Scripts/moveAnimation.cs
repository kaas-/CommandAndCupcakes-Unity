using UnityEngine;
using System.Collections;

public class moveAnimation : MonoBehaviour
{
    Animator anim;
    int movehash = Animator.StringToHash("Speed");
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        anim = GetComponent<Animator>();
            float move = Mathf.Abs(Input.GetAxis("Vertical") + Mathf.Abs(Input.GetAxis("Horizontal")));
            anim.SetFloat("Speed", move);
    }
}
