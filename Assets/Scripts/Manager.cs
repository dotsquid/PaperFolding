using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private FoldDragPointController _foldDragPointController;
    [SerializeField]
    private FoldDispatcher _foldDispatcher;
    [SerializeField]
    private CloneDispatcher _cloneDispatcher;

    private void Awake()
    {
        _foldDispatcher.Init();
        _cloneDispatcher.Init(_foldDispatcher);
        _foldDragPointController.Init();
    }
}
