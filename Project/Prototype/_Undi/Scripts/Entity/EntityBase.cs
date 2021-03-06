﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public abstract class EntityBase : MonoBehaviour
{

	#region Variables
	protected enum States
	{
		Init = 0,
		Appearing,
		Normal,
		Hurting,
		Dying,
		Dead,
		MaxValues
	}
	protected States currentState { private set; get; }

	// Components
	// [Header("Components")]
	private Animator animator;
	new protected Rigidbody rigidbody;
	new private Collider collider;
	new private SpriteRenderer renderer;

	// Data
	// [Header("Data")]
	public int id;
	[SerializeField] protected int health;
	protected const int healthMax = 5;
	//private bool isInvulnerable;
	//private float invulnerabilityTime = -1f;
	//private const uint invulnerabilityFrames = 5u;

	// Physics
	//[Header("Physics")]
	[Header("Base configuration")]
	[SerializeField] protected float runSpeed = 4f;


	// Tweens
	//[Header("Tweens")]
	//public TweenScale tweenAppear;
	//public TweenScale tweenDisappear;
	private Vector3 tweenAppearScaleBegin = Vector3.zero;
	private Vector3 tweenAppearScaleFinish = Vector3.one;
	private Vector3 tweenDisappearScaleBegin = Vector3.one;
	private Vector3 tweenDisappearScaleFinish = Vector3.zero;

	// Events
	public delegate void Delegate();
	public Delegate OnAppear;
	public Constants.Method OnDie;
	public Delegate OnExitMap;
	public Delegate OnCollideWithEntity;

	public delegate void DelegateInt( int newHealth );
	public DelegateInt OnHurt;
	#endregion


	#region Monobehaviour
	private void Awake()
	{
		Initialize();
	}

	private void Start()
	{
		ChangeState( States.Init );
	}

	protected virtual void Initialize()
	{
		if( animator == null )
		{
			animator = GetComponent<Animator>();
		}
		if( animator == null )
		{
			animator = GetComponentInChildren<Animator>();
		}
		if( animator == null )
		{
			//Debug.LogWarning("Animator wasn't setted in " + this.gameObject.name);
		}

		if( rigidbody == null )
		{
			rigidbody = transform.GetComponent<Rigidbody>();
		}
		if( rigidbody == null )
		{
			rigidbody = transform.GetComponentInChildren<Rigidbody>();
		}
		if( rigidbody == null )
		{
			//Debug.LogWarning("Rigidbody wasn't setted in " + this.gameObject.name);
		}

		if( renderer == null )
		{
			renderer = GetComponent<SpriteRenderer>();
		}
		if( renderer == null )
		{
			renderer = GetComponentInChildren<SpriteRenderer>();
		}
		if( renderer == null )
		{
			//Debug.LogWarning("Renderer wasn't setted in " + this.gameObject.name);
		}

		if( collider == null )
		{
			collider = GetComponent<Collider>();
		}
		if( collider == null )
		{
			collider = GetComponentInChildren<Collider>();
		}
		if( collider == null )
		{
			//Debug.LogWarning("Collider wasn't setted in " + this.gameObject.name);
		}

	}
	#endregion


	#region Life cycle
	protected void ChangeState( States to )
	{
		currentState = to;
		Debug.Log("Change the entity " + name + " state to: " + currentState);
	}

	#endregion


	#region Entity management
	public virtual void Set( int idP )
	{
		id = idP;
	}



	private void Die()
	{
		// TODO: Check integrity, this should be better integrated with entitymanager
		if( OnDie != null )
		{
			OnDie();
		}

		// TODO: Does this destroy everything?
		//Director.Instance.managerEntity.RemovePlayer( id );

		//Debug.Log(name + " has died.");
	}

	private void Reappear()
	{
		ChangeState( States.Appearing );
		//transform.localPosition = Vector2.zero;
		//transform.localPosition = initialPosition;
		rigidbody.velocity = Vector2.zero;
	}
	#endregion


	#region Animations and events
	private void PlayAnimationAppearing()
	{
		// Play animation, when it is finished change state to Normal
		// Play animation, when it is finished change state to Normal
		float scaleInit = 0f;
		float scaleFinish = 1f;
		float time = 0.4f;
		Ease ease = Ease.OutBack;

		transform.localScale.Set( scaleInit, scaleInit, transform.localScale.z );
		transform.DOScale( new Vector3( scaleFinish, scaleFinish, transform.localScale.z ), time )
						  .SetEase( ease )
						  .OnComplete( PlayAnimationAppearingHelper );
	}

	private void PlayAnimationAppearingHelper()
	{
		ChangeState( States.Normal );
	}

	private void PlayAnimationHurting()
	{
		// Play animation, when it is finished change state to Normal
		float scaleInit = 1f;
		float scaleFinish = 1.2f;
		float time = 0.4f;
		Ease ease = Ease.InOutBack;

		transform.localScale.Set( scaleInit, scaleInit, transform.localScale.z );
		transform.DOScale( new Vector3( scaleFinish, scaleFinish, transform.localScale.z ), time )
						  .SetEase( ease )
						  .SetLoops( 2, LoopType.Yoyo );
		//.OnComplete( Reappear );
	}

	private void PlayAnimationDying()
	{
		// Play animation, when it is finished change state to Dead
		float scaleInit = 1f;
		float scaleFinish = 0f;
		float time = 0.5f;
		Ease ease = Ease.OutBack;

		transform.localScale.Set( scaleInit, scaleInit, transform.localScale.z );
		transform.DOScale( new Vector3( scaleFinish, scaleFinish, transform.localScale.z ), time )
						  .SetEase( ease )
						  .OnComplete( PlayAnimationDyingHelper );
	}

	private void PlayAnimationDyingHelper()
	{
		ChangeState( States.Dead );
	}
	#endregion


	#region Physics
	/*
	public void MoveUp()
	{
		rigidbody.AddForce( Vector2.up * runSpeed, ForceMode.Force );
	}
	public void MoveDown()
	{
		rigidbody.AddForce( Vector2.down * runSpeed, ForceMode.Force );
	}
	public void MoveLeft()
	{
		rigidbody.AddForce( Vector2.left * runSpeed, ForceMode.Force );
	}
	public void MoveRight()
	{
		rigidbody.AddForce( Vector2.right * runSpeed, ForceMode.Force );
	}
	*/
	#endregion


	#region Collision
	void OnTriggerExit( Collider col )
	{
		//// For example, when exiting the game zone
		//if (col.gameObject.CompareTag( "Boundary" ))
		//{
		//    //Die();
		//    if (OnExitMap != null)
		//    {
		//        OnExitMap();
		//    }
		//}
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		// For example, when touching another entity
		//if (collision.gameObject.CompareTag( "Player" ))
		//{
		//    if (OnCollideWithEntity != null)
		//    {
		//        OnCollideWithEntity();
		//    }
		//}
	}
	#endregion
}
