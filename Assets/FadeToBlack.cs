using UnityEngine;

public class FadeToBlack : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoFadeToBlack()
    {
        animator.SetTrigger("FadeOut");

    }
}
