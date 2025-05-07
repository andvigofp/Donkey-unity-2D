using UnityEngine;

public class MenuControles : MonoBehaviour
{
    public GameObject menuPrincipal;  // Referencia al Canvas del Menú Principal
    public GameObject menuControles;  // Referencia al Canvas del Menú Controles

    private void Update()
    {
        // Si estamos en el menú de controles y se presiona Escape, volver al menú principal
        if (menuControles.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }
    }

    public void GoToControlesMenu()
    {
        menuPrincipal.SetActive(false); // Ocultar menú principal
        menuControles.SetActive(true);  // Mostrar menú de controles
    }

    public void BackToMainMenu()
    {
        menuControles.SetActive(false); // Ocultar menú de controles
        menuPrincipal.SetActive(true);  // Mostrar menú principal
    }
}
