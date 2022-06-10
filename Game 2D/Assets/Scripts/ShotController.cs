using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    // Vari�veis para controle do Tiro / Ataque
    public float speed = 20f;
    public Rigidbody2D rigidbody2;
    public int damage;
    public GameObject impactEffect;
    private GameObject impact;
    
    // Start is called before the first frame update
    void Start()
    {
        // Aplica velocidade para o Tiro / Ataque
        rigidbody2.velocity = transform.right * speed;
    }

    // Fun��o para verificar as colis�es do Tiro / Ataque com objetos do tipo IsTrigger
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Criando uma vari�vel com os componentes do inimigo Polvo
        OctopusController octopus = hitInfo.GetComponent<OctopusController>();
        // Se o tiro colidir com o Octopus, ele chama a fun��o TakeDamage() passando o valor do dano do Ataque
        if (octopus != null)
        {
            octopus.TakeDamage(damage);
        }

        // Criando uma vari�vel com os componentes do inimigo Crab
        CrabController crab = hitInfo.GetComponent<CrabController>();
        // Se o tiro colidir com o Crab, ele chama a fun��o TakeDamage() passando o valor do dano do Ataque
        if (crab != null)
        {
            crab.TakeDamage(damage);
        }

        // Informa um Log com o nome do objeto atingido e se destroi
        Debug.Log(hitInfo.name);
        impact = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(impact, 0.2f);
        Destroy(gameObject);
    }
}
