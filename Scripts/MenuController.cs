using UnityEngine;
using UnityEngine.SceneManagement;

// Script responsável por controlar as ações do menu principal (botões de iniciar e sair)
public class MenuController : MonoBehaviour
{
    // Método público chamado ao clicar no botão "Iniciar Jogo"
    public void IniciarJogo()
    {
        // Carrega a cena principal do jogo (certifique-se de que "CENA1" está adicionada na Build Settings)
        SceneManager.LoadScene("CENA1");
    }
	
    // Método público chamado ao clicar no botão "Sair do Jogo"
    public void SairDoJogo()
    {
        // Exibe uma mensagem no console (visível apenas no Editor)
        Debug.Log("Saindo do jogo...");

        // Encerra a aplicação (só funciona em build — no Editor do Unity, não fecha)
        Application.Quit();
    }
}