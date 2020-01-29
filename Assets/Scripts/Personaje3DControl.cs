using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje3DControl : MonoBehaviour
{

	public float velMovimientoA;
	public float velGiroA;
	const float MOV_MULTIPLIER = 10f;
	const float ROT_MULTIPLIER = 10f;

	public float velMovimientoB;
	public float velGiroB;

	public float radioDeteccion;
	public float fuerzaSalto;
	public Transform detectorSuelo;

	public LayerMask queEsSuelo;
	[SerializeField] private bool enSuelo;

	Vector3 angularVelocity;
	Vector3 moveVector;

	Rigidbody rb;
	Animator animator;

	public GameObject carameloObj;
	public Transform posicionTiro;
	public float delayDisparo = 0.2f;

	
	void Start()
    {

		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame

	public void Update()
	{
		CheckInput();

		if (Input.GetButtonDown("Fire1"))
		{
			if (animator.GetCurrentAnimatorStateInfo(1).IsName("tiro"))
				return;

			animator.SetTrigger("Tiro");
			Invoke("tiroGalleta", delayDisparo);
		}

	}

	public void FixedUpdate()
    {
		CheckGround();
		MovimientoA();
		//MovimientoB();
	}


	void MovimientoA()
	{
	
		if(Input.GetButtonDown("Jump") && enSuelo)
		{
			rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
			animator.SetBool("EnSuelo", false);
		}

		if (!enSuelo)
			animator.SetFloat("VelocidadVertical", rb.velocity.y);

		rb.angularVelocity = angularVelocity;
		rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
	}

	void MovimientoB()
	{
		float velX = Input.GetAxisRaw("Horizontal");
		float velZ = Input.GetAxisRaw("Vertical");

		Vector3 movVector = new Vector3(velX, 0, velZ).normalized * velMovimientoB;

		rb.velocity = new Vector3(movVector.x, rb.velocity.y, movVector.z);

		animator.SetFloat("Direccion", 1);

		if (velX != 0 || velZ != 0)
		{

			Quaternion rotacion = Quaternion.LookRotation(new Vector3(velX, 0, velZ), Vector3.up);
			rb.rotation = Quaternion.Slerp(transform.rotation, rotacion, velGiroB * Time.fixedDeltaTime);
			animator.SetFloat("WalkSpeed", 1);
		}
		else
			animator.SetFloat("WalkSpeed", 0);
		
	}

	void CheckGround()
	{
		Collider[] colisiones = Physics.OverlapSphere(detectorSuelo.position, radioDeteccion, queEsSuelo);

		if (colisiones.Length > 0)
		{
			enSuelo = true;
		}
		else
		{
			enSuelo = false;
		}

		animator.SetBool("EnSuelo", enSuelo);
	}
	void setAnimatorState()
	{
		animator.SetFloat("WalkSpeed", Mathf.Abs(Input.GetAxisRaw("Vertical")));
		animator.SetFloat("Direccion", Input.GetAxisRaw("Vertical"));
	}

	void CheckInput()
	{
		moveVector = transform.forward * Input.GetAxisRaw("Vertical") * velMovimientoA * MOV_MULTIPLIER * Time.deltaTime;
		angularVelocity = Vector3.up * Input.GetAxisRaw("Horizontal") * velGiroA * ROT_MULTIPLIER * Time.deltaTime;

		setAnimatorState();
	}

	void tiroGalleta()
	{
		Instantiate(carameloObj, posicionTiro.position, posicionTiro.rotation);
	}
}
