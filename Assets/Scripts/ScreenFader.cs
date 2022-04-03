using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour
{
    public float solidAlpha =1.0f;
    public float clearAlpha =0f;
    public float delay = 0f;
    public float timeToFade = 1f;

    public Button resetButton;
    public Button backButton;
    public Canvas overlay;
    public Text outOfMoveText;

    MaskableGraphic m_graphic;

    void Start()
    {
        resetButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        outOfMoveText.gameObject.SetActive(false);
        overlay.GetComponent<GraphicRaycaster>().enabled = false;
        m_graphic =  GetComponent<MaskableGraphic>();
        FadeOff();
    }

    IEnumerator FadeRoutine(float alpha)
    {
        yield return new WaitForSeconds(delay);
        m_graphic.CrossFadeAlpha(alpha, timeToFade, true);
    }

    public void FadeOn(bool isOutOfMove = false)
    {
        overlay.GetComponent<GraphicRaycaster>().enabled = true;
        outOfMoveText.gameObject.SetActive(isOutOfMove);
        resetButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        backButton.interactable = !isOutOfMove;
        StartCoroutine(FadeRoutine(solidAlpha));
    }

    public void FadeOff()
    {
        overlay.GetComponent<GraphicRaycaster>().enabled = false;
        resetButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        outOfMoveText.gameObject.SetActive(false);
        StartCoroutine(FadeRoutine(clearAlpha));
    }

}
