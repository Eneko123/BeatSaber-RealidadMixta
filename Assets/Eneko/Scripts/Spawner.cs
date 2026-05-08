using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject projectil; // Prefab del proyectil con ProyectilMR
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
        // Verificar que el prefab tiene el componente correcto
        if (projectil != null)
        {
            ProyectilMR proyectilComponent = projectil.GetComponent<ProyectilMR>();
            if (proyectilComponent == null)
            {
                Debug.LogError(" El prefab de proyectil NO tiene el componente ProyectilMR. Por favor aniadelo.");
            }
            else
            {
                Debug.Log(" Spawner configurado correctamente con ProyectilMR");
            }
        }
        else
        {
            Debug.LogError(" No hay prefab asignado en el Spawner!");
        }

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

                // CAMBIO IMPORTANTE: Usar ProyectilMR en vez de Proyectil
                ProyectilMR proyectilComponent = pool[i].GetComponent<ProyectilMR>();

                if (proyectilComponent != null)
                {
                    proyectilComponent.Launch(destinationOffsetRange, isBomb, speed);
                }
                else
                {
                    Debug.LogError($" El proyectil {pool[i].name} no tiene el componente ProyectilMR!");
                }

                return;
            }
        }

        // Si no hay proyectiles disponibles, crear uno nuevo
        AddProyectil(1);
        ShootProyectil(origin);
    }

    Vector3 OriginPoint()
    {
        Transform cam = Camera.main.transform;

        if (cam == null)
        {
            Debug.LogError(" No se encuentra Main Camera!");
            return Vector3.zero;
        }

        Vector3 spawnPos = cam.position + cam.forward * spawnDistance;
        spawnPos.y = 1.5f;
        return spawnPos;
    }
}
