using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class AnimatedText : MonoBehaviour
{
    public TMP_Text textcomponent;
    private IEnumerator coroutine;
    public void DisplayText(string textkey, float durationSec)
    {
        textcomponent.gameObject.SetActive(true);
        var localizedstring = new LocalizedString("CUTSCENE", textkey);
        textcomponent.text = localizedstring.GetLocalizedString();
        coroutine = EndText(durationSec);
        StartCoroutine(coroutine);
    }
    public IEnumerator EndText(float sec)
    {
        yield return new WaitForSeconds(sec);
        textcomponent.gameObject.SetActive(false);

    }
}
