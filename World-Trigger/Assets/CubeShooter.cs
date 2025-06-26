using UnityEngine;

public class CubeShooter : MonoBehaviour
{
    public Transform cubeParent; // Cube.000~063の親オブジェクト
    public float shootInterval = 1f; // 飛ばす間隔
    public float shootForce = 500f; // 飛ばす力

    private int currentIndex = 0;
    private float shootTimer = 0f;
    private Transform[] cubes;

    void Start()
    {
        // 子オブジェクト（Cube.000~）を取得し、名前順に並び替え
        cubes = new Transform[cubeParent.childCount];
        for (int i = 0; i < cubeParent.childCount; i++)
        {
            cubes[i] = cubeParent.GetChild(i);
        }

        System.Array.Sort(cubes, (a, b) => string.Compare(a.name, b.name));
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // 左クリック長押し中
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >= shootInterval && currentIndex < cubes.Length)
            {
                ShootCube(cubes[currentIndex]);
                currentIndex++;
                shootTimer = 0f;
            }
        }
        else
        {
            shootTimer = 0f; // 離したらタイマーリセット
        }
    }

    void ShootCube(Transform cube)
    {
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = cube.gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = false;
        rb.AddForce(transform.forward * shootForce);
    }
}
