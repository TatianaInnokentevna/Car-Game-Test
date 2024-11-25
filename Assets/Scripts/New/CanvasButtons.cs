using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasButtons : MonoBehaviour
{
    public Sprite btn, btnPressed;
    private Image image;
    public GameObject activeCanvas;

    public void Start()
    {
        image = GetComponent<Image>();
    }

    public void StartGame()
    {
        Debug.Log("Метод StartGame вызван.");
        StartCoroutine(LoadScene("Game"));
    }

    public void StartScene()
    {

        StartCoroutine(LoadScene("Start"));
    }

    public void SetPressedBtn()
    {
        image.sprite = btnPressed;
        transform.GetChild(0).localPosition -= new Vector3(0, 5f, 0);
    }

    public void SetDefaultBtn()
    {
        image.sprite = btn;
        transform.GetChild(0).localPosition += new Vector3(0, 5f, 0);
    }


    public void RestartScene()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

    }

    IEnumerator LoadScene(string name)
    {
        float fadeTime = Camera.main.GetComponent<Fading>().Fade(1f);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(name);
    }

   


}
