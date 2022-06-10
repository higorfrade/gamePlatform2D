using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    // Vari�veis basicas do inimigo
    public float speed;
    public int health;
    public Transform groundCheck;
    public Transform wallCheck;
    public GameObject deathEffect;
    private GameObject death;

    // Vari�veis para controles de estados do inimigo
    private bool grounded = false;
    private bool touchWall = false;
    private bool facingRight = false;

    // Vari�veis para os componentes do inimigo
    private SpriteRenderer sprite;
    private Rigidbody2D rigidbody2D;


    // Start is called before the first frame update
    void Start()
    {
        // Atribuindo os componentes para cada vari�vel
        rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        // Aplicando velocidade ao RigidBody do Crab
        rigidbody2D.velocity = transform.position * speed;
    }

    // Update is called once per frame
    void Update()
    {
        // Cria uma linha imagin�ria da posi��o do Octopus at� o groundCheck e o wallCheck para validar se estamos tocando a camada do ch�o e da parede
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        touchWall = Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        // Se ele detectar uma parede ele executa a fun��o Flip() para virar o Crab para o lado contr�rio
        if (touchWall)
        {
            Flip();
        }

    }

    // Fun��o para virar o Crab para o lado oposto
    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        speed *= -1;
    }

    void FixedUpdate()
    {
        // Se ele estiver tocando o ch�o ele aplica velocidade ao movimento no eixo X
        if (grounded)
        {
            rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
        }

    }

    // Fun��o para verificar as colis�es do Crab com objetos do tipo IsTrigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se ele colidir com um objeto com a tag Player ele vai emitir um Log informando a colis�o
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Colidiu com o Player");
        }

        // Se ele colidir com um objeto com a tag Shot ele vai emitir um Log informando que sofreu dano
        if (collision.CompareTag("Shot"))
        {
            Debug.Log("Sofreu dano");
        }
    }

    // Fun��o do Efeito do dano usando IEnumerator
    IEnumerator DamageEffect()
    {
        // Acessa a op��o de Cor do sprite renderer e altera ela para vermelho por 0.1s e depois volta a cor do sprite ao normal
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    // Fun��o para aplicar dano ao Crab
    public void TakeDamage(int damage)
    {
        // Diminui a vida baseado no dano do Tiro do jogador e aplica a fun��o de efeito de dano
        health -= damage;
        StartCoroutine(DamageEffect());

        // Se a vida do Crab chegar a 0 ele morre
        if (health <= 0)
        {
            death = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(death, 0.25f);
            Destroy(gameObject);
        }
    }
}
