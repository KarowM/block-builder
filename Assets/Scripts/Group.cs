using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour {

    float lastFall = 0;

    void Start() {
        if (!isValidGridPos()) {
            Destroy(gameObject);
        }    
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { // can be improved
            transform.position += new Vector3(-1, 0, 0);

            if (isValidGridPos()) {
                updateGrid();
            } else {
                transform.position += new Vector3(1, 0, 0);
            }
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) { // can be improved
            transform.position += new Vector3(1, 0, 0);

            if (isValidGridPos()) {
                updateGrid();
            } else {
                transform.position += new Vector3(-1, 0, 0);
            }
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) { // can be improved
            transform.Rotate(0, 0, -90);

            if (isValidGridPos()) {
                updateGrid();
            } else {
                transform.Rotate(0, 0, 90);
            }
        } else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 1) {
            Debug.Log(Time.time);
            Debug.Log(lastFall);
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (isValidGridPos()) {
                // It's valid. Update grid.
                updateGrid();
            } else {
                // It's not valid. revert.
                transform.position += new Vector3(0, 1, 0);

                // Clear filled horizontal lines
                PlayGrid.deleteFullRows();

                // Spawn next Group
                FindObjectOfType<BlockSpawner>().spawnNext();

                // Disable script
                enabled = false;
            }
            lastFall = Time.time;
        }
    }

    bool isValidGridPos() {
        foreach (Transform child in transform) {
            Vector2 v = PlayGrid.roundVec2(child.position);

            // Not inside Border?
            if (!PlayGrid.insideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (PlayGrid.grid[(int)v.x, (int)v.y] != null &&
                PlayGrid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    void updateGrid() {
        // Remove old children from grid
        for (int y = 0; y < PlayGrid.h; ++y)
            for (int x = 0; x < PlayGrid.w; ++x)
                if (PlayGrid.grid[x, y] != null)
                    if (PlayGrid.grid[x, y].parent == transform)
                        PlayGrid.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform) {
            Vector2 v = PlayGrid.roundVec2(child.position);
            PlayGrid.grid[(int)v.x, (int)v.y] = child;
        }
    }
}
