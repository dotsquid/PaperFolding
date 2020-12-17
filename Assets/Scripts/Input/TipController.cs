using System.Collections;
using UnityEngine;

public class TipController : MonoBehaviour
{
    private static readonly int kRunTriggerHash = Animator.StringToHash("Run");

    [SerializeField]
    private Animator _animator;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        while (Input.touchCount == 0 && !Input.GetMouseButtonDown(0))
            yield return null;
        _animator.SetTrigger(kRunTriggerHash);
    }
}
