using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ConveyorSettings : ScriptableObject
{
    [Header("Speed & Acceleration")]
    public float accel;
    public AnimationCurve accelFalloff;
    public float maxSpeed;

    [Header("Push Curves")]
    public AnimationCurve horizontalPushCurve; // returns direction object should be pushed based on normalized relative position of the object HORIZONTALLY
    public AnimationCurve verticalPushCurve;   // returns direction object should be pushed based on normalized relative position of the object VERTICALLY

    public Vector3 CalculatePush(Vector3 normRelPos) // takes normalized relative position of object on conveyor
    {
        float horizontal = horizontalPushCurve.Evaluate(normRelPos.x + 0.5f); // assume north up
        float vertical = verticalPushCurve.Evaluate(normRelPos.z + 0.5f);

        return new Vector3(horizontal, 0, vertical);

    }

    public float CalculateAccel(float currSpeed, float fixedDeltaTime)
    {
        if (currSpeed > maxSpeed || maxSpeed == 0) return 0;

        float newAccel = 50 * fixedDeltaTime * accel * accelFalloff.Evaluate(currSpeed / maxSpeed);
        return newAccel;
    }
}
