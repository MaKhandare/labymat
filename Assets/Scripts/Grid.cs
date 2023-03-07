using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {


    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform cam;

    private const int WIDTH = 25;
    private const int HEIGHT = 25;
    private GameObject[,] grid = new GameObject[WIDTH, HEIGHT];
    private MazeGenerator mazeGenerator;

    void Start() {

        CreateGrid();
        mazeGenerator = new MazeGenerator(grid);
    }

    public void IterativeDFS() {
        if (!mazeGenerator.isRunning) {
            StartCoroutine(mazeGenerator.IterativeDFS());
        }
    }


    private void CreateGrid() {

        for (int x = 0; x < WIDTH; x++) {
            for (int y = 0; y < HEIGHT; y++) {
                GameObject node = Instantiate(nodePrefab, new Vector3(x, y), Quaternion.identity);
                node.name = $"{x},{y}";
                node.transform.SetParent(this.transform);

                grid[x, y] = node;
            }
        }


        // add neighbors for each node in grid after creating it
        for (int x = 0; x < WIDTH; x++) {
            for (int y = 0; y < HEIGHT; y++) {
                grid[x, y].GetComponent<Node>().GetNeighbors(grid, x, y);
            }
        }


        // adjust camera position
        cam.transform.position = new Vector3((float)WIDTH / 2 - 0.5f, (float)HEIGHT / 2 - 0.5f, -10);
    }


    public void ResetGrid() {

        if (!mazeGenerator.isRunning) {

            for (int x = 0; x < WIDTH; x++) {
                for (int y = 0; y < HEIGHT; y++) {
                    grid[x, y].GetComponent<Node>().SetVisited(false);
                    grid[x, y].GetComponent<Node>().SetBaseColor();
                    grid[x, y].GetComponent<Node>().AddWalls();

                }
            }

            mazeGenerator.goalX = Random.Range(0, grid.GetLength(0) - 1);
            mazeGenerator.goalY = Random.Range(0, grid.GetLength(1) - 1);
        }

    }
}
