using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;
public class GlobalUpdater : MonoBehaviour
{
    public InputField field = null;
    StringVariable myString;
    // Start is called before the first frame update
    void Start()
    {
        if (field)
        {
            field.onValueChanged.AddListener(updateGlobal);
        }

        PersistentVariablesSource source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
        myString = source["global"]["player-name"] as StringVariable;
    }

    private void updateGlobal(string arg0)
    {
        myString.Value = arg0;
    }


}
