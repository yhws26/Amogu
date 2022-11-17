using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    Animator animator;
    BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            animator.SetBool("door",true);
        } 
    }

    void OnTriggerExit2D(Collider2D other){
        animator.SetBool("door",false);

    }
    void boxcolliderenabled(){
        boxCollider.enabled = true;
    }
    void boxcolliderdisable(){
        boxCollider.enabled = false;
    }
}
