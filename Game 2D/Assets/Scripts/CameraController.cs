using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Componentes da C�mera
    private Vector2 velocity;
    private Transform player;

    // Vari�veis para controle de delay da C�mera
    public float smoothTimeX;
    public float smoothTimeY;


    // Start is called before the first frame update
    void Start()
    {
        // Pega o componente transform baseado no nome do objeto (no caso o jogador)
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        // Se existir um player, a posi��o da c�mera ser� baseado na posi��o do player
        if (player != null) {
            float posX = Mathf.SmoothDamp(transform.position.x, player.position.x, ref velocity.x, smoothTimeX);
            float posY = Mathf.SmoothDamp(transform.position.y, player.position.y, ref velocity.y, smoothTimeY);

            transform.position = new Vector3(posX, posY, transform.position.z);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
