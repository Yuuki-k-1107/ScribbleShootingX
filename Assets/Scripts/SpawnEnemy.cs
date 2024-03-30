using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab; // Enemy�̃v���n�u
    public ShootingScript shootingScript;
    public float spawnInterval =10f; // �����̊Ԋu�i�b�j
    private int level = 0;
    private int oldLevel = 0;
    void Start()
    {
        this.gameObject.AddComponent<LevelManager>();
        shootingScript = GameObject.Find("Triangle").GetComponent<ShootingScript>();
        if (!CPUBehaviour.isVsCPU)
        {
            // ����̐�����҂����ɂ����ɐ�������
            SpawnNewEnemy();

            // ��莞�Ԃ����ɐ������J��Ԃ�
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
        // �����_���Ȉʒu�𐶐�
        float randomX = Random.Range(-8f, 8f);
        Vector3 spawnPosition = new Vector3(randomX, 7f, 0f);

        // Enemy�v���n�u�𐶐�
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // �������ꂽEnemy�ɖ��O��t����
        newEnemy.name = "Enemy";
        newEnemy.GetComponent<EnemyBehavior>().HP = 80 + level*20;
    }
}
