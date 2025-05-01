using UnityEngine;
using TMPro;
using System.Collections;

public class MessageController : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Asignar el texto en el Inspector
    private float fadeDuration = 1f; // Duración de la aparición y desaparición

    private void Start()
    {
        StartCoroutine(ShowMessage());
    }

    private IEnumerator ShowMessage()
    {
        messageText.text = "Ayuda Mario"; // Mostrar el mensaje

        // Aparecer suavemente
        yield return StartCoroutine(FadeText(0, 1));

        // Esperar unos segundos antes de desaparecer
        yield return new WaitForSeconds(2);

        // Desaparecer suavemente
        yield return StartCoroutine(FadeText(1, 0));

        messageText.text = ""; // Asegurar que el texto no quede visible
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0;
        Color textColor = messageText.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            messageText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }
    }
}
