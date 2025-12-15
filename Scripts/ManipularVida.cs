using UnityEngine;

public class ManipularVida : MonoBehaviour
{
    //Referencia de vida  a ser alterada
    VidaJogador vidaJogador;

    //Quantidade de vida a ser alterada
    public int quantidade;

    //Intervalo de tempo entre cada aplicação de dano
    public float damageTime;

    //Cronometro que conta o tempo desde a ultima aplicação de dano
    float currentDamageTime;

    void Start()
    {
        vidaJogador = GameObject.FindGameObjectWithTag("Player").GetComponent<VidaJogador>();
    }

    private void OnTriggerStay(Collider coll)
    {
        //verifica se o objeto dentro do trigger é o jogador
        if(coll.tag == "Player")
        {
            //Incrementa o tempo acumulado dentro da area
            currentDamageTime += Time.deltaTime;

            //Quando o tempo ultrapassa o tempo definido
            if(currentDamageTime > damageTime)
            {
                //Altera a vida do jogador( pode ser dano ou cura)
                vidaJogador.AlterarVida(quantidade);

                //Reinicia o cronometro para o proximo intervalo    
                currentDamageTime = 0.0f;
            }
        }
    }
}
