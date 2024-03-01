using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public GameObject Square;
    bool change = false;

    public void Update() {
        if (change) {
            StartCoroutine(FadeBlackSquare());
        }
    }

    public void Change() {
        change = true;
    }

    public IEnumerator FadeBlackSquare(bool fade = true, int speed = 5) {
        change = false;
        Color objectColor = Square.GetComponent<Image>().color;
        float fadeAmount;
        Debug.Log("uh");

        if (fade) {
            while (Square.GetComponent<Image>().color.a < 1) {
                fadeAmount = objectColor.a + (speed*Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                Square.GetComponent<Image>().color = objectColor;
                Debug.Log("got here");
                yield return null;
            }

        }


        yield return new WaitForEndOfFrame();
    }


}