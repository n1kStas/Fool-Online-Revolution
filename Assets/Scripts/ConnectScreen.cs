using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectScreen : MonoBehaviour
{
    public List<string> phrases = new List<string>();

    public Text phrase;
    public Image progressLoad;
    public Image fonLock;
    private IEnumerator _showPhraseIEnumerator;
    private float loadingGameProgress;
    public GameObject glowingEyes;

    private void Start()
    {
        _showPhraseIEnumerator = ShowPhraseIEnumerator();
        StartCoroutine(_showPhraseIEnumerator);
    }

    public IEnumerator ShowPhraseIEnumerator()
    {
        int indexPhrase = Random.Range(0, phrases.Count);
        phrase.text = phrases[indexPhrase];
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= 1.5f)
            {
                timer = 0;
                indexPhrase = Random.Range(0, phrases.Count);
                phrase.text = phrases[indexPhrase];
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void ShowProgressIEnumerator(int progressCount)
    {        
        
        for(int i = 0; i < progressCount; i++)
        {
            loadingGameProgress += 0.01f;
            progressLoad.fillAmount = loadingGameProgress;
        }
    }

    public IEnumerator ConnectionComplited()
    {
        fonLock.enabled = false;
        progressLoad.fillAmount = 1;
        yield return new WaitForSeconds(1f);
        Destroy(glowingEyes);
    }

    public void ErrorConectionToServer()
    {
        StopCoroutine(_showPhraseIEnumerator);
        progressLoad.fillAmount = 1;
        progressLoad.color = Color.red;
        phrase.text = "Ошибка соединения";
    }
}
