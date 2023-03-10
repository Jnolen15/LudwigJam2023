using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float camMoveSpeed;
    [SerializeField] private Transform snackPos;
    [SerializeField] private Transform mainPos;
    private bool moving;
    private bool atMain = true;
    private bool atSnack;

    public void MoveToSnack()
    {
        if (!moving && !atSnack)
        {
            atSnack = true;
            atMain = false;
            StartCoroutine(MoveCamera(snackPos));
        }
    }

    public void MoveToMain()
    {
        if (!moving && !atMain)
        {
            atMain = true;
            atSnack = false;
            StartCoroutine(MoveCamera(mainPos));
        }
    }

    public IEnumerator MoveCamera(Transform movePos)
    {
        moving = true;
        float time = 0;

        Vector3 startPos = Camera.main.transform.position;
        Quaternion startRot = Camera.main.transform.rotation;
        float startFOV = Camera.main.fieldOfView;

        while (time < camMoveSpeed)
        {
            float t = time / camMoveSpeed;
            t = t * t * (3f - 2f * t);

            Camera.main.transform.position = Vector3.Lerp(startPos, movePos.position, t);
            Camera.main.transform.rotation = Quaternion.Lerp(startRot, movePos.rotation, t);
            //Camera.main.fieldOfView = Mathf.Lerp(startFOV, newFOV, t);

            time += Time.deltaTime;
            yield return null;
        }

        moving = false;
    }
}
