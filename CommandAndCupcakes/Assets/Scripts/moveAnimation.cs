using UnityEngine;
using System.Collections;

public class moveAnimation : MonoBehaviour
{
    Animator anim;
    int movehash = Animator.StringToHash("Speed");
    int attackhash = Animator.StringToHash("Attack");
    int searchhash = Animator.StringToHash("Search");
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = Mathf.Abs(Input.GetAxis("Vertical") + Mathf.Abs(Input.GetAxis("Horizontal")));
        anim.SetFloat("Speed", move);
    }
    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.gameObject.CompareTag("interactable"))
        {
            anim.SetBool(searchhash, true);
        }
        else
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
    //Get message when combat is down to turn attackhash to false. 
}