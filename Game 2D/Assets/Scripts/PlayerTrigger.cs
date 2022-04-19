using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    // Variável para importar os componentes do Script do Jogador
    private PlayerController playerScript;

    // Start is called before the first frame update
    void Start()
    {
        // Pega o componente do Script baseado no nome do objeto (no caso o jogador)
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Função para verificar as colisões com objetos do tipo IsTrigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se ele colidir com um objeto com a tag Enemy ele vai chamar a função de dano ao jogador
        if (collision.CompareTag("Enemy"))
        {
            playerScript.DamagePlayer();
        }

        // Se ele colidir com um objeto com a tag Water ele vai chamar a função de dano de água ao jogador
        if (collision.CompareTag("Water"))
        {
            playerScript.DamageWater();
        }
    }
}
