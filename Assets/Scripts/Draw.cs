using System.Collections;
using UnityEngine;

public class Draw : MonoBehaviour
{
    private Coroutine _drawing;
    
    private void Update()
    {
        if (!PlayManager.Instance.Active || PlayManager.Instance.Paused) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            StartLine();
        }

        if (Input.GetMouseButtonUp(0))
        {
            FinishLine();
        }
    }

    private void StartLine()
    {
        if (_drawing != null) StopCoroutine(_drawing);

        _drawing = StartCoroutine(DrawLine());
    }
    
    public void FinishLine()
    {
        if (_drawing == null) return;
        
        StopCoroutine(_drawing);
    }

    private IEnumerator DrawLine()
    {
        var newGameObject = Instantiate(Resources.Load("Line") as GameObject, new Vector3(0, 0, 0),
            Quaternion.identity, transform);
        var line = newGameObject.GetComponent<LineRenderer>();

        newGameObject.layer = 7;
        line.positionCount = 0;

        while (true)
        {
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, position);
            
            yield return null;
        }
    }
}
