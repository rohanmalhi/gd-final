using System;
using System.Linq;
using UnityEngine;

static class Utils
{
    public static void Normalize<T>(this T[,] array) where T : IComparable<T>, IConvertible
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        T[] flattenedArray = array.Cast<T>().ToArray();

        T minValue = flattenedArray.Min();
        T maxValue = flattenedArray.Max();

        T range = (dynamic)maxValue - (dynamic)minValue;
        if (range.Equals(0)) range = (dynamic)1;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = (T)((dynamic)(array[i, j] - (dynamic)minValue) / (dynamic)range);
            }
        }
    }
}