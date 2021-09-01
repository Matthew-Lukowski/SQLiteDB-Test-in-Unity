using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy
{
    public string Name { get; set; }

    public string Sport { get; set; }

    public string ImgString { get; set; }

    public Trophy(string Name, string Sport, string ImgString) {
        this.Name = Name;
        this.Sport = Sport;
        this.ImgString = ImgString;
    }

}
