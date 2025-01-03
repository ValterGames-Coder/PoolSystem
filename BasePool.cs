using System;
using System.Collections.Generic;
using UnityEngine;

public class BasePool<T>
{
    private readonly Func<T> _preloadFunc;
    private readonly Action<T> _getAction;
    private readonly Action<T> _returnAction;

    private Queue<T> _pool = new();
    private List<T> _active = new();

    public BasePool(Func<T> preloadFunc, Action<T> getAction, Action<T> returnAction, int preloadCount)
    {
        _preloadFunc = preloadFunc;
        _getAction = getAction;
        _returnAction = returnAction;

        if (preloadFunc == null)
        {
            Debug.LogError("Preload func is none");
            return;
        }

        for (int i = 0; i < preloadCount; i++)
            Return(preloadFunc());
    }

    public T Get()
    {
        T item = _pool.Count > 0 ? _pool.Dequeue() : _preloadFunc();
        _getAction(item);
        _active.Add(item);

        return item;
    }

    public void Return(T item)
    {
        _returnAction(item);
        _pool.Enqueue(item);
        _active.Remove(item);
    }

    public void RemoveAll()
    {
        while (_active.Count > 0)
            Return(_active[0]);
    }
}