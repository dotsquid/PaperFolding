using UnityEngine;

public class CloneDispatcher : MonoBehaviour
{
    [SerializeField]
    private Transform _hero;

    public void Init(FoldDispatcher foldDispatcher)
    {
        int counter = 0;
        foreach (var foldController in foldDispatcher.foldControllers)
        {
            var cloneTransform = Instantiate(_hero, foldController.transform);
            var cloneGameObject = cloneTransform.gameObject;
            var clone = cloneGameObject.AddComponent<Clone>();
            clone.Init(foldController.paperTransform, _hero);
#if UNITY_EDITOR
            clone.name = $"Clone_{++counter}";
#endif
        }
    }
}
