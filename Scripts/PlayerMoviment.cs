// Atualizado 13/11/2025
using UnityEngine;

// Script responsável pelo controle de movimento do jogador (andar, correr, pular e interagir com trampolim)
public class PlayerMoviment : MonoBehaviour
{
    // Componentes principais
    private CharacterController personagem;
    private Animator animator;

    // Câmera que define a direção do movimento (usada para movimento relativo à câmera)
    public Camera seguirCamera;

    [Header("Movimentação")]
    public float velocidadeNormal = 5f;       // Velocidade padrão ao andar
    public float velocidadeCorrida = 8f;      // Velocidade ao correr (Shift)
    public float velocidadeRotacao = 10f;     // Suavidade da rotação

    [Header("Pulo & Gravidade")]
    public float alturaPulo = 1.0f;           // Altura do pulo
    public float gravidade = -9.81f;          // Valor da gravidade (negativo para baixo)

    // Controle interno da física
    private Vector3 velocidadeJogador;        // Armazena a velocidade vertical (e ajustes de queda)
    private bool jogadorNoChao;               // Indica se o jogador está tocando o chão

    void Start()
    {
        // Obtém referências aos componentes obrigatórios
        personagem = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Caso o jogador esqueça de atribuir a câmera no Inspector,
        // pega automaticamente a principal da cena
        if (seguirCamera == null)
            seguirCamera = Camera.main;
    }

    void Update()
    {
        // Chama a função de movimento a cada frame
        Mover();
    }

    void Mover()
    {
        // Verifica se o jogador está no chão
        jogadorNoChao = personagem.isGrounded;

        // Se estiver no chão e ainda houver força vertical negativa,
        // aplica um leve valor negativo para mantê-lo preso ao solo
        if (jogadorNoChao && velocidadeJogador.y < 0)
        {
            velocidadeJogador.y = -2f;
        }

        // Captura entrada do jogador (teclas WASD ou setas)
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        // Detecta se o jogador está pressionando Shift (correndo)
        bool correndo = Input.GetKey(KeyCode.LeftShift);

        // Define a velocidade atual conforme o estado (andando/correndo)
        float velocidadeAtual = correndo ? velocidadeCorrida : velocidadeNormal;

        // Calcula o movimento relativo à rotação da câmera
        Vector3 moveInput = Quaternion.Euler(0, seguirCamera.transform.eulerAngles.y, 0) 
                          * new Vector3(hInput, 0, vInput);

        // Normaliza a direção para evitar aumento de velocidade ao andar na diagonal
        Vector3 movementDirection = moveInput.normalized;

        // Move o personagem no plano XZ
        personagem.Move(movementDirection * velocidadeAtual * Time.deltaTime);

        // Rotaciona suavemente o personagem na direção do movimento
        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, velocidadeRotacao * Time.deltaTime);
        }

        // ---- CONTROLE DE ANIMAÇÕES ----
        animator.SetBool("Mover", movementDirection != Vector3.zero);
        animator.SetBool("EstaNoChao", jogadorNoChao);
        animator.SetBool("Correndo", correndo && movementDirection != Vector3.zero);

        // ---- PULO ----
        // Só pode pular se estiver no chão
        if (Input.GetButtonDown("Jump") && jogadorNoChao)
        {
            // Calcula a força do pulo usando a fórmula da gravidade
            velocidadeJogador.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);

            // Dispara a animação de salto
            animator.SetTrigger("Saltar");
        }

        // ---- GRAVIDADE ----
        // Aplica a gravidade continuamente
        velocidadeJogador.y += gravidade * Time.deltaTime;

        // Move o jogador verticalmente
        personagem.Move(velocidadeJogador * Time.deltaTime);
    }

    // --- Chamado pelo trampolim ---
    // Esse método é invocado quando o jogador entra em contato com o trampolim
    public void SetTrampolineForce(float force)
    {
        // Calcula a força do impulso vertical, semelhante ao pulo, mas com valor recebido do trampolim
        velocidadeJogador.y = Mathf.Sqrt(force * -2f * gravidade);
    }
}


// Atualizado 21/09/2025
// Animator precisa destes parâmetros: 
// Bool → Mover
// Bool → EstaNoChao
// Bool → Correndo
// Trigger → Saltar
/*
using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    private CharacterController personagem;
    private Animator animator;

    public Camera seguirCamera;

    [Header("Movimentação")]
    public float velocidadeNormal = 5f;
    public float velocidadeCorrida = 8f;
    public float velocidadeRotacao = 10f;

    [Header("Pulo & Gravidade")]
    public float alturaPulo = 1.0f;
    public float gravidade = -9.81f;

    private Vector3 velocidadeJogador;
    private bool jogadorNoChao;

    void Start()
    {
        personagem = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Se não arrastar a câmera no Inspector, pega a principal
        if (seguirCamera == null)
            seguirCamera = Camera.main;
    }

    void Update()
    {
        Mover();
    }

    void Mover()
    {
        // Checa se está no chão
        jogadorNoChao = personagem.isGrounded;
        if (jogadorNoChao && velocidadeJogador.y < 0)
        {
            velocidadeJogador.y = -2f; // mantém no chão
        }

        // Entrada de movimento
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        // Define velocidade (anda ou corre)
        bool correndo = Input.GetKey(KeyCode.LeftShift);
        float velocidadeAtual = correndo ? velocidadeCorrida : velocidadeNormal;

        // Movimento relativo à câmera
        Vector3 moveInput = Quaternion.Euler(0, seguirCamera.transform.eulerAngles.y, 0) * new Vector3(hInput, 0, vInput);
        Vector3 movementDirection = moveInput.normalized;

        // Move personagem
        personagem.Move(movementDirection * velocidadeAtual * Time.deltaTime);

        // Rotação suave
        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, velocidadeRotacao * Time.deltaTime);
        }

        // ---- ANIMAÇÕES ----
        animator.SetBool("Mover", movementDirection != Vector3.zero);
        animator.SetBool("EstaNoChao", jogadorNoChao);
        animator.SetBool("Correndo", correndo && movementDirection != Vector3.zero);

        // Pulo
        if (Input.GetButtonDown("Jump") && jogadorNoChao)
        {
            velocidadeJogador.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            animator.SetTrigger("Saltar");
        }

        // Gravidade
        velocidadeJogador.y += gravidade * Time.deltaTime;
        personagem.Move(velocidadeJogador * Time.deltaTime);
    }
}*/