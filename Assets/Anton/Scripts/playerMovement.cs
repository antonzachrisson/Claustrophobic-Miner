using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    private float horizontal;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private AudioSource source;
    [SerializeField] private AudioClip mineSound;
    [SerializeField] private AudioClip walkSound;
    private float walkSoundTimer = 0f;

    private float mineTimer = 0f;
    private SpriteRenderer rend;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite mineSprite;
    [SerializeField] private Sprite walkSprite1;
    [SerializeField] private Sprite walkSprite2;
    private bool walking = false;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        walking = false;
        bool takeStep = false;
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        float vectorLength = rb.velocity.magnitude;
        if(vectorLength > 0 && IsGrounded())
        {
            walkSoundTimer += Time.deltaTime;
            walking = true;
            if(walkSoundTimer > 0.4f)
            {
                walkSoundTimer = 0f;
                takeStep = true;
                source.PlayOneShot(walkSound);
            }
        }

        if (walking)
        {
            if (rend.sprite == normalSprite)
            {
                rend.sprite = walkSprite1;
            }
            if (takeStep)
            {
                if (rend.sprite != walkSprite1)
                {
                    rend.sprite = walkSprite1;
                }
                else
                {
                    rend.sprite = walkSprite2;
                }
                takeStep = false;
            }
        }

        else
        {
            rend.sprite = normalSprite;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void mine()
    {
        mineTimer += Time.deltaTime;

        if (mineTimer > 0.3f)
        {
            mineTimer = 0f;
            source.PlayOneShot(mineSound);
            if (rend.sprite != mineSprite)
            {
                rend.sprite = mineSprite;
            }
            else
            {
                rend.sprite = normalSprite;
            }
        }    
    }

    public void stopMining()
    {
        rend.color = Color.white;
        mineTimer = 0f;
    }
}
