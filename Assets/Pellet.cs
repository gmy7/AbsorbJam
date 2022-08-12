using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public bool isEditable;
    public static System.Random rand;
    public static bool randomMade;

    private void Start()
    {
        CreateStaticRand();

        int randomNum = rand.Next(0, 2);
        if (randomNum == 0)
            isEditable = true;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (isEditable)
            sr.color = Color.green;
        else
            sr.color = Color.red;

    }
    private static void CreateStaticRand()
    {
        if (!randomMade)
        {
            rand = new System.Random();
            randomMade = true;
        }
    }
}
