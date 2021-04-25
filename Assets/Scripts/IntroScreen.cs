using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroScreen : MonoBehaviour
{
    //public CanvasGroup canvasGroup;
    //public AnimationCurve alphaCurve;
    public TextMeshProUGUI continueText;

    public float minDuration;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if( timer > minDuration )
        {
            //canvasGroup.alpha = 1;
            continueText.gameObject.SetActive(true);
            if( Input.GetMouseButtonDown(0) )
            {
                GameManager.instance.RestartRoom();
                gameObject.SetActive(false);
            }
        }
        else
        {
            //float lerp = timer / minDuration;
            //canvasGroup.alpha = alphaCurve.Evaluate(lerp);
        }
    }
}
