using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Inicia o jogo carregando a cena principal
    public void InicarJogo()
    {
        // Carrega a cena chamada "Cena Principal"
        SceneManager.LoadScene("Cena Principal");
    }

    // Sai do jogo
    public void SairJogo()
    {
        //Debug.Log("Saindo do jogo");
        
        // Encerra a aplicação
        Application.Quit();
    }
}