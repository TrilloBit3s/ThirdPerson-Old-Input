using UnityEngine;

// Script responsável por aplicar uma força vertical (impulso) ao jogador
// quando ele entra em contato com o trampolim.
public class Trampoline : MonoBehaviour
{
    // Força do impulso do trampolim — pode ser ajustada no Inspector
    [SerializeField] private float trampolineForce = 10f;

    // Detecta quando um objeto entra na área de colisão do trampolim
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que colidiu possui o componente "PlayerMoviment"
        // (isso garante que o trampolim só afete o jogador e não outros objetos)
        PlayerMoviment player = other.GetComponent<PlayerMoviment>();

        // Se o componente foi encontrado...
        if (player != null)
        {
            // Aplica a força vertical de impulso ao jogador
            // (a função SetTrampolineForce deve estar definida no script PlayerMoviment)
            player.SetTrampolineForce(trampolineForce);
        }
    }
}