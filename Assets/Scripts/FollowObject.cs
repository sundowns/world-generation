using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject target;

    public float speed = 2.0f;

    public Vector3 offset;

    void Update()
    {
        float delta = speed * Time.deltaTime;

        if (Vector3.Distance(this.transform.position, target.transform.position) > offset.magnitude)
        {
            Vector3 position = this.transform.position;
            position.z = Mathf.Lerp(this.transform.position.z, target.transform.position.z + offset.z, delta);
            position.x = Mathf.Lerp(this.transform.position.x, target.transform.position.x + offset.x, delta);
            position.y = Mathf.Lerp(this.transform.position.y, target.transform.position.y + offset.y, delta);

            this.transform.position = position;
        }

        this.transform.LookAt(target.transform.position);
    }
}
