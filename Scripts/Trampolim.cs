using UnityEngine;

public class Trampolim : MonoBehaviour
{
    //força aplicada pelo trampolim
    [SerializeField] private float trampolimForce = 4f;

    //detecta colisões com o jogador
    private void OnTriggerEnter(Collider other)
    {
        PlayerMoviment player = other.GetComponent<PlayerMoviment>();

        //se o component foi encontrado
        if(player != null)
        {
            //aplica a força vertical
            player.SetTrampolimForce(trampolimForce);
        }
    }
}