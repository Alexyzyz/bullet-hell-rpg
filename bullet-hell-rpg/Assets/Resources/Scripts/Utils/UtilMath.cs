using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilMath
{

    public static Vector2 GetRandomUnitVector2()
    {
        float random = Random.Range(0f, 360f);
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }

}
