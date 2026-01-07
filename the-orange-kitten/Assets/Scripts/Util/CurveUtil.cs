using System.Linq;
using UnityEngine;

public static class CurveUtil
{

    public static float GetMaxTime(this AnimationCurve curve)
        => curve.keys.Select(keyframe => keyframe.time).Max();

    public static float GetMinTime(this AnimationCurve curve)
        => curve.keys.Select(keyframe => keyframe.time).Min();

}
