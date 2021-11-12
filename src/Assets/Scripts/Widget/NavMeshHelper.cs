using UnityEngine;
using UnityEngine.AI;

namespace Widget
{
    public static class NavMeshHelper
    {
        public static Vector3 GetRandomLocation(Transform transform, float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;

            Vector3 samplePosition = Vector3.zero;

            if (NavMesh.SamplePosition(randomDirection, out var hit, radius, 1))
            {
                samplePosition = hit.position;
            }

            return samplePosition;
        }
    }
}