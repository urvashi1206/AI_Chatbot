using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject Camera_1;
    public GameObject Camera_2;
    public int Manager;


    public void ChangeCamera()
    {
        GetComponent<Animator>().SetTrigger("Change");
    }

    public void ManageCamera()
    {
        if (Manager == 0)
        {
            Cam2();
            Manager = 1;
        }

        else
        {
            Cam1();
            Manager = 0;
        }
    }

    void Cam1()
    {
        Camera_1.SetActive(true);
        Camera_2.SetActive(false);
    }

    void Cam2()
    {
        Camera_1.SetActive(false);
        Camera_2.SetActive(true);
    }
}
