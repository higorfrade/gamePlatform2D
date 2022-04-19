using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusController : MonoBehaviour
{
    // Vari�veis basicas do inimigo
    public float speed;
    public int health;
    public Transform groundCheck;
    public Transform skyCheck;

    // Vari�veis para controles de estados do inimigo
    private bool grounded = false;
    private bool touchSky = false;

    // Vari�veis para os componentes do inimigo
    private SpriteRenderer sprite;
    private Rigidbody2D rigidbody2D;


    // Start is called before the first frame update
    void Start()
    {
        // Atribuindo os componentes para cada vari�vel
        rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        // Aplicando velocidade na posi��o do eixo Y
        rigidbody2D.velocity = transform.position * speed;
    }

    // Update is called once per frame
    void Update()
    {
        // Cria uma linha imagin�ria da posi��o do Octopus at� o groundCheck e o skyCheck para validar se estamos tocando a camada do ch�o e do c�u
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        touchSky = Physics2D.Linecast(transform.position, skyCheck.position, 1 << LayerMask.NameToLayer("Sky"));

        
    }

    void FixedUpdate()
    {
        // Se ele detectar colis�o com o ch�o ele aplica velocidade ao eixo Y para o Octopus subir
        if (grounded)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, speed);
        }

        // Se ele detectar colis�o com o c�u ele aplica velocidade negativa ao eixo Y para o Octopus descer
        if (touchSky)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -speed);
        }
    }

    // Fun��o para verificar as colis�es do Octopus com objetos do tipo IsTrigger
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

    // Fun��o para aplicar dano ao Octopus
    public void TakeDamage(int damage)
    {
        // Diminui a vida baseado no dano do Tiro do jogador e aplica a fun��o de efeito de dano
        health -= damage;
        StartCoroutine(DamageEffect());

        // Se a vida do Octopus chegar a 0 ele morre
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
