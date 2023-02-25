using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {


    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform cam;

    private const int WIDTH = 25;
    private const int HEIGHT = 25;
    public GameObject[,] grid = new GameObject[WIDTH, HEIGHT];

    Node current;
    Stack mazeGeneratorStack;


    void Start() {
        CreateGrid();
        StartCoroutine("MazeGenerator");
    }


    /*         

        https://en.wikipedia.org/wiki/Maze_generation_algorithm

        1. Make the initial cell the current cell and mark it as visited
        2. While there are unvisited cells
            1. If the current cell has any neighbours which have not been visited
                1. Choose randomly one of the unvisited neighbours
                2. Push the current cell to the stack
                3. Remove the wall between the current cell and the chosen cell
                4. Make the chosen cell the current cell and mark it as visited
            2. Else if stack is not empty
                1. Pop a cell from the stack
                2. Make it the current cell 
    */
    IEnumerator MazeGenerator() {

        mazeGeneratorStack = new Stack();

        current = grid[0, 0].GetComponent<Node>();
        current.SetVisited(true);
        current.SetStartColor();

        while (UnvisitedNodesLeft()) {

            current.SetVisitedColor();
            current.SetVisited(true);

            List<Node> unvisitedNeighbors = current.GetUnvisitedNeighbors();

            if (unvisitedNeighbors.Count > 0) {
                Node next = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                mazeGeneratorStack.Push((Node)current);

                current.removeWalls(next);

                next.SetVisited(true);
                current = next;
            } else if (mazeGeneratorStack.Count > 0) {
                current = (Node)mazeGeneratorStack.Pop();
                current.SetStackColor();
            }

            yield return new WaitForSeconds(.01f);
        }

        current.SetVisitedColor();
        grid[0, 0].GetComponent<Node>().SetStartColor();
        grid[WIDTH - 1, HEIGHT - 1].GetComponent<Node>().SetEndColor();

    }


    void CreateGrid() {

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


    private bool UnvisitedNodesLeft() {
        for (int x = 0; x < WIDTH; x++) {
            for (int y = 0; y < HEIGHT; y++) {
                if (!grid[x, y].GetComponent<Node>().GetVisited() == true) {
                    return true;
                }
            }
        }
        return false;
    }
}
