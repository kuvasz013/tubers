using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class KnifeManager : MonoBehaviour
{
    [SerializeField] private float secondsPerKnife;
    [SerializeField] private float firstKnifeDelay;
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private float knifeHeight;
    [SerializeField] private float knifeTarget;
    [SerializeField] private float knifeStrikeSeconds;
    [SerializeField] private int keepKnifes;
    [SerializeField] private Transform knifeParent;
    [SerializeField] private GameObject killPlane;

    private Coroutine _knifeCoroutine;
    private readonly IList<GameObject> _knives = new List<GameObject>();
    private bool _knifeMirrored = false;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        StartKnives();
    }

    public void StartKnives()
    {
        _knifeCoroutine = StartCoroutine(KnifeRoutine());
    }

    public void StopKnives()
    {
        StopCoroutine(_knifeCoroutine);
        _knifeCoroutine = null;
    }

    private IEnumerator KnifeRoutine()
    {
        killPlane?.SetActive(false);
        yield return new WaitForSeconds(firstKnifeDelay);
        killPlane?.SetActive(true);

        while (true)
        {
            var knife = Instantiate(knifePrefab, knifeParent);
            _knives.Add(knife);

            if (_knifeMirrored)
            {
                knife.transform.SetPositionAndRotation(killPlane.transform.position + new Vector3(0, knifeHeight, -9), Quaternion.Euler(0, 90, 0));
            } else
            {
                knife.transform.SetPositionAndRotation(killPlane.transform.position + new Vector3(0, knifeHeight, 9), Quaternion.Euler(0, -90, 0));
            }

            _knifeMirrored = !_knifeMirrored;

            source.Play();
            knife.transform.DOMoveY(knifeTarget, knifeStrikeSeconds).OnComplete(() =>
            {
                knife.transform.DOMoveY(knifeHeight, knifeStrikeSeconds);
                if (_knives.Count <= keepKnifes) return;
                var knifeToRemove = _knives[0];
                _knives.RemoveAt(0);
                Destroy(knifeToRemove);
            });
            yield return new WaitForSeconds(secondsPerKnife);
        }
    }
}
