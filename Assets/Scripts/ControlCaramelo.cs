using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControlCaramelo : MonoBehaviour
{
	public GameObject explosionObj;

	public float fuerza = 3000f;
	public float tiempoVida = 3f;
    void Start()
    {
		GetComponent<Rigidbody>().AddForce(transform.forward * fuerza);
		Destroy(gameObject, tiempoVida);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	 void OnCollisionEnter(Collision collision)
	{
		Instantiate(explosionObj, collision.contacts[0].point, Quaternion.Euler(0, 0, 0));
		Destroy(gameObject);	
	}
}
