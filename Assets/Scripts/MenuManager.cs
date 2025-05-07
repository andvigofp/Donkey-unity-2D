using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuCreditos; // Referencia al Canvas de Créditos
    public GameObject menuPrincipal; // Referencia al Canvas del Menú Principal

    public void GoToCreditosMenu()
    {
        menuCreditos.SetActive(true); // Mostrar créditos
        menuPrincipal.SetActive(false); // Ocultar menú principal
    }

    public void BackToMainMenu()
    {
        menuCreditos.SetActive(false); // Ocultar créditos
        menuPrincipal.SetActive(true); // Mostrar menú principal
    }
}
