using UnityEngine;
using System.Collections;

public class moveAnimation : MonoBehaviour
{
    Animator anim;
    int movehash = Animator.StringToHash("Speed");
    int attackhash = Animator.StringToHash("Attack");
    int searchhash = Animator.StringToHash("Search");
    int stophash = Animator.StringToHash("Stopped");
    float move;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", move);
        if (move == 0)
        {
            anim.SetBool(stophash, true);
        }
        else
            anim.SetBool(stophash, false);
    }
    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.gameObject.CompareTag("interactable"))
        {
            anim.SetBool(searchhash, true);
        }
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Search"))
        {
            anim.SetBool(searchhash, false);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player2"))
        {
            anim.SetBool(attackhash, true);
        }
    }

    void setMove(float move)
    {
        this.move = move;
    }

    void setAllFalse()
    {
        anim.SetBool(movehash, false);
        anim.SetBool(attackhash, false);
        anim.SetBool(searchhash, false);
    }

    //Get message when combat is down to turn attackhash to false. 
}