using UnityEngine;

public class GameObjectPool : BasePool<GameObject>
{
    public GameObjectPool(GameObject prefab, int preloadCount)
        : base (() => Preload(prefab), GetAction, ReturnAction, preloadCount)
    {
    }

    public static GameObject Preload(GameObject prefab) => GameObject.Instantiate(prefab);
    public static void GetAction(GameObject prefab) => prefab.SetActive(true);
    public static void ReturnAction(GameObject prefab) => prefab.SetActive(false);
}