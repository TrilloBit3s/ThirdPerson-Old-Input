using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    //Componentes principais
    private CharacterController personagem;
    private Animator animator;

    //Câmera que define a direção do moviemento
    public Camera seguirCamera;

    [Header("Movimentação")]
    public float velocidadeNormal = 5f; //velocidade padrao de andar
    public float velocidadeCorrida = 8f; //velocidade de corrida
    public float velocidadeRotacao = 15f; //velocidade de rotação do personagem

    // Controle interno da física
    private Vector3 velocidadeJogador; //armazena a velocidade atual do jogador
    private bool jogadorNoChao; //verifica se o jogador está no chão

    [Header("Pulo e Gravidade")]
    public float alturadoPulo = 1.0f; //altura do pulo
    private float gravidade = -9.81f; //força da gravidade

    void Start()
    {
        // obtem os componentes necessários
        personagem = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //caso o jogador não tenha uma câmera atribuída, atribui a câmera principal
        if(seguirCamera == null)
        {
            seguirCamera = Camera.main;
        }
    }

    void Update()
    {
        Mover();
    }

    void Mover()
    {
        // verificar se o jogador está no chão
        jogadorNoChao = personagem.isGrounded;

        // se estiver no chao e ainda houver força vertical negativa
        if(jogadorNoChao && velocidadeJogador.y < 0)
        {
            velocidadeJogador.y = -2f; //pequena força para manter o jogador no chão  
        }

        // captura de entrada do jogador (teclas WASD ou setas)
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        // Detectar se o jogador esta pressionando a tecla de corrida (Shift esquerdo)
        bool correndo = Input.GetKey(KeyCode.LeftShift);

        // Define a veloccidade atual conforme o estado (andar ou correr)
        float velocidadeAtual = correndo ? velocidadeCorrida : velocidadeNormal; 

        // Calcula a direção do movimento com base na câmera
        Vector3 moveInput = Quaternion.Euler(0, seguirCamera.transform.eulerAngles.y, 0) * new Vector3(hInput, 0, vInput);

        // Normaliza a direção para evitar aumento de velocidade ao andar na diagonal
        Vector3 movementDirection = moveInput.normalized;

        // Move o personagem no plano X e Z
        personagem.Move(movementDirection * velocidadeAtual * Time.deltaTime);

        // Rotacionar suavemente o personagem na direção do movimento
        if(movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, velocidadeRotacao * Time.deltaTime);
        }

        //---CONTROLE DE ANIMAÇÕES---
        bool estaMovendo = movementDirection.magnitude > 0.1f;

        //Ativa "Mover" quando ha movimento
        animator.SetBool("Mover", estaMovendo);

        //Informar se esta no chão
        animator.SetBool("EstaNoChao", jogadorNoChao);

        //Ativa correndo somente se estiver se movendo e segurando o shift
        animator.SetBool("Correndo", estaMovendo && correndo);

        //Pulo
        //Só pula se o jogador estiver no chao
        if(Input.GetButtonDown("Jump") && jogadorNoChao)
        {
            //calcula a força do pulo usando a gravidade
            velocidadeJogador.y = Mathf.Sqrt(alturadoPulo * -2f * gravidade);

            //Dispara animação de salto
            animator.SetTrigger("Saltar");
        }

        //Gravidade
        velocidadeJogador.y += gravidade * Time.deltaTime;

        //mover o jogadror verticalmente
        personagem.Move(velocidadeJogador * Time.deltaTime);
    }

    public void SetTrampolimForce(float force)
    {
        //aplica a força do trampolim na velocidade vertical do jogador, mas com valor recebido do trampolim
        velocidadeJogador.y = Mathf.Sqrt(force * -2f * gravidade);

        if(animator != null)
        {
            animator.SetTrigger("Saltar");
        }
    }
}