using UnityEngine;

public class SpawnAndAnimate : MonoBehaviour
{
    public GameObject prefab;  // 生成するPrefab
    public Transform hand;     // B-hand.R の Transform

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnPrefab();
        }
    }

    void SpawnPrefab()
    {
        if (prefab == null || hand == null) return;

        // B-hand.R の -2 の位置に生成
        Vector3 spawnPosition = hand.position + hand.forward * -2;
        GameObject spawnedObject = Instantiate(prefab, spawnPosition, hand.rotation);

        // アニメーションを再生
        Animator animator = spawnedObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("StartAnimation"); // "StartAnimation" はアニメーションの名前
        }
    }
}
