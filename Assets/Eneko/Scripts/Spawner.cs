using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject projectil; // Solo necesitas un prefab ahora
    public float bombProbability = 0.3f;
    public List<GameObject> pool = new List<GameObject>();

    public float spawnDistance = 50;
    public float destinationOffsetRange = 2;

    // Variables de velocidad configurables
    public float minSpeed = 5f;
    public float maxSpeed = 10f;
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 3f;


    private int poolSize = 10;
    private float cooldown = 0;
    private float nextSpawnTime;

    void Start()
    {
        AddProyectil(poolSize);
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update()
    {
        cooldown += Time.deltaTime;

        if (cooldown >= nextSpawnTime)
        {
            ShootProyectil(OriginPoint());
            cooldown = 0f;
            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    void AddProyectil(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject p = Instantiate(projectil);
            p.SetActive(false);
            pool.Add(p);
        }
    }

    void ShootProyectil(Vector3 origin)
    {
        bool isBomb = Random.value < bombProbability;
        float speed = Random.Range(minSpeed, maxSpeed);

        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeSelf)
            {
                pool[i].transform.position = origin;
                pool[i].SetActive(true);
                pool[i].GetComponent<Proyectil>().Launch(destinationOffsetRange, isBomb, speed);
                return;
            }
        }

        AddProyectil(1);
        ShootProyectil(origin);
    }

    Vector3 OriginPoint()
    {
        Transform cam = Camera.main.transform;
        Vector3 spawnPos = cam.position + cam.forward * spawnDistance;
        spawnPos.y = 1.5f;
        return spawnPos;
    }
}