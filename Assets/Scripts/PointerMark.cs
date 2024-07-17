using System.Collections;
using UnityEngine;

public class PointerMark : MonoBehaviour
{
    private const float DestroyTime = 1f;
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start() => StartCoroutine(DestroyAfterTime());
    
    private IEnumerator DestroyAfterTime()
    {
        var elapsedTime = 0f;

        while (elapsedTime < DestroyTime)
        {
            spriteRenderer.color = new Color(1f, 0f, 0f, 1f - elapsedTime / DestroyTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
