using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public GameObject Square;
    bool change = false;
    bool change2 = false;
    int times = 0;

    public Button start, quit;

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

    public IEnumerator FadeBlackSquare(bool fade = true, int speed = 3) {
        Color objectColor = Square.GetComponent<Image>().color;
        float fadeAmount;
        //int times = 0;

        if (fade) {
            while (Square.GetComponent<Image>().color.a < 1 && change == true) {
                fadeAmount = objectColor.a + (speed*Time.deltaTime);
                times++;
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                Square.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            change = false;
            change2 = true;
            start.gameObject.SetActive(false);
            quit.gameObject.SetActive(false);
        } else {
            while (Square.GetComponent<Image>().color.a > 0 && change2 == true) {
                //Debug.Break();
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