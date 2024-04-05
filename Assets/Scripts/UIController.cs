using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public GameObject Square;
    bool change = false;
    bool change2 = false;

    public Button start, quit, load, collections, settings;
    public GameObject foreground, background, title;

    public void Update() {
        if (change) {
            StartCoroutine(FadeBlackSquare());
        } else if (change2) {
            StartCoroutine(FadeBlackSquare(false));
        }
    }

    public void Change() {
        change = true;
    }

    public bool getChange() {
        return change;
    }

    public IEnumerator FadeBlackSquare(bool fade = true, int speed = 1) {
        Color objectColor = Square.GetComponent<Image>().color;
        float fadeAmount;

        if (fade) {
            while (Square.GetComponent<Image>().color.a < 1 && change == true) {
                fadeAmount = objectColor.a + (speed*Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                Square.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            change = false;
            change2 = true;
            start.gameObject.SetActive(false);
            quit.gameObject.SetActive(false);
            load.gameObject.SetActive(false);
            collections.gameObject.SetActive(false);
            settings.gameObject.SetActive(false);
            foreground.SetActive(false);
            background.SetActive(false);
            title.SetActive(false);
        } else {
            while (Square.GetComponent<Image>().color.a > 0 && change2 == true) {
                fadeAmount = objectColor.a - (speed*Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                Square.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            change2 = false;
        }

        //yield return new WaitForEndOfFrame();
    }


}