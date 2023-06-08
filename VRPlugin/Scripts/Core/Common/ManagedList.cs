using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagedList<T> : IEnumerable
{
    public event Action OnEmptied;
    public event Action OnFirstElementAdded;

    public bool IsEmpty => _list.IsNullOrEmpty();

    public List<T> List => _list;

    private List<T> _list;

    public ManagedList()
    {
        _list = new List<T>();
    }

    public int Count => _list.Count;
    
    public void Add(T target)
    {
        bool isFirst = _list.Count == 0;
        if (!_list.Contains(target))
        {
            _list.Add(target);
        }

        if (isFirst)
        {
            OnFirstElementAdded?.Invoke();
        }
    }
    
    public void Remove (T target)
    {
        if (_list.Contains(target))
        {
            _list.Remove(target);
        }

        if (IsEmpty)
        {
            OnEmptied?.Invoke();
        }
    }

    public IEnumerator GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public T Get(int index)
    {
        return _list[index];
    }
    public T[] GetArray(ref int count)
    {
        count = Mathf.Min(count, _list.Count);
        if (count != 0)
        {
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = _list[i];
            }
            return result;
        }
        else
        {
            return Array.Empty<T>();
        }
    }
}