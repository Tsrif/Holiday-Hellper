using UnityEngine;

public class ClothFix : MonoBehaviour {

    Cloth cloth;

    void Awake() {
        cloth = GetComponent<Cloth>();
    }

    void OnEnable() {
        // workaround for SetActive not restarting cloth simulation
        cloth.enabled = false;
        cloth.enabled = true;
        // workaround for Unity 2017.3 adding a mesh collider to every single vertex regardless of settings
        foreach (MeshCollider c in GetComponents<MeshCollider>())
            Destroy(c);
    }

    void OnDisable() {
        cloth.enabled = false;
    }
}
