using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public static float Speed = 50f;

    void Update()
    {
        transform.position -= new Vector3(0, Speed * Time.deltaTime, 0);
    }
}
