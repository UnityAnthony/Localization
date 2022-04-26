using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// This example expects a Smart String with a named placeholder of `TimeNow`, such as "The time now is {TimeNow}".
/// </summary>
public class LocalizedStringSmart : MonoBehaviour
{
    public LocalizedString myString;

   public  void UpdateString()
    {
       Debug.Log("update "+ myString.GetLocalizedString());
    }


}

