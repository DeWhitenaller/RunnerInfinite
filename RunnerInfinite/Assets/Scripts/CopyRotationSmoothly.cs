using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CopyRotationSmoothly : MonoBehaviour
{
    public GameObject objectToCopyFrom;
    public GameObject root;
    public float runSmoothness;
    public float slideSmoothness;
    public float slideDownTime;
    public float slideTime;
    public float slideUpTime;
    public float slideRotationTime;
    public float originalY;
    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunningAnimation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, objectToCopyFrom.transform.rotation, Time.deltaTime * runSmoothness);
        //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, originalY, transform.position.z), Time.deltaTime * slideSmoothness);
    }

    public void SlidingAnimation()
    {
        Vector3 thisY = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 otherY = new Vector3(transform.position.x, root.transform.position.y + originalY, transform.position.z);
        transform.position = Vector3.Lerp(thisY, otherY, Time.deltaTime * slideSmoothness);

        //transform.DOLocalMoveY(0.3f, slideDownTime).OnComplete(() =>
        //{
        //    StartCoroutine(OnSlideDown());
        //}).SetEase(Ease.InOutBack);

        //transform.DOLocalRotate(new Vector3(15, 0, 0), slideRotationTime).OnComplete(() => { StartCoroutine(OnRotateDown()); });
    }

    //IEnumerator OnSlideDown()
    //{
    //    yield return new WaitForSeconds(slideTime);

    //    transform.DOLocalMoveY(originalY, slideUpTime);
    //}
    IEnumerator OnRotateDown()
    {
        yield return new WaitForSeconds(slideTime);

        transform.DOLocalRotate(new Vector3(0, 0, 0), slideRotationTime);
    }
}
