using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject enemyPawnPrefab;
    public GameObject enemyBishopPrefab;
    public GameObject enemyKnightPrefab;
    public GameObject enemyRookPrefab;
    public GameObject queenBoss;
    private int wave;
    public int enemiesAlive;
    private int[][][] rooms;
    public int roomCounter;
    private int roomsCompleted;
    public bool active;

    private readonly int[] xBounds = new int[] {14, 14, -50, 78, 14};
    private readonly int[] yBounds = new int[] {14, 78, 78, 78, 132};

    public GameObject doors;
    public GameObject specialdoor;
    public GameObject pathway;
    public GameObject pathwayWalls;

    public GameObject infoText;

    public void OnPause() {
        active = false;
        Time.timeScale = 0f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().shooting = false;
        /*
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet")) {
            if (bullet.TryGetComponent<bulletScript>(out bulletScript b)) { b.PauseBullet(); }
            if (bullet.TryGetComponent<EnemyBishopBulletScript>(out EnemyBishopBulletScript bb)) { bb.PauseBullet(); }
            if (bullet.TryGetComponent<EnemyRookBulletScript>(out EnemyRookBulletScript bbb)) { bbb.PauseBullet(); }
        }
        */
    }

    public void OnPlay() {
        active = true;
        Time.timeScale = 1f;
        /*
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet")) {
            if (bullet.TryGetComponent<bulletScript>(out bulletScript b)) { b.ResetSpeed(); }
            if (bullet.TryGetComponent<EnemyBishopBulletScript>(out EnemyBishopBulletScript bb)) { bb.ResetSpeed(); }
            if (bullet.TryGetComponent<EnemyRookBulletScript>(out EnemyRookBulletScript bbb)) { bbb.ResetSpeed(); }
        }
        */
    }

    private void Awake()
    {
        wave = 0;
        enemiesAlive = 0;
        active = true;

        roomCounter = 1;
        roomsCompleted = 0;

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
                SpawnRandom(enemyPawnPrefab).GetComponent<EnemyScript>().energyValue = 1;
            }
            else if (i == 2) {
                SpawnRandom(enemyBishopPrefab).GetComponent<EnemyBishopScript>().energyValue = 2;
            }
            else if (i == 3) {
                SpawnRandom(enemyKnightPrefab).GetComponent<EnemyKnightScript>().energyValue = 2;
            }
            else if (i == 4) {
                SpawnRandom(enemyRookPrefab).GetComponent<EnemyRookScript>().energyValue = 3;
            }
            else if (i == 5) {
                Instantiate(queenBoss, new Vector3(0, 200, 0), Quaternion.identity);
            }
        }
    }

    private GameObject SpawnRandom(GameObject enemyType) {
        int xb = xBounds[roomCounter-1];
        int yb = yBounds[roomCounter-1];
        int randomY = Random.Range(yb-28, yb);
        int randomX = Random.Range(0, 2);
        int x = randomX == 0 ? xb : xb-28;

        GameObject enemy = Instantiate(enemyType, new Vector3(x, randomY, 0), Quaternion.identity);
        return enemy;
    }

    private void RoomComplete() {
        roomsCompleted += 1;
        Debug.Log("Complete");
        doors.SetActive(false);
        pathway.SetActive(true);
        pathwayWalls.SetActive(true);
        if (roomsCompleted >= 4) {
            specialdoor.SetActive(false);
            StartCoroutine(FlashText("Click!\n(A door is unlocked somewhere)"));
        }
        else {
            StartCoroutine(FlashText("Room Complete!"));
        }
    }

    public void EnterRoom(int room) {
        roomCounter = room;
        doors.SetActive(true);
        pathway.SetActive(false);
        pathwayWalls.SetActive(false);
        active = true;
        wave = 0;
    }

    private void Update()
    {
        if (active && enemiesAlive <= 0) {
            if (wave < 3) {
                Debug.Log("Wave Clear");
                StartCoroutine(FlashText("Wave Clear!"));
                wave ++;
                SpawnEnemy();
            }
            else {
                wave = 0;
                active = false;
                RoomComplete();
            }
        }
    }

    private IEnumerator FlashText(string text) {
        infoText.GetComponent<TextMeshProUGUI>().text = text;
        infoText.SetActive(true);
        yield return new WaitForSeconds(1);
        infoText.SetActive(false);
    }

}
