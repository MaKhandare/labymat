# labymat

## A simple maze generating algorithm

![ezgif-1-8290339d49](https://user-images.githubusercontent.com/52472164/223395539-a5579df2-0963-4806-8614-501a08e856a9.gif)

As of right now, only one algorithm is implemented. More might be added if time allows it.

**The code snippets in this README are simplified to only show the essentials. Check out the [source code](https://github.com/MaKhandare/labymat/tree/main/Assets/Scripts) to see the real implementations.**

_____

### Node.cs

A node has 4 walls. (top, right, bottom, left)

```cs
    private GameObject wallTop;
    private GameObject wallRight;
    private GameObject wallBottom;
    private GameObject wallLeft;
```

A node keeps track of it's visited state.

```cs
    private bool visited;
```

A node has a List of nodes, where it's neighbors are stored.

```cs
    private List<Node> neighbors;
```

_____

### Grid.cs

As the name of the class already suggests, Grid.cs is responsible for holding an 2D Array of GameObjects, which have the Node Script attached to them.

```cs
    private GameObject[,] grid = new GameObject[WIDTH, HEIGHT];
```

When creating the grid, a nested for loop is used, and for each iteration a new GameObject is instantiated at the correct position.
This new GameObject then gets inserted into the 2d grid array:

```cs
        for (int x = 0; x < WIDTH; x++) {
            for (int y = 0; y < HEIGHT; y++) {
                GameObject node = Instantiate(nodePrefab, new Vector3(x, y), Quaternion.identity);
                grid[x, y] = node;
            }
        }
```

After it is created, each node in the grid, needs to know of it's neighbors.

```cs
        for (int x = 0; x < WIDTH; x++) {
            for (int y = 0; y < HEIGHT; y++) {
                grid[x, y].InitializeNeighbors();
            }
        }
```

_____

### MazeGenerator.cs

<https://en.wikipedia.org/wiki/Maze_generation_algorithm>

As of right now, only one of many algorithms to generate a maze is implemented:
The Depth-First-Search Algorithm, which works as follows:

1. Make the initial node the current node and mark it as visited
2. While there are unvisited nodes
    1. If the current nodes has any neighbours which have not been visited
        1. Choose randomly one of the unvisited neighbours
        2. Push the current node to the stack
        3. Remove the wall between the current node and the chosen node
        4. Make the chosen node the current node and mark it as visited
    2. Else if stack is not empty
        1. Pop a node from the stack
        2. Make it the current node

```cs
        // start by setting one node to the current node and mark it as visited
        Node current = grid[0, 0];
        current.SetVisited(true);

        while (UnvisitedNodesLeft()) {

            List<Node> unvisitedNeighbors = current.GetUnvisitedNeighbors();

            if (unvisitedNeighbors.Count > 0) {
                // if  the current node has any unvisited neighbors, pick one randomly
                // and push it to the stack
                Node next = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                mazeGeneratorStack.Push(current);

                // remove the walls between the current node and the next one
                current.RemoveWalls(next);

                // set the next node to the current node and mark it as visited
                next.SetVisited(true);
                current = next;

            // if however, there is no unvisited neighbor left,
            // we need to backtrack: set the current node to the popped element of stack
            } else if (mazeGeneratorStack.Count > 0) {
                current = mazeGeneratorStack.Pop();
            }

        }
```
