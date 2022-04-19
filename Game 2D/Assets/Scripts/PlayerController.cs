using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour   
{
    // Vari�veis basicas do Jogador
    public float speed;
    public int jumpForce;
    public int health;
    public Transform groundCheck;

    // Vari�veis para controle de estados do Jogador
    private bool invunerable = false;
    private bool grounded = false;
    private bool jumping = false;
    private bool facingRight = true;

    // Vari�veis para os componentes do Jogador
    private SpriteRenderer sprite;
    private Rigidbody2D rigidbody2D;
    private Animator animation;
    private Transform transf;

    // Vari�veis para controle do Tiro / Ataque do Jogador
    public float fireRate;
    public Transform spawnShot;
    public GameObject shotPrefab;
    private float nextShot = 0f;

    // Start is called before the first frame update
    void Start()    
    {
        // Atribuindo os componentes para cada vari�vel
        sprite = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        transf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()    
    {
        // Cria uma linha imagin�ria da posi��o do jogador at� o groundCheck para validar se estamos tocando a camada do ch�o
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

        // Se o bot�o de pulo for pressionado e o Jogador estiver tocando o ch�o, o estado de pulo fica positivo
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
        // V�riavel para receber os eixos horizontais quando a tecla for pressionada e aplicando velocidade para o movimento
        float move = Input.GetAxis("Horizontal");
        rigidbody2D.velocity = new Vector2(move * speed, rigidbody2D.velocity.y);

        // Chamando a fun��o Flip() para virar o Jogador para o lado que ele est� caminhando
        if ((move < 0f && facingRight) || (move > 0f && !facingRight))
        {
            Flip();
        }

        // Se o estado de Jumping estiver validada ele adiciona for�a no pulo para o jogador saltar
        if(jumping)
        {
            // Adiciona for�a ao pulo
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            jumping = false;
        }
    }

    // Fun��o para virar o jogador para o lado oposto
    void Flip()    
    {
        facingRight = !facingRight;
        transf.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

    // Fun��o para chamar as anima��es do Jogador
    void SetAnimations()   
    {
        animation.SetFloat("VelY", rigidbody2D.velocity.y);
        animation.SetBool("Player_Jump", !grounded);
        animation.SetBool("Player_Run", rigidbody2D.velocity.x != 0f && grounded);

    }

    // Fun��o de Tiro
    void Shot()  
    {
        // Chama a anima��o de atirar e coloca um tempo para o pr�ximo tiro
        animation.SetTrigger("Player_Shot");
        nextShot = Time.time + fireRate;

        // Cria v�rios clones do mesmo objeto na tela com a fun��o Instantiate
        GameObject cloneShot = Instantiate(shotPrefab, spawnShot.position, spawnShot.rotation);

        // Caso o personagem estiver virado para esquerda ele rotaciona os tiros em 180�
        if(!facingRight)
        {
            cloneShot.transform.eulerAngles = new Vector3(180, 0, 180);
        }
    }

    // Fun��o do Efeito do dano usando IEnumerator
    IEnumerator DamageEffect()
    {
        // Aplica um la�o de repeti��o para controlar o efeito
        for (float i = 0f; i < 1f; i += 0.1f)
        {
            // Desabilita e habilita a sprite do jogador pelos tempos de 0.1s cada, durante o la�o de repeti��o
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        // Desabilita a invulnerabilidade novamente
        invunerable = false;

    }

    // Fun��o para aplicar dano ao jogador
    public void DamagePlayer()
    {
        // Se a invulnerabilidade for falsa, ele executa os comandos abaixo
        if (!invunerable)
        {
            invunerable = true;
            health--;
            StartCoroutine(DamageEffect());

            // Se a vida do jogador chegar a zero ele emite um log e reinicia a fase com o m�todo Invoke
            if (health <= 0)
            {
                Debug.Log("Morreu");
                Invoke("ReloadFase", 1f);
                gameObject.SetActive(false);
            }
        }
    }

    // Fun��o para matar o jogador ao entrar em contato com a �gua
    public void DamageWater()
    {
        Debug.Log("Morreu");
        health = 0;
        Invoke("ReloadFase", 1f);
        gameObject.SetActive(false);

    }

    // Fun��o para reiniciar a fase atual
    void ReloadFase()
    {
        // Chama uma fun��o do gerenciador de cenas para carregar a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}