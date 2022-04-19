using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour   
{
    // Variáveis basicas do Jogador
    public float speed;
    public int jumpForce;
    public int health;
    public Transform groundCheck;

    // Variáveis para controle de estados do Jogador
    private bool invunerable = false;
    private bool grounded = false;
    private bool jumping = false;
    private bool facingRight = true;

    // Variáveis para os componentes do Jogador
    private SpriteRenderer sprite;
    private Rigidbody2D rigidbody2D;
    private Animator animation;
    private Transform transf;

    // Variáveis para controle do Tiro / Ataque do Jogador
    public float fireRate;
    public Transform spawnShot;
    public GameObject shotPrefab;
    private float nextShot = 0f;

    // Start is called before the first frame update
    void Start()    
    {
        // Atribuindo os componentes para cada variável
        sprite = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        transf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()    
    {
        // Cria uma linha imaginária da posição do jogador até o groundCheck para validar se estamos tocando a camada do chão
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

        // Se o botão de pulo for pressionado e o Jogador estiver tocando o chão, o estado de pulo fica positivo
        if(Input.GetButtonDown("Jump") && grounded)
        {
            jumping = true;
        }

        SetAnimations();

        if(Input.GetButton("Fire1") && grounded && Time.time > nextShot)
        {
            Shot();
        }
    }


    void FixedUpdate()    
    {
        // Váriavel para receber os eixos horizontais quando a tecla for pressionada e aplicando velocidade para o movimento
        float move = Input.GetAxis("Horizontal");
        rigidbody2D.velocity = new Vector2(move * speed, rigidbody2D.velocity.y);

        // Chamando a função Flip() para virar o Jogador para o lado que ele está caminhando
        if ((move < 0f && facingRight) || (move > 0f && !facingRight))
        {
            Flip();
        }

        // Se o estado de Jumping estiver validada ele adiciona força no pulo para o jogador saltar
        if(jumping)
        {
            // Adiciona força ao pulo
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            jumping = false;
        }
    }

    // Função para virar o jogador para o lado oposto
    void Flip()    
    {
        facingRight = !facingRight;
        transf.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

    // Função para chamar as animações do Jogador
    void SetAnimations()   
    {
        animation.SetFloat("VelY", rigidbody2D.velocity.y);
        animation.SetBool("Player_Jump", !grounded);
        animation.SetBool("Player_Run", rigidbody2D.velocity.x != 0f && grounded);

    }

    // Função de Tiro
    void Shot()  
    {
        // Chama a animação de atirar e coloca um tempo para o próximo tiro
        animation.SetTrigger("Player_Shot");
        nextShot = Time.time + fireRate;

        // Cria vários clones do mesmo objeto na tela com a função Instantiate
        GameObject cloneShot = Instantiate(shotPrefab, spawnShot.position, spawnShot.rotation);

        // Caso o personagem estiver virado para esquerda ele rotaciona os tiros em 180º
        if(!facingRight)
        {
            cloneShot.transform.eulerAngles = new Vector3(180, 0, 180);
        }
    }

    // Função do Efeito do dano usando IEnumerator
    IEnumerator DamageEffect()
    {
        // Aplica um laço de repetição para controlar o efeito
        for (float i = 0f; i < 1f; i += 0.1f)
        {
            // Desabilita e habilita a sprite do jogador pelos tempos de 0.1s cada, durante o laço de repetição
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        // Desabilita a invulnerabilidade novamente
        invunerable = false;

    }

    // Função para aplicar dano ao jogador
    public void DamagePlayer()
    {
        // Se a invulnerabilidade for falsa, ele executa os comandos abaixo
        if (!invunerable)
        {
            invunerable = true;
            health--;
            StartCoroutine(DamageEffect());

            // Se a vida do jogador chegar a zero ele emite um log e reinicia a fase com o método Invoke
            if (health <= 0)
            {
                Debug.Log("Morreu");
                Invoke("ReloadFase", 1f);
                gameObject.SetActive(false);
            }
        }
    }

    // Função para matar o jogador ao entrar em contato com a água
    public void DamageWater()
    {
        Debug.Log("Morreu");
        health = 0;
        Invoke("ReloadFase", 1f);
        gameObject.SetActive(false);

    }

    // Função para reiniciar a fase atual
    void ReloadFase()
    {
        // Chama uma função do gerenciador de cenas para carregar a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}