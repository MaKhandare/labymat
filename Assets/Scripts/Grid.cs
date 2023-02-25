using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {


    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform cam;

    private const int WIDTH = 5;
    private const int HEIGHT = 5;
    private GameObject[,] grid = new GameObject[WIDTH, HEIGHT];


    void Start() {

        CreateGrid();

    }

    void Update() {

    }


    void CreateGrid() {

        for (int x = 0; x < WIDTH; x++) {
            for (int y = 0; y < HEIGHT; y++) {
                GameObject node = Instantiate(nodePrefab, new Vector3(x, y), Quaternion.identity);

                node.name = $"Node: {x} {y}";
                node.transform.SetParent(this.transform);

                grid[x, y] = node;
            }
        }


        // adjust camera position
        cam.transform.position = new Vector3((float)WIDTH / 2 - 0.5f, (float)HEIGHT / 2 - 0.5f, -10);
    }
}
