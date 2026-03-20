#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class VRFixerWindow : ScriptableObject
{
    // Ce bouton va apparaître tout en haut de l'écran dans Unity !
    [MenuItem("Outils Magiques/1-Clic Configurer la VR (Arme & Mains)")]
    public static void AppliquerLaMagie()
    {
        int count = 0;

        // 1. Scanner toute la scène pour trouver les composants de VR ("Interactors")
        var tousLesScripts = Resources.FindObjectsOfTypeAll<MonoBehaviour>();

        foreach (var comp in tousLesScripts)
        {
            // On s'assure qu'ils sont bien dans la scène et non dans les dossiers de base
            if (comp != null && comp.gameObject.scene.name != null)
            {
                SerializedObject objetEdite = new SerializedObject(comp);
                bool modifié = false;

                // Chercher l'option "Select Action Trigger" (Le type de prise en main)
                SerializedProperty selectTriggerProp = objetEdite.FindProperty("m_SelectActionTrigger");
                if (selectTriggerProp != null)
                {
                    // 2 correspond à la valeur "Toggle" (appuyer une fois pour coller)
                    selectTriggerProp.enumValueIndex = 2; 
                    modifié = true;
                }

                // Chercher l'option "Force Grab" (Pour que l'arme bondisse dans la main depuis le sol)
                SerializedProperty forceGrabProp = objetEdite.FindProperty("m_ForceGrab");
                if (forceGrabProp != null)
                {
                    forceGrabProp.boolValue = true;
                    modifié = true;
                }

                // Chercher "Retain Transform Parent" (souvent une source de bugs d'attachement)
                SerializedProperty retainProp = objetEdite.FindProperty("m_RetainTransformParent");
                if (retainProp != null)
                {
                    retainProp.boolValue = false; // Désactiver pour un déplacement normal
                    modifié = true;
                }

                if (modifié)
                {
                    objetEdite.ApplyModifiedProperties();
                    count++;
                }
            }
        }

        // 2. Corriger le Blaster (Il manquait de collision sur ta capture !)
        GameObject blaster = GameObject.Find("blaster") ?? GameObject.Find("Blaster");
        if (blaster != null)
        {
            Collider col = blaster.GetComponent<Collider>();
            if (col == null)
            {
                blaster.AddComponent<BoxCollider>();
                Debug.Log("🛡️ Une boîte de collision a été magiquement ajoutée au pistolet.");
            }
            EditorUtility.SetDirty(blaster);
        }

        Debug.Log("✅ Magie terminée ! J'ai passé " + count + " mains/lasers en mode Toggle (collant) et forcé l'attraction de loin (Force Grab). Tu peux lancer le jeu !");
    }
}
#endif
