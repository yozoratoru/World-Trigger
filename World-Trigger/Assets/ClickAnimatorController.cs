using UnityEngine;

public class ClickAnimatorController : MonoBehaviour
{
    Animator animator;
    bool wasHolding = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isHolding = Input.GetMouseButton(0);

        animator.SetBool("isHolding", isHolding);

        // 離した瞬間に Trigger を送る
        if (wasHolding && !isHolding)
        {
            animator.SetTrigger("released");
        }

        wasHolding = isHolding;
    }
}
