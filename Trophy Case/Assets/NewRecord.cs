using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRecord : MonoBehaviour
{
    public GameObject Name;
    public GameObject Sport;

    public void setTextBoxes(string sName, string sSport) {
        Name.GetComponent<Text>().text = sName;
        Sport.GetComponent<Text>().text = sSport;
    }

 

}
