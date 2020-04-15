using UnityEngine;

public class CloneDispatcher : MonoBehaviour
{
    [SerializeField]
    private Transform _hero;
    [SerializeField]
    private Transform _root;

    public void Init(FoldDispatcher foldDispatcher)
    {
        int counter = 0;
        foreach (var foldController in foldDispatcher.foldControllers)
        {
            var cloneTransform = Instantiate(_hero, _root);
            var cloneGameObject = cloneTransform.gameObject;
            var clone = cloneGameObject.AddComponent<Clone>();
            clone.Init(foldController.paperTransform, _hero);
#if UNITY_EDITOR
            clone.name = $"Clone_{counter++}";
#endif
        }
    }
}
