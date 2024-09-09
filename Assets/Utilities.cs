using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneProperties
{
    public static float clearingRadius = 57f;
    public static Vector3 sceneCenter = new Vector3(200f, 0f, 200f);

    public   static Transform playerTransform = GameObject.Find("Player").transform;
    public static Vector2 playerXZPosition => playerTransform.position.ToXZ();
    public static float playerDistanceFromCenter => (playerXZPosition - sceneCenter.ToXZ()).magnitude;

    private static Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
    public static float TerrainHeightAtPosition(Vector2 position) => terrain.SampleHeight(new Vector3(position.x, 0f, position.y));
    public static float TerrainHeightAtPosition(Vector3 position) => terrain.SampleHeight(position);

    public static bool IsInClearing(Vector3 position) => Vector2.Distance(position.ToXZ(), sceneCenter.ToXZ()) < clearingRadius + 4f;
    public static bool LineIntersectsClearing(Vector2 start, Vector2 end) => Calculations.LineIntersectsCircle(start, end, sceneCenter.ToXZ(), clearingRadius);
    public static Vector3 GetTangentPointNearestToDestination(Vector3 point, float radiusExtension, Vector3 destination)
    {
        Vector3 tangentPoint1, tangentPoint2;
        Calculations.CalculateTangentPoints(
            point,
            sceneCenter,
            clearingRadius + radiusExtension,
            out tangentPoint1,
            out tangentPoint2);

        float tangent1Distance = Vector3.Distance(tangentPoint1, destination);
        float tangent2Distance = Vector3.Distance(tangentPoint2, destination);

        return tangent1Distance < tangent2Distance
            ? tangentPoint1
            : tangentPoint2;
    }
}

public static class Calculations
{
    public static bool LineIntersectsCircle(Vector2 start, Vector2 end, Vector2 center, float radius)
    {
        Vector2 line = end - start;
        Vector2 startToCenter = start - center;

        float a = Vector2.Dot(line, line);
        float b = 2 * Vector2.Dot(startToCenter, line);
        float c = Vector2.Dot(startToCenter, startToCenter) - radius * radius;
        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
        {
            return false;
        }

        discriminant = Mathf.Sqrt(discriminant);
        float t1 = (-b - discriminant) / (2 * a);
        float t2 = (-b + discriminant) / (2 * a);

        return (t1 >= 0 && t1 <= 1) || (t2 >= 0 && t2 <= 1);
    }

    public static void CalculateTangentPoints(
        Vector3 point,
        Vector3 center,
        float radius,
        out Vector3 tangentPoint1,
        out Vector3 tangentPoint2)
    {
        Vector3 centerToPoint = point - center;
        float distance = centerToPoint.magnitude;

        float angleOffset = Mathf.Asin(radius / distance);
        float baseAngle = Mathf.Atan2(centerToPoint.z, centerToPoint.x);

        float theta1 = baseAngle + angleOffset;
        tangentPoint1 = new Vector3(
            center.x + radius * Mathf.Cos(theta1),
            point.y,
            center.z + radius * Mathf.Sin(theta1)
        );

        float theta2 = baseAngle - angleOffset;
        tangentPoint2 = new Vector3(
            center.x + radius * Mathf.Cos(theta2),
            0f,
            center.z + radius * Mathf.Sin(theta2)
        );
    }
}

public static class VectorExtensions
{
    public static Vector2 ToXZ(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
}
