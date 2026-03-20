using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    [Header("Paramètres d'éjection")]
    public GameObject cubePrefab; // Le prefab à générer (Cube avec ShootableCube et un Rigidbody non kinematic)
    public Transform spawnPoint; // Position et direction. Mets-le à l'avant du 'Case'
    public float spawnInterval = 2f; // Ejecter un cube toutes les 2 secondes
    public float ejectForce = 5f; // Puissance pour le pousser
    public int maxCubesToSpawn = 5; // Nombre max (tu veux tirer sur 5)

    private bool isSpawning = false;
    private int spawnedCount = 0;

    public void StartSpawning()
    {
        // On commence si on a pas déjà atteint le max
        if (!isSpawning && spawnedCount < maxCubesToSpawn)
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    private IEnumerator SpawnRoutine()
    {
        while (isSpawning && spawnedCount < maxCubesToSpawn)
        {
            SpawnCube();
            spawnedCount++;
            
            // Si on a spawn les 5 cubes, on s'arrête
            if (spawnedCount >= maxCubesToSpawn)
            {
                StopSpawning();
                break;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCube()
    {
        if (cubePrefab == null || spawnPoint == null)
        {
            Debug.LogError("CubePrefab ou SpawnPoint non configuré dans le CubeSpawner !");
            return;
        }

        // Création du cube sans rotation aléatoire pour ne pas fausser sa trajectoire au départ
        GameObject newCube = Instantiate(cubePrefab, spawnPoint.position, spawnPoint.rotation);
        
        // Applique une couleur très vive aléatoirement (Saturation = 1, Value = 1)
        Renderer rend = newCube.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        }

        // Ajoute la physique (Rigidbody) pour expulser l'objet
        Rigidbody rb = newCube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // On le jette vers la direction bleue (Forward) du spawnPoint
            rb.AddForce(spawnPoint.forward * ejectForce, ForceMode.Impulse);
            
            // Pareil pour lui donner une rotation rigolote pendant le saut
            rb.AddTorque(Random.insideUnitSphere * 10f); 
        }
    }
}
