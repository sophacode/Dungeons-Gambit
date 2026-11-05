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
    public bool needToReactivate;

    private readonly int[] xBounds = new int[] {14, 14, -50, 78, 14};
    private readonly int[] yBounds = new int[] {14, 78, 78, 78, 142};

    public GameObject doors;
    public GameObject specialdoor;
    public GameObject pathway;
    public GameObject pathwayWalls;

    public GameObject infoText;

    public TutorialManagerScript king;
    public bool firstEnemyKilled;
    public bool secondWave;

    public GameObject Timer;

    public void OnPause() {
        if (active) {
            active = false;
            needToReactivate = true;
        }
        Time.timeScale = 0f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().shooting = false;
    }

    public void OnPlay() {
        if (needToReactivate) {
            active = true;
            needToReactivate = false;
        }
        Time.timeScale = 1f;
    }

    private void Awake()
    {
        wave = 0;
        enemiesAlive = 0;
        active = true;
        needToReactivate = false;

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

        king = GameObject.FindGameObjectWithTag("Tutorial").GetComponent<TutorialManagerScript>();
        firstEnemyKilled = false;
        secondWave = false;

        if (GlobalInfoScript.fromBoss) {
            firstEnemyKilled = true;
            secondWave = true;
            roomCounter = 6;
            roomsCompleted = 5;
            active = false;
            pathway.SetActive(true);
            pathwayWalls.SetActive(true);
            doors.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().FromBoss();
        }
        else if (GlobalInfoScript.speedrun) {
            firstEnemyKilled = true;
            secondWave = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Speedrun();
            Timer.SetActive(true);
        }
        else {
            Destroy(Timer);
        }
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
        if (!GlobalInfoScript.speedrun) {
            if (roomCounter == 2) {
                king.SecondRoomComplete();
            }
        }
        roomsCompleted += 1;
        Debug.Log("Complete");
        doors.SetActive(false);
        pathway.SetActive(true);
        pathwayWalls.SetActive(true);
        if (roomsCompleted == 4) {
            specialdoor.SetActive(false);
            king.Click();
            StartCoroutine(FlashText("Click!\n(A door is unlocked somewhere)"));
        }
        else {
            StartCoroutine(FlashText("Room Complete!"));
        }
    }

    public void EnterRoom(int room) {
        if (!GlobalInfoScript.speedrun) {
            if (room == 2) {
                king.EnterSecondRoom();
            }
            else if (room == 3) {
                king.EnterThirdRoom();
            }
            else if (room == 4) {
                king.EnterFourthRoom();
            }
        }
        
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
                if (wave != 0) {
                    if (!firstEnemyKilled) {
                        firstEnemyKilled = true;
                        if (roomCounter == 1) {
                            king.FirstEnemyKilled();
                        }
                        else {
                            king.FirstBishopKilled();
                        }
                    }
                    else if (!secondWave) {
                        secondWave = true;
                        king.SecondWaveFinished();
                    }
                    Debug.Log("Wave Clear");
                    StartCoroutine(FlashText("Wave Clear!"));
                }
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
