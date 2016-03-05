using UnityEngine;
using System.Collections;

public class Target
{

    private static Target _in = new Target();

    public static Target In
    {
        
        
        get
        {
            string address="";
            return _in;
        }
    }
}
