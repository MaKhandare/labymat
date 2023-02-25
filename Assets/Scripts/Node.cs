using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField] private Color baseColor, visitedColor, stackColor, startColor, endColor;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool visited;
    public List<Node> neighbors;

    public GameObject wallTop;
    public GameObject wallRight;
    public GameObject wallBottom;
    public GameObject wallLeft;

    public void SetStartColor() {
        spriteRenderer.color = startColor;
    }

    public void SetEndColor() {
        spriteRenderer.color = endColor;
    }

    public void SetVisitedColor() {
        spriteRenderer.color = visitedColor;
    }

    public void SetStackColor() {
        spriteRenderer.color = stackColor;
    }

    public void SetVisited(bool x) {
        visited = x;
    }

    public bool GetVisited() {
        return visited;
    }

    public void removeWalls(Node other) {
        int x = (int)(this.transform.position.x - other.transform.position.x);
        int y = (int)(this.transform.position.y - other.transform.position.y);

        if (x == 1) {
            this.wallLeft.SetActive(false);
            other.wallRight.SetActive(false);
        } else if (x == -1) {
            this.wallRight.SetActive(false);
            other.wallLeft.SetActive(false);
        }


        if (y == 1) {
            this.wallBottom.SetActive(false);
            other.wallTop.SetActive(false);
        } else if (y == -1) {
            this.wallTop.SetActive(false);
            other.wallBottom.SetActive(false);
        }
    }

    public void GetNeighbors(GameObject[,] grid, int x, int y) {
        neighbors = new List<Node>();


        // top neighbor
        try {
            neighbors.Add(grid[x, y + 1].GetComponent<Node>());
        } catch {
            // Debug.Log("no top neighbor");
        }

        // right neighbor
        try {
            neighbors.Add(grid[x + 1, y].GetComponent<Node>());
        } catch {
            // Debug.Log("no right neighbor");
        }


        // bottom neighbor
        try {
            neighbors.Add(grid[x, y - 1].GetComponent<Node>());
        } catch {
            // Debug.Log("no bottom neighbor");
        }

        // left neighbor
        try {
            neighbors.Add(grid[x - 1, y].GetComponent<Node>());
        } catch {
            // Debug.Log("no left neighbor");
        }

    }

    public Node GetRandomNeighborNode() {
        if (neighbors.Count > 0) {

            return neighbors[Random.Range(0, neighbors.Count)];
        }

        return null;
    }

    public List<Node> GetUnvisitedNeighbors() {
        List<Node> unvisitedNeighbors = new List<Node>();

        for (int i = 0; i < neighbors.Count; i++) {
            if (!neighbors[i].GetVisited()) {
                unvisitedNeighbors.Add(neighbors[i]);
            }
        }

        return unvisitedNeighbors;
    }



}
