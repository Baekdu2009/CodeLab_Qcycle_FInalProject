using UnityEngine;

public class Test : MonoBehaviour
{
    Animator MyAnimator;
    public int speed = 6;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MyAnimator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        MyAnimator.SetBool("Walk", false);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MyAnimator.SetBool("Walk", true);
            transform.Translate(0.001f * speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MyAnimator.SetBool("Walk", true);
            transform.Translate(-0.001f * speed, 0, 0);
        }

    }
}
