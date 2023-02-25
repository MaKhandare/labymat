using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator {


    private Stack<Node> mazeGeneratorStack;
    private GameObject[,] grid;
    private float waitDelay;

    public bool isRunning;

    public MazeGenerator(GameObject[,] grid) {
        mazeGeneratorStack = new Stack<Node>();
        this.grid = grid;
        waitDelay = 0.01f;
        isRunning = false;
    }






    /*         
        Iterative DFS with a stack

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
    public IEnumerator IterativeDFS() {

        isRunning = true;

        Node current = grid[0, 0].GetComponent<Node>();
        current.SetVisited(true);
        current.SetStartColor();

        while (UnvisitedNodesLeft()) {

            current.SetVisitedColor();
            current.SetVisited(true);

            List<Node> unvisitedNeighbors = current.GetUnvisitedNeighbors();

            if (unvisitedNeighbors.Count > 0) {
                Node next = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                mazeGeneratorStack.Push((Node)current);

                current.RemoveWalls(next);

                next.SetVisited(true);
                current = next;
            } else if (mazeGeneratorStack.Count > 0) {
                current = (Node)mazeGeneratorStack.Pop();
                current.SetStackColor();
            }

            yield return new WaitForSeconds(waitDelay);
        }

        current.SetVisitedColor();
        grid[0, 0].GetComponent<Node>().SetStartColor();
        grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1].GetComponent<Node>().SetEndColor();

        isRunning = false;
    }

    private bool UnvisitedNodesLeft() {
        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                if (!grid[x, y].GetComponent<Node>().GetVisited() == true) {
                    return true;
                }
            }
        }
        return false;
    }

}
