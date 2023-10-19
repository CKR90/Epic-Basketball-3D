using UnityEngine;

public class ThrowingVelocityToReachTheTarget : MonoBehaviour
{
    public static Vector3 CalculateVelocity(Vector3 objectPosition, Vector3 targetPosition, Vector3 targetVelocity, bool isFast)
    {
        Vector3 velocity = GetVelocity(objectPosition, targetPosition, isFast);

        if(targetVelocity.magnitude > 0.001f)
        {
            Vector3 objectH = objectPosition;
            Vector3 targetH = targetPosition;
            objectH.y = 0f;
            targetH.y = 0f;
            float distanceH = Vector3.Distance(targetH, objectH);
            velocity.y = 0f;
            float movingTime = distanceH / velocity.magnitude;

            Vector3 targetOffset = targetVelocity * movingTime;
            targetPosition += targetOffset;
            velocity = GetVelocity(objectPosition, targetPosition, isFast);
        }
        return velocity;
    }
    private static Vector3 GetVelocity(Vector3 objectPosition, Vector3 targetPosition, bool isFast)
    {
        float angle = atan360xz(targetPosition - objectPosition);
        float distance_horizontal = Vector2.Distance(new Vector2(objectPosition.x, objectPosition.z), new Vector2(targetPosition.x, targetPosition.z));
        float h_max = 0f;
        if (isFast)
        {
            h_max = Mathf.Max(0f, (targetPosition.y + distance_horizontal / 30f) - objectPosition.y);
        }
        else h_max = Mathf.Max(0f, (targetPosition.y + distance_horizontal / 5f) - objectPosition.y);
        float V_0y = Mathf.Sqrt(2f * Mathf.Abs(Physics.gravity.y) * h_max);
        float H_end = targetPosition.y - objectPosition.y;
        float T_end = deltaMax(0.5f * Mathf.Abs(Physics.gravity.y), -V_0y, H_end);
        float Vy_end = Mathf.Sqrt(V_0y * V_0y + Physics.gravity.y * H_end);
        float T_freefall = Vy_end / Mathf.Abs(Physics.gravity.y);
        float T_to_Hmax = T_end - T_freefall;
        float X_distance_when_Hmax = distance_horizontal * (T_to_Hmax / T_end);
        float Vx = X_distance_when_Hmax / T_to_Hmax;
        Vector3 velocity = new Vector3(Vx, V_0y, 0f);
        velocity = roty(velocity.x, velocity.y, velocity.z, -angle);

        return velocity;
    }


    private static float deltaMax(float a, float b, float c)
    {
        float k1 = (-b + Mathf.Sqrt(b * b - 4f * a * c)) / (2 * a);
        float k2 = (-b - Mathf.Sqrt(b * b - 4f * a * c)) / (2 * a);
        float k = Mathf.Max(k1, k2);

        return k;
    }
    private static float radyan = Mathf.PI / 180;
    private static float atan360xz(Vector3 v)
    {
        float a = 0f;
        if (v.x == 0f && v.z == 0f) a = 0f;
        else if (v.x == 0f && v.z > 0f) a = 90f;
        else if (v.x == 0f && v.z < 0f) a = 270f;
        else
        {
            a = atan(Mathf.Abs(v.z / v.x));
            if (v.x < 0f && v.z >= 0f) a = 180f - a;
            else if (v.x < 0f && v.z < 0f) a += 180f;
            else if (v.x >= 0f && v.z < 0f) a = 360f - a;
        }
        return a;
    }
    private static float atan(float x)
    {
        float xVal;
        xVal = Mathf.Atan(x) / radyan;
        if (double.IsNaN(xVal)) xVal = 90;
        return xVal;
    }
    private static Vector3 rotx(float x, float y, float z, float a)
    {
        Vector3 H = new Vector3();
        H.x = x;
        H.y = Mathf.Cos(a * radyan) * y - Mathf.Sin(a * radyan) * z;
        H.z = Mathf.Sin(a * radyan) * y + Mathf.Cos(a * radyan) * z;
        if (H.x < 0.0001f && H.x > -0.0001f) H.x = 0f;
        if (H.y < 0.0001f && H.y > -0.0001f) H.y = 0f;
        if (H.z < 0.0001f && H.z > -0.0001f) H.z = 0f;
        return H;


    }
    private static Vector3 roty(float x, float y, float z, float a)
    {
        Vector3 H = new Vector3();
        H.x = Mathf.Cos(a * radyan) * x + Mathf.Sin(a * radyan) * z;
        H.y = y;
        H.z = -Mathf.Sin(a * radyan) * x + Mathf.Cos(a * radyan) * z;
        if (H.x < 0.0001f && H.x > -0.0001f) H.x = 0f;
        if (H.y < 0.0001f && H.y > -0.0001f) H.y = 0f;
        if (H.z < 0.0001f && H.z > -0.0001f) H.z = 0f;
        return H;


    }
    private static Vector3 rotz(float x, float y, float z, float a)
    {
        Vector3 H = new Vector3();
        H.x = Mathf.Cos(a * radyan) * x - Mathf.Sin(a * radyan) * y;
        H.y = Mathf.Sin(a * radyan) * x + Mathf.Cos(a * radyan) * y;
        H.z = z;
        if (H.x < 0.0001f && H.x > -0.0001f) H.x = 0f;
        if (H.y < 0.0001f && H.y > -0.0001f) H.y = 0f;
        if (H.z < 0.0001f && H.z > -0.0001f) H.z = 0f;
        return H;


    }
}
