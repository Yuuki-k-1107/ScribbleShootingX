using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemyのプレハブ
    public ShootingScript shootingScript;
    public float spawnInterval =10f; // 生成の間隔（秒）
    private int level = 0;
    private int oldLevel = 0;
    void Start()
    {
        this.gameObject.AddComponent<LevelManager>();
        shootingScript = GameObject.Find("Triangle").GetComponent<ShootingScript>();
        if (!CPUBehaviour.isVsCPU)
        {
            // 初回の生成を待たずにすぐに生成する
            SpawnNewEnemy();

            // 一定時間おきに生成を繰り返す
            InvokeRepeating("SpawnNewEnemy", spawnInterval, spawnInterval);
        }
    }

    private void Update()
    {
        oldLevel = level;
        level = this.gameObject.GetComponent<LevelManager>().GetLevelFromScore(ScoreManager.score);
        if (level != oldLevel) {
            LevelUP();
        }
    }

    void LevelUP()
    {
        if (CPUBehaviour.isVsCPU) return;
        Debug.Log($"Level Promoted to {level}");
        CancelInvoke("SpawnNewEnemy");
        InvokeRepeating("SpawnNewEnemy", spawnInterval, spawnInterval - level + 1);
        if(shootingScript != null)
        {
            shootingScript.flash++;
        }
    }

    void SpawnNewEnemy()
    {
        // ランダムな位置を生成
        float randomX = Random.Range(-8f, 8f);
        Vector3 spawnPosition = new Vector3(randomX, 7f, 0f);

        // Enemyプレハブを生成
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // 生成されたEnemyに名前を付ける
        newEnemy.name = "Enemy";
        newEnemy.GetComponent<EnemyBehavior>().HP = 80 + level*20;
    }
}
