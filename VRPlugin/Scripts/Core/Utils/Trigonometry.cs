using System;
using UnityEngine;

public static class Trigonometry 
{
    public static bool GetCastPoint(Vector3 from, Vector3 to, Vector3 pointToCast, out Vector3 result)
    {
        return GetCastPoint(from, to, pointToCast, out result, out float signedFac);
    }
    
    public static bool GetCastPoint(Vector3 from, Vector3 to, Vector3 pointToCast, out Vector3 result, out float signedFac)
    {
            Vector3 direction = to - from;
            Vector3 normal = direction;
            Vector3 translation = to - from;
            Debug.Log(translation);
            float dot = Vector3.Dot(normal, translation);
            if (Mathf.Abs(dot) > Single.Epsilon)
            {
                Vector3 distance1 = from - pointToCast;
                signedFac = -Vector3.Dot(normal, distance1) / dot;
                float fac = Mathf.Clamp01(signedFac);
                
                translation *= fac;
                result = from + translation;
                return true;
            }
    
            Plane plane  = new Plane(direction, pointToCast);
            
            result = Vector3.zero;
            signedFac = 0f;
            return false;
        }
}
