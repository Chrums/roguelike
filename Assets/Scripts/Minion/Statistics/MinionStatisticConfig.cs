using System;
using UnityEngine;

namespace Fizz6.Roguelike.Minion.Statistics
{
    [Serializable]
    public class MinionStatisticConfig
    {
        [SerializeField, Range(2, 8)] 
        private int minimum;

        [SerializeField, Range(64, 512)]
        private int maximum;
    }
}
