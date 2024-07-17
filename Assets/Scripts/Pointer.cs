using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private Transform pointerMarkParent;
    [SerializeField] private GameObject pointerMarkPrefab;

    private void Update()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = -1f;
        transform.position = position;

        if (Input.GetMouseButtonUp(0) && !PlayManager.Instance.Paused)
        {
            Instantiate(pointerMarkPrefab, transform.position, Quaternion.identity, pointerMarkParent);
        }
    }
}
