using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {
    Animator anim;
    int idleHash = Animator.StringToHash("idle");
    int interactHash = Animator.StringToHash("Interact");
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
    void Update()
    {
        anim = GetComponent<Animator>();
    }

    public void OnTriggerStay(Collider other) //Runs code as long as the object is touching the trigger
    {
        if (other.gameObject.CompareTag("test") ||other.gameObject.CompareTag("Untagged") && Input.GetKeyDown(KeyCode.E)) //checks if the game object being touched is tagged "interactable" and if the E key is pressed down
        { //checks the object that is hit what the objects tag is. if it in this case is untagged it decreasse the speed by 5
            anim.SetBool(interactHash,true);
        }
    }
}
