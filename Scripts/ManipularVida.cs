using UnityEngine;

// Este script aplica dano ou cura ao jogador quando ele permanece dentro de uma área de colisão (trigger).
public class ManipularVida : MonoBehaviour
{
    // Referência ao script VidaJogador, que controla a vida do jogador
    VidaJogador vidaJogador;
	
    // Quantidade de vida a ser alterada (valor positivo = cura, valor negativo = dano)
    public int quantidade;

    // Intervalo de tempo (em segundos) entre cada aplicação de dano/recuperação
    public float damageTime;

    // Cronômetro que conta o tempo desde a última aplicação de dano/recuperação
    float currentDamageTime;
	
    // Chamado uma vez no início
    void Start()
    {
        // Procura o GameObject com a tag "Player" e obtém o componente VidaJogador
        vidaJogador = GameObject.FindWithTag("Player").GetComponent<VidaJogador>();
    }
	
    // Chamado a cada frame enquanto outro collider permanece dentro deste trigger
    private void OnTriggerStay(Collider other)
    {
        // Verifica se o objeto dentro do trigger é o jogador
        if (other.tag == "Player")
        {
            // Incrementa o tempo acumulado dentro da área
            currentDamageTime += Time.deltaTime;

            // Quando o tempo acumulado ultrapassa o intervalo definido...
            if (currentDamageTime > damageTime)
            {
                // Altera a vida do jogador (pode ser dano ou cura)
                vidaJogador.AlterarVida(quantidade);

                // Reinicia o cronômetro para o próximo intervalo
                currentDamageTime = 0.0f;
            }
        }
    }
}