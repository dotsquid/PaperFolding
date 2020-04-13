using System.Collections.Generic;
using UnityEngine;

public class FoldDispatcher : MonoBehaviour
{
    [SerializeField]
    private Transform _foldControllerRoot;
    [SerializeField]
    private FoldController _foldControllerPrefab;
    [SerializeField, Min(1)]
    private int _count = 4;

    private Stack<FoldController> _foldControllers = new Stack<FoldController>();

    private void Awake()
    {
        for (int i = 0; i < _count; ++i)
        {
            var foldController = Instantiate(_foldControllerPrefab, _foldControllerRoot);
            foldController.Init();
            _foldControllers.Push(foldController);
#if UNITY_EDITOR
            foldController.name = $"{_foldControllerPrefab.name}_{i + 1}";
#endif
        }
    }

    public FoldController Acquire()
    {
        FoldController foldController = null;
        int freeCount = _foldControllers.Count;
        if (freeCount > 0)
        {
            foldController = _foldControllers.Pop();
            foldController.Acquire(_count - freeCount);
        }
        return foldController;
    }

    public void Release(FoldController foldController)
    {
        if (foldController != null)
        {
            foldController.Release();
            _foldControllers.Push(foldController);
        }
    }
}
