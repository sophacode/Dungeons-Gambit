using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject enemyPawnPrefab;
    private int wave;
    public int enemiesAlive;
    private int[][][] rooms;
    private int roomCounter;
    private bool active;

    //private int xBounds = 7;
    private int yBounds = 7;
    

    private void Awake()
    {
        wave = 0;
        enemiesAlive = 0;
        active = true;

        roomCounter = 1;

        //rooms -- 6 rooms,3 waves,x enemies
        //enemies -- 0 - none, 1 - pawn, 2 - bishop, 3 - knight, 4 - rook, 5 - queen
        rooms = new int[][][] {
            new int[][] {
                new int[] {1},
                new int[] {1,1,1},
                new int[] {1,1,1,1,1}
            },
            new int[][] {
                new int[] {2},
                new int[] {2, 1, 1},
                new int[] {2, 2, 1, 1, 1}
            },
            new int[][] {
                new int[] {3},
                new int[] {3, 3, 3},
                new int[] {3, 3, 3, 2, 1}
            },
            new int[][] {
                new int[] {4},
                new int[] {4, 4, 1, 1, 1},
                new int[] {4, 4, 4, 2, 2}
            },
            new int[][] {
                new int[] {4, 2, 2, 1, 1},
                new int[] {3, 3, 3, 2, 2},
                new int[] {4, 4, 3, 3, 2}
            },
            new int[][] {
                new int[] {5}
            }
        };
    }


    private void SpawnEnemy() {
        int[] enemiesToSpawn = rooms[roomCounter-1][wave-1];
        enemiesAlive = enemiesToSpawn.Length;
        foreach (int i in enemiesToSpawn) {
            if (i == 1) {
                SpawnRandom(enemyPawnPrefab, i);
            }
        }
    }

    private void SpawnRandom(GameObject enemyType, int value) {
        int randomY = Random.Range(-yBounds, yBounds);
        int randomX = Random.Range(0, 1);
        int x = randomX == 0 ? 7 : -7;

        GameObject enemy = Instantiate(enemyType, new Vector3(x, randomY, 0), Quaternion.identity);
        enemy.GetComponent<EnemyScript>().energyValue = value;
        
    }

    private void RoomComplete() {
        Debug.Log("Complete");
    }

    private void Update()
    {
        if (active && enemiesAlive == 0) {
            if (wave < 3) {
                Debug.Log("Wave Clear");
                wave ++;
                SpawnEnemy();
            }
            else {
                active = false;
                RoomComplete();
            }
        }
    }

}
