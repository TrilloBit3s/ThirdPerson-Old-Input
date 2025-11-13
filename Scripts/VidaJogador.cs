using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Script responsável por gerenciar a vida do jogador e atualizar a interface (HUD)
public class VidaJogador : MonoBehaviour
{
    // Quantidade atual de vida do jogador
    public float vida = 100;

    // Referência à imagem da barra de vida (UI)
    public Image barraDeVida;

    // Referência ao texto que mostra a saúde (UI - TextMeshPro)
    public TMP_Text TextoSaude; 

    // Referência ao Animator do jogador (opcional, usado para desativar animações ao morrer)
    public Animator animator; 
    
    // Tempo que o sinal (+/-) será mostrado ao sofrer ou recuperar dano
    private float tempoMostraSinal = 1.5f;

    // Cronômetro para controlar o tempo de exibição do sinal
    private float cronometroSinal = 0f;

    // Armazena o sinal atual: "+" para cura, "-" para dano
    private string sinalAtual = "";
    
    // Indica se o jogador ainda está vivo
    private bool estaVivo = true;
    
    // Método chamado uma vez no início do jogo
    void Start()
    {
        // Atualiza a interface de vida assim que o jogo começa
        AtualizarInterface(0);
    }

    // Método chamado a cada frame
    void Update()
    {
        // Se o cronômetro estiver ativo, diminui seu tempo gradualmente
        if (cronometroSinal > 0)
            cronometroSinal -= Time.deltaTime;

        // Verifica se a vida chegou a 0 e o jogador ainda está marcado como vivo
        if (vida <= 0 && estaVivo)
        {
            estaVivo = false; // Marca como morto
            GameOver();       // Chama o método de fim de jogo
        }			

        // Atualiza o HUD de vida constantemente
        AtualizarInterface(0);
    }

    // Método público para alterar a vida (recebe o valor a ser somado ou subtraído)
    public void AlterarVida(float delta)
    {
        // Se o jogador já estiver morto, não faz nada
        if (!estaVivo) return;
        
        // Altera a vida e limita o valor entre 0 e 100
        vida += delta;
        vida = Mathf.Clamp(vida, 0, 100);
        
        // Define o sinal mostrado na interface conforme o tipo de alteração
        if (delta > 0)
            sinalAtual = "+";
        else if (delta < 0)
            sinalAtual = "-";
        else
            sinalAtual = "";
        
        // Reinicia o cronômetro do sinal
        cronometroSinal = tempoMostraSinal;
        
        // Atualiza a interface com o novo valor
        AtualizarInterface(delta);
        
        // Se a vida chegou a 0, desativa o Animator (opcional)
        if (vida <= 0 && animator != null)
        {
            animator.enabled = false;
        }
    }
    
    // Atualiza os elementos visuais da HUD de vida
    private void AtualizarInterface(float delta)
    {
        // Ajusta o preenchimento da barra de vida (0 a 1)
        barraDeVida.fillAmount = vida / 100f;
        
        // Se o jogador ainda estiver vivo
        if (vida > 0)
        {
            // Enquanto o cronômetro do sinal estiver ativo, mostra o sinal (+ ou -)
            if (cronometroSinal > 0)
                TextoSaude.text = $"{sinalAtual}{vida:F0}";
            else
                // Caso contrário, mostra apenas o número da vida
                TextoSaude.text = vida.ToString("F0");
        }
        else
        {
            // Exibe mensagem de morte quando a vida chega a 0
            TextoSaude.text = "- MORTO -";
        }
    }
    
    // Método chamado quando o jogador morre
    private void GameOver()
    {
        // Mostra uma mensagem no console
        Debug.Log("Game Over!");

        // Carrega a cena de Game Over
        SceneManager.LoadScene("GAMEOVER");
    }
}