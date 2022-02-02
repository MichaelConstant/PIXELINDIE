using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorBullet : MonoBehaviour
{
    public float bulletSpeed;
    // Start is called before the first frame u

    void Start()
    {
        //transform.rotation.ToAngleAxis(out float angle, out Vector3 axis);
        //GetComponent<Rigidbody2D>().AddForce(axis * bulletSpeed);
        GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
        Invoke("DestroyItself", 1f);
    }

    void DestroyItself()
    {
        Destroy(this.gameObject);
    }
}
