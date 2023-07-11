using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fearArea : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private List<GameObject> fearTiles = new List<GameObject>();
    public float distance = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            fearTiles.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            fearTiles.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        distance = 10f;
        if (fearTiles.Count > 0)
        {
            for (int i = 0; i < fearTiles.Count; i++)
            {
                float tempDistance = Vector3.Distance(fearTiles[i].transform.position, player.transform.position);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                }
            }
        }

        //Debug.Log(distance.ToString());
    }
}
