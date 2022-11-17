using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animatorEnemy;
    float limizq;
    float limder;
    public float velEnemy;
    int direccion = 1;
    Vector3 escalaOriginal;
    public int lifeEnemy;
    bool setDeath;
    enum tipoComportamiento{pasivo, persecusion, ataque}
    tipoComportamiento comportamiento = tipoComportamiento.pasivo;
    public float limiteIzquierdo;
    public float limiteDerecho;
    public float entradaPersecusion = 10f;
    public float salidaPersecusion = 20f;
    public float distanciaAtaque = 5f;
    float distanciaDelCharacter;
    public Transform Character;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorEnemy = GetComponent<Animator>();
        limizq = transform.position.x - limiteDerecho;
        limder = transform.position.x + limiteIzquierdo;
        escalaOriginal = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        distanciaDelCharacter = Mathf.Abs(Character.position.x - transform.position.x); 

        switch(comportamiento)
        {
            case tipoComportamiento.pasivo:
                //Caminar
                rb.velocity = new Vector2(velEnemy * direccion, rb.velocity.y);

                //Girar
                if(transform.position.x < limizq) direccion = 1;
                if(transform.position.x > limder) direccion = -1;

                //velocidad del animator
                animatorEnemy.speed = 1f;

                //entra zona de persecusion
                if(distanciaDelCharacter < entradaPersecusion) comportamiento = tipoComportamiento.persecusion;

                break;
            
            case tipoComportamiento.persecusion:
                //Correr
                rb.velocity = new Vector2(velEnemy * 1.5f * direccion, rb.velocity.y);
                //Girar al jugador
                if(Character.position.x > transform.position.x) direccion = 1;
                if(Character.position.x < transform.position.x) direccion = -1;
                
                //velocidad del animator
                animatorEnemy.speed = 1.5f;

                //entra zona pasiva
                if(distanciaDelCharacter > salidaPersecusion) comportamiento = tipoComportamiento.pasivo;
                
                //entra zona ataque
                if(distanciaDelCharacter < distanciaAtaque) comportamiento = tipoComportamiento.ataque;
                break;

            case tipoComportamiento.ataque:
                animatorEnemy.SetTrigger("attack");
                
                //Girar al jugador
                if(Character.position.x > transform.position.x) direccion = 1;
                if(Character.position.x < transform.position.x) direccion = -1;

                //vuelve zona de persecusion
                if((distanciaDelCharacter > distanciaAtaque) && !animatorEnemy.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttack"))
                {
                    comportamiento = tipoComportamiento.persecusion;
                    animatorEnemy.ResetTrigger("attack");
                } 
                else if(animatorEnemy.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttack")){
                    rb.velocity = new Vector2(0, 0);
                }
                break;
        }
        transform.localScale = new Vector3(escalaOriginal.x * direccion, escalaOriginal.y, escalaOriginal.z);
        checkLife();
        SetEnemyDeath();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<CharacterMovement>().lifeCharacter -= 1;
        }
    }
    
    //Death
    void SetEnemyDeath(){
        animatorEnemy.SetBool("death", setDeath);
    }
    
    void checkLife(){
        if(lifeEnemy <= 0)
        {
            setDeath = true;
            this.enabled = false;
        }
    }


}
