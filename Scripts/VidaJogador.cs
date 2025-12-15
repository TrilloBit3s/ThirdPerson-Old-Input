using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VidaJogador : MonoBehaviour
{
    //Quantidade de vida do jogador
    public float vida = 100;

    //Referencia a imagem da barra de vida UI
    public Image barraDeVida;

    //Referencia ao texto que mostra a saúde do jogador UI
    public TMP_Text TextoSaude;

    //Referencia ao Animator do jogador 
    public Animator animator;

    //Tempo que o sinal (+/-) sera mostrado ai sofrer ou recuperar vida
    private float tempoMostrarSinal = 1.5f;

    //Cronometro para contar o tempo que exibe o sinal
    private float cronometroSinal = 0f;

    // Armazena o sinal Atual: "+" para cura, "-" para dano 
    private string sinalAtual = "";

    //Indica se o jogador ainda esta vivo
    private bool estaVivo = true;

    void Start()
    {
        //Atualiza a iterface de vida assim que o jogo começa
        AtualizarInterface(0);
    }

    void Update()
    {
        //Se o cronometro estiver ativo, diminui seu tempo rgadualmente
        if(cronometroSinal > 0)
           cronometroSinal -= Time.deltaTime; 
        
        //verifica se a vida do jogador chegou a 0 e se o jogador ainda esta vivo
        if(vida <= 0 && estaVivo)
        {
            estaVivo = false; //marca como morto
            GameOver(); //chama Game Over 
        }

        AtualizarInterface(0);
    }

    public void AlterarVida(float delta)
    {
        //se o jogador estiver Morto nao faz nada
        if(!estaVivo) return;

        vida += delta;
        vida = Mathf.Clamp(vida, 0, 100);
        
        //Define o sinal mostrado na interface conforme o tipo de alteração
        if(delta > 0)
            sinalAtual = "+";
        else if(delta < 0)
            sinalAtual = "-";
        else
            sinalAtual = "";    
        
        //Reinicia o cronometro do sinal
        cronometroSinal = tempoMostrarSinal;

       AtualizarInterface(delta);

       //Se a vida chegou a zero, desativa o animator
       if(vida <=0 && animator != null)
        {
            animator.enabled = false;
        }
    }

    //Atualiza os elementos visuais da HUD "UI" de vida
    private void AtualizarInterface(float delta)
    {
        //Ajusta o preenchimento da barra de vida (0 a 1)
        barraDeVida.fillAmount = vida / 100f;

        //se o jogador ainda esta vivo
        if(vida > 0)
        {
           //Enquanto o cronometro do sinal estivo ativo, mostra o sinal + ou -
           if(cronometroSinal > 0)
                TextoSaude.text = $"{sinalAtual}{vida:F0}";
            else
                //caso o contrário, mostra apenas o numero da vida
                TextoSaude.text = vida.ToString("F0");      
        }
        else
        {
            //Exibir mensagem de morte quando a vida chegar a zero
            TextoSaude.text = "0"; 
        }

    }

    private void GameOver()
    {   
        //Mostrar mensagens no Console
       // Debug.Log("GameOver!");

        //chama o Game Over
        SceneManager.LoadScene("GameOver");        
    }
}