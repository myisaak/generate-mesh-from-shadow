using UnityEngine;
using System.Linq;

public class ShadowMesh : MonoBehaviour
{
    private void ProjectVertcies(Light source, GameObject obj, out Vector3[] verticies, out Vector3[] normals)
    {
        verticies = obj.GetComponent<MeshFilter>().mesh.vertices;
        normals = new Vector3[verticies.Length];

        Vector3 sourcePos = source.transform.position;
        Vector3[] projectedVerticies = new Vector3[verticies.Length];

        for (var i=0;i<verticies.Length;i++)
        {
            Ray direction = new Ray(sourcePos, obj.transform.TransformPoint(verticies[i]) - sourcePos);
            RaycastHit[] hits = Physics.RaycastAll(direction);

            if (hits.Length > 0)
            {
                if (hits[0].collider.gameObject != obj)
                {
                    Transform transform = hits[0].transform;
                    normals[i] = hits[0].normal;

                    projectedVerticies[i] = (hits[0].point);

                    // DEBUG
                    Debug.DrawLine(source.transform.position, hits[0].point, Color.red);
                    Debug.DrawRay(hits[0].point, hits[0].normal, Color.yellow);
                }
                else if (hits.Length > 1)
                {
                    Transform transform = hits[1].transform;
                    normals[i] = hits[1].normal;

                    projectedVerticies[i] = (hits[1].point);

                    // DEBUG
                    Debug.DrawLine(source.transform.position, hits[1].point, Color.red);
                    Debug.DrawRay(hits[1].point, hits[1].normal, Color.yellow);
                }
            }
        }
        
        verticies = projectedVerticies;
    }

    private void ManuiplateCuboid(Vector3[] verticies)
    {
        Vector3 averagePosition = new Vector3(verticies.Average(vector => vector.x),
                                              verticies.Average(vector => vector.y),
                                              verticies.Average(vector => vector.z));
        Vector3 averageScale = new Vector3(verticies.Max(vector => vector.x) - verticies.Min(vector => vector.x),
                                           verticies.Max(vector => vector.y) - verticies.Min(vector => vector.y),
                                           normalScale);

        transform.position = averagePosition;
        transform.localScale = averageScale;
    }

    public Light source;
    public GameObject cube;
    public float normalScale;

    private void LateUpdate()
    {
        Vector3[] verticies;
        Vector3[] normals;

        ProjectVertcies(source, cube, out verticies, out normals);

        ManuiplateCuboid(verticies);
    }
}
