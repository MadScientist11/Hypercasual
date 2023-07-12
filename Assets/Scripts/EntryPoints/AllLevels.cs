using System.Collections;
using System.Collections.Generic;
using Hypercasual.Services;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/AllLevels", fileName = "AllLevels")]
public class AllLevels : ScriptableObject, IEnumerable<Level>, IData
{
    [SerializeField] private Level[] _levelList;
    public Level this[int index] => _levelList[index];

    public IEnumerator<Level> GetEnumerator()
    {
        return ((IEnumerable<Level>)_levelList).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}