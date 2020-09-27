using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject[] blocks;

    void Start() {
        spawnNext();    
    }

    public void spawnNext() {
        int i = Random.Range(0, blocks.Length);

        Instantiate(blocks[i], transform.position, Quaternion.identity);
    }
}
