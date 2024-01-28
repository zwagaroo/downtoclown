using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    public Animator animator;
    string phoneUp = "Phone Enter";
    string phoneDown = "Phone Exit";

    bool up = false;
    void Start()
    {
        PhoneUp();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            if (up)
            {
                PhoneDown();
            }
            else
            {
                PhoneUp();
            }
        }*/
    }

    // Update is called once per frame
    void PhoneUp()
    {
        animator.Play(phoneUp);
        up = true;
    }

    void PhoneDown()
    {
        animator.Play(phoneDown);
        up = false;
    }
}
