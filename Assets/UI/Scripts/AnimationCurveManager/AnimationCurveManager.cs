using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurveManager : GenerticSingleton<AnimationCurveManager>
{
    public AnimationCurve skinPersentCurve;
    public AnimationCurve objScaleAndBackCurve;
    public AnimationCurve objScaleCurve;
    public AnimationCurve objMoveCurve;
    public AnimationCurve rotageCurve;
    public AnimationCurve spiralCountEndAnimPosCurve;
    public AnimationCurve spiralCountEndAnimScaleCurve;
    public AnimationCurve spiralCountPositionAnimCurve;
    public AnimationCurve spiralCountScaleAnimCurve;
    public AnimationCurve rankPointAnim;
    public AnimationCurve speedSpin;
}
