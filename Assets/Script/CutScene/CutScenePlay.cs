using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutScenePlay : MonoBehaviour
{
    public GameObject train;
    public GameObject gangster;
    public GameObject mafia1;
    public GameObject gangster1;
    public Image TBlack;
    public Image BBlack;
    public TextMeshProUGUI[] subtitles;
    public GameObject[] objects;

    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
        StartCoroutine(PlayCutScene());
    }

    IEnumerator PlayCutScene()
    {
        yield return new WaitForSeconds(1f);
        TBlack.gameObject.SetActive(true);
        BBlack.gameObject.SetActive(true);
        gangster.SetActive(true);

        yield return new WaitForSeconds(2f);
        subtitles[0].gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        train.SetActive(true);
        subtitles[0].gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        for (int i = 1; i < 7; i++)
        {
            subtitles[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);

            subtitles[i].gameObject.SetActive(false);
            yield return new WaitForSeconds(1.1f);
        }

        subtitles[7].gameObject.SetActive(true);

        for (int i = 0; i < 8; i++)
        {
            objects[i].SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }

        subtitles[7].gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        subtitles[8].gameObject.SetActive(true);

        for (int i = 8; i < 41; i++)
        {
            objects[i].SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }

        subtitles[8].gameObject.SetActive(false);
        gangster1.SetActive(true);
        mafia1.SetActive(true);

        yield return new WaitForSeconds(1f);
        subtitles[9].gameObject.SetActive(true);
    }
}