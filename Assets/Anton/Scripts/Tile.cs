using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Dirt,
    Coal,
    Empty,
}

public class Tile : MonoBehaviour
{
    public State state;

    [SerializeField] Sprite dirtSprite;
    [SerializeField] Sprite coalSprite;
    [SerializeField] Sprite emptySprite;

    [SerializeField] GameObject crackObject;
    private SpriteRenderer crackRend;
    [SerializeField] Sprite startBreak;
    [SerializeField] Sprite midBreak;
    [SerializeField] Sprite fullBreak;

    private float miningTimer = 0f;
    public GameObject player;
    public TerrainGeneration terrainGen;
    private float miningDistance;
    private playerMovement playerScript;

    private void Start()
    {
        crackRend = crackObject.GetComponent<SpriteRenderer>();
        miningDistance = 1.5f;
        playerScript = player.GetComponent<playerMovement>();
    }

    private void Update()
    {
        SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        if(miningTimer >= 0.75f)
        {
            miningTimer = 0f;

            if(state == State.Coal)
            {
                terrainGen.addScore();
            }

            state = State.Empty;
            crackRend.sprite = null;
            playerScript.stopMining();
        }

        else if (miningTimer >= 0.5f && miningTimer < 0.75f)
        {
            crackRend.sprite = fullBreak;
        }

        else if (miningTimer >= 0.25f && miningTimer < 0.5f)
        {
            crackRend.sprite = midBreak;
        }

        else if (miningTimer > 0f && miningTimer < 0.25f)
        {
            crackRend.sprite = startBreak;
        }

        else
        {
            crackRend.sprite = null;
        }

        switch (state)
        {
            case State.Dirt:
                if(rend.sprite != dirtSprite)
                {
                    rend.sprite = dirtSprite;
                }
                if (!collider.isActiveAndEnabled)
                {
                    collider.enabled = true;
                }
                gameObject.layer = 6;
                break; 
            case State.Coal:
                if (rend.sprite != coalSprite)
                {
                    rend.sprite = coalSprite;
                }
                if (!collider.isActiveAndEnabled)
                {
                    collider.enabled = true;
                }
                gameObject.layer = 6;
                break;
            case State.Empty:
                if (rend.sprite != emptySprite)
                {
                    rend.sprite = emptySprite;
                }
                if (collider.isActiveAndEnabled)
                {
                    collider.enabled = false;
                }
                gameObject.layer = default;
                break;
            default:
                break;
        }
    }

    public void randCoal()
    {
        int randomInt = Random.Range(0, 10);
        if (randomInt == 0) 
        {
            state = State.Coal;
        }
    }

    void OnMouseOver()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= miningDistance)
        {
            if (Input.GetMouseButton(0) && state != State.Empty)
            {
                miningTimer += Time.deltaTime;
                player.GetComponent<playerMovement>().mine();
            }
        }
    }

    private void OnMouseExit()
    {
        if (miningTimer < 0.5f)
        {
            miningTimer = 0f;
            crackRend.sprite = null;
        }

        playerScript.stopMining();
    }
}
