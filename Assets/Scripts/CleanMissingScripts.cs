#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class CleanMissingScripts : ScriptableObject
{
    [MenuItem("Outils Magiques/2- Nettoyer les scripts fantômes (Missing)")]
    public static void NettoyerScripts()
    {
        int totalMissing = 0;
        
        // Parcourt absolument tous les objets de la scène
        GameObject[] tousLesObjets = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in tousLesObjets)
        {
            if (obj.scene.name != null) // S'il est dans la scène
            {
                // Nettoie automatiquement les trous (Missing Scripts) de cet objet
                int deledCount = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
                totalMissing += deledCount;
            }
        }
        
        Debug.Log("🧹 Nettoyage terminé ! J'ai effacé " + totalMissing + " composants fantômes liés à Meta qui faisaient planter la console.");
    }
}
#endif
