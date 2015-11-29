using UnityEngine;
using System.Collections;

public class CharacterControllerScript : MonoBehaviour
{
	public float MaxSpeed = 10f; 
	public float JumpSpeed = 500f; 
	public float GroundRadius = 1f;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	
	private bool isFacingRight = true;
	private Animator animation;
	private Rigidbody2D rigidbody2D;
	private bool isGrounded = false;
	private Quaternion rotation;
	
	
	private void Start()
	{
		this.animation = GetComponent<Animator>();
		this.rotation = this.transform.rotation;
	}
	
	private void FixedUpdate()
	{
		this.rigidbody2D = GetComponent<Rigidbody2D>();
		Collider2D collider = Physics2D.OverlapCircle (groundCheck.position, GroundRadius, whatIsGround);
		this.isGrounded = collider == null? false: true; 
		System.Console.WriteLine ("Is Ground = " + isGrounded);
		
		this.animation.SetBool("Ground", this.isGrounded);
		this.animation.SetFloat("VSpeed", this.rigidbody2D.velocity.y);

		if (!this.isGrounded)
			return;
		float move = Input.GetAxis("Horizontal");

		this.animation.SetFloat("Speed", Mathf.Abs(move));
		this.rigidbody2D.velocity = new Vector2(move * MaxSpeed, rigidbody2D.velocity.y);

		if(move > 0 && !isFacingRight)
			this.Flip();
		else if (move < 0 && isFacingRight)
			this.Flip();
	}
	
	private void Update()
	{
		this.transform.rotation = this.rotation;
		if (this.isGrounded && Input.GetKeyDown (KeyCode.Space)) 
		{
			this.animation.SetBool("Ground", false);
			this.rigidbody2D.AddForce(new Vector2(0, JumpSpeed));				
		}
	}
	
	private void Flip()
	{
		this.isFacingRight = !this.isFacingRight;
		Vector3 theScale = this.transform.localScale;
		theScale.x *= -1;
		this.transform.localScale = theScale;
	}
}