/*
Player (seu personagem com CharacterController e Animator)
ManipuladorDeCamera (Empty com o script ManipuladorDeCamera)
└── Pivot (Empty)
    └── Main Camera (a câmera real)
*/
using UnityEngine;

public class ManipuladorDeCamera : MonoBehaviour
{
    public Transform jogador;
    private Transform pivot;

    [Header("Configurações de movimento")]
    public float lerpSpeed = 5f;
    public float turnSpeed = 1.5f;
    public float turnSmoothing = 0.1f;

    [Header("Limites de rotação")]
    public float tiltMax = 75f;
    public float tiltMin = -30f;   // negativo para olhar para baixo
    public float minCameraHeight = 0.5f;

    private float smoothX;
    private float smoothY;
    private float lookAngle; // rotação horizontal
    private float tiltAngle; // rotação vertical
    private float velX;
    private float velY;

    void Start()
    {
        pivot = transform.GetChild(0);

        if (jogador == null)
        {
            Debug.LogWarning("Jogador não atribuído na câmera! Arraste o Player no Inspector.");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }
    
   
    void LateUpdate()
    {
        if (jogador == null) return;

        // ---- Segue a posição do jogador ----
        transform.position = jogador.position;

        // ---- Captura inputs do mouse ----
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        // Suavização dos inputs
        smoothX = Mathf.SmoothDamp(smoothX, x, ref velX, turnSmoothing);
        smoothY = Mathf.SmoothDamp(smoothY, y, ref velY, turnSmoothing);

        // ---- Rotação horizontal (em torno do jogador) ----
        lookAngle += smoothX * turnSpeed;
        Quaternion rotacaoHorizontal = Quaternion.Euler(0f, lookAngle, 0f);
        transform.rotation = rotacaoHorizontal;

        // ---- Rotação vertical (tilt do pivot) ----
        tiltAngle -= smoothY * turnSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, tiltMin, tiltMax);
        pivot.localRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
    }
}