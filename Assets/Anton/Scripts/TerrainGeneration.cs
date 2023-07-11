using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] private float worldWidth;
    [SerializeField] private float worldHeight;
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject player;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject blurVolume;

    private GameObject[,] grid;
    private int frame;
    private GameObject actualPlayer;
    private int score;
    private GameObject actualVolume;

    private void Start()
    {
        
        grid = new GameObject[(int)worldWidth, (int)worldHeight];
        for (int i = 0; i < worldWidth; i++)
        {
            for (int j = 0; j < worldHeight; j++)
            {
                grid[i, j] = Instantiate(tile);
                float x = (float)i / 3.125f;
                float y = (float)j / 3.125f;
                grid[i, j].transform.position = new Vector3(x - (worldWidth / 2) / 3.125f, y - (worldHeight / 2f) / 3.125f, 0);
                grid[i, j].GetComponent<Tile>().randCoal();
            }
        }

        int worldMidX = (int)worldHeight / 2;
        int worldMidY = (int)worldWidth / 2;

        score = 0;

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                grid[worldMidX + i - 3, worldMidY + j - 3].GetComponent<Tile>().state = State.Empty;
            }
        }

        grid[worldMidX + 3, worldMidY + 3].GetComponent<Tile>().state = State.Dirt;
        grid[worldMidX + 3, worldMidY - 3].GetComponent<Tile>().state = State.Dirt;
        grid[worldMidX - 3, worldMidY + 3].GetComponent<Tile>().state = State.Dirt;
        grid[worldMidX - 3, worldMidY - 3].GetComponent<Tile>().state = State.Dirt;

        actualPlayer = Instantiate(player);
        actualVolume = Instantiate(blurVolume);
        actualVolume.GetComponent<BlurScript>().player = actualPlayer;
        actualPlayer.transform.position = new Vector3(0,0,0);
        frame = 0;

        for (int i = 0; i < worldWidth; i++)
        {
            for (int j = 0; j < worldHeight; j++)
            {
                grid[i, j].GetComponent<Tile>().player = actualPlayer;
                grid[i, j].GetComponent<Tile>().terrainGen = this;
            }
        }
    }
    private void FixedUpdate()
    {
        frame++;
        if (frame >= 4)
        {
            actualPlayer.GetComponent<CapsuleCollider2D>().enabled = true;
            actualPlayer.GetComponent<Rigidbody2D>().gravityScale = 4f;
        }
    }

    public void addScore()
    {
        score++;
    }

    private void Update()
    {
        scoreText.text = score.ToString();

        if (score >= 20)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
