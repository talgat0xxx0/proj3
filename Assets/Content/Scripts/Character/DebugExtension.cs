using UnityEngine;

public static class DebugExtension
{
    public static void DebugWireSphere(Vector3 position, Color color, float radius)
    {
        float angle = 10f;
        float duration = 0.1f; // persists long enough to see
        Vector3 lastPoint = Vector3.zero;
        Vector3 nextPoint = Vector3.zero;
        Vector3 lastPointY = Vector3.zero;
        Vector3 nextPointY = Vector3.zero;
        Vector3 lastPointZ = Vector3.zero;
        Vector3 nextPointZ = Vector3.zero;

        for (int i = 0; i <= 360; i += (int)angle)
        {
            nextPoint = new Vector3(Mathf.Sin(Mathf.Deg2Rad * i) * radius, 0, Mathf.Cos(Mathf.Deg2Rad * i) * radius);
            nextPointY = new Vector3(Mathf.Sin(Mathf.Deg2Rad * i) * radius, Mathf.Cos(Mathf.Deg2Rad * i) * radius, 0);
            nextPointZ = new Vector3(0, Mathf.Cos(Mathf.Deg2Rad * i) * radius, Mathf.Sin(Mathf.Deg2Rad * i) * radius);

            if (i > 0)
            {
                Debug.DrawLine(position + lastPoint, position + nextPoint, color, duration);
                Debug.DrawLine(position + lastPointY, position + nextPointY, color, duration);
                Debug.DrawLine(position + lastPointZ, position + nextPointZ, color, duration);
            }

            lastPoint = nextPoint;
            lastPointY = nextPointY;
            lastPointZ = nextPointZ;
        }
    }
}
