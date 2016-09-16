using UnityEngine;
using System.Collections;

namespace Surface
{
    public static class Noise
    {
        public static float Value1D(Vector3 point, float frequency)
        {
            point *= frequency;
            int i = Mathf.FloorToInt(point.x);
            i &= hashMask;
            return hash[i] * (1f / hashMask);
        }

        public static float Value2D(Vector3 point, float frequency)
        {
            point *= frequency;
            int ix = Mathf.FloorToInt(point.x);
            int iy = Mathf.FloorToInt(point.y);
            ix &= hashMask;
            iy &= hashMask;
            return hash[hash[ix] + iy] * (1f / hashMask);
        }

        public static float Value3D(Vector3 point, float frequency)
        {
            point *= frequency;
            int ix = Mathf.FloorToInt(point.x);
            int iy = Mathf.FloorToInt(point.y);
            int iz = Mathf.FloorToInt(point.z);
            ix &= hashMask;
            iy &= hashMask;
            iz &= hashMask;
            return hash[hash[hash[ix] + iy] + iz] * (1f / hashMask);
        }
    }
}
