using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    BoxCollider2D boxCollider;
    public float velocidad;
    bool mirandoDerecha = true;
    public float fuerzaSalto;  
    public LayerMask capaSuelo;
    public int lifeCharacter;
    bool setDeath;
    bool tieneArma = false;
    public AudioClip ShootSound; 
    public AudioClip PunchSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcesarMovimiento();
        Jump();
        Attack();
        Shoot();
        checkLife();
        SetCharacterDeath();
    }

    //Walk
    void ProcesarMovimiento()
    {
        float inputMovimiento = Input.GetAxis("Horizontal");

        if(inputMovimiento != 0){
            animator.SetBool("isWalking", true);
            //Debug.Log("The Character Walks");
        }
        else{
            animator.SetBool("isWalking", false);
        }

        if(!this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Punch"))
        {
            rb.velocity = new Vector2(inputMovimiento * velocidad, rb.velocity.y);
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Punch"))
        {
            rb.velocity = new Vector2(0,rb.velocity.y);
        }

        GestionarOrientacion(inputMovimiento);
    }

    void GestionarOrientacion(float inputMovimiento)
    {
        if((mirandoDerecha == true && inputMovimiento < 0) || (mirandoDerecha == false && inputMovimiento > 0)){
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
    
    //Jump
    void Jump() 
    {
        if(Input.GetKeyDown(KeyCode.Space) && EstaEnSuelo()) 
        {
            rb.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
            //Debug.Log("The Character Jumps");
        }
    }

    bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, capaSuelo);
        return raycastHit.collider != null;
    }

    //Attack_Punch
    void Attack() 
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && EstaEnSuelo()) 
        {
            animator.SetBool("punch", true);
            AudioSource.PlayClipAtPoint(PunchSound, transform.position);
            //Debug.Log("The Character attacks");
        } 
        else
        {
            animator.ResetTrigger("punch");
        }
    }

    //Attack_Pistol
    void Shoot()
    {
        if(tieneArma){
            if(Input.GetKeyDown(KeyCode.Mouse1) && EstaEnSuelo()) 
            {
                animator.SetBool("shoot", true);
                AudioSource.PlayClipAtPoint(ShootSound, transform.position);
                //Debug.Log("The Character shoots");
            } 
            else
            {
                animator.ResetTrigger("shoot");
            }
        }
    }

    void ShootImpulso()
    {
        if(mirandoDerecha){
            rb.AddForce(new Vector2(-2000f * 1,0),ForceMode2D.Impulse);
        } else{
            rb.AddForce(new Vector2(-2000f * -1,0),ForceMode2D.Impulse);
        }
    }
    
    //Death
    void SetCharacterDeath(){
        animator.SetBool("death", setDeath);
    }
    
    void checkLife(){
        if(lifeCharacter <= 0){
            setDeath = true;
            this.enabled = false;
            //Debug.Log("The Character died!");
            SceneManager.LoadScene("Scenes/SampleScene");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //HitBox
        if((other.CompareTag("Enemy")) && (tieneArma))
        {
            other.GetComponent<Enemys>().lifeEnemy -= 3;
        } 
        else if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemys>().lifeEnemy -= 1; 
        }
        //Pistol
        if(other.gameObject.CompareTag("Weapon"))
        {
            tieneArma = true;
            Destroy(other.gameObject);
        }
    }
}
