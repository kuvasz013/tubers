using DG.Tweening;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public void StartSpinner()
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(new Vector3(0, 0, 5), 1f));
        seq.Append(transform.DOLocalRotate(new Vector3(0, 0, -5), 1f));
        seq.SetLoops(-1);
        seq.Play();
    }
}
