﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetaryDiversity.Components
{
    public class Gradient
    {
        // Points in the gradient we are generating
        SortedList<Single, Color> points = new SortedList<Single, Color>();

        // Add a point to the gradient
        public void Add(Single p, Color c)
        {
            points.Add(p, c);
        }

        // Get a color from the gradient
        public Color ColorAt(Single p)
        {
            // Gradient points
            Color a = Color.black;
            Color b = Color.black;
            Single ap = Single.NaN;
            Single bp = Single.NaN;

            // Find the points along the gradient
            IEnumerator<KeyValuePair<Single, Color>> enumerator = points.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<Single, Color> point = enumerator.Current;
                if (point.Key >= p)
                {
                    bp = point.Key;
                    b = point.Value;

                    // If we never found a leading color
                    if (Single.IsNaN(ap))
                        return b;

                    // break out
                    break;
                }

                // Otherwise cache these colors
                ap = point.Key;
                a = point.Value;
            }

            // If we never found a tail color
            if (Single.IsNaN(bp))
                return a;

            // Calculate the color
            Single k = (p - ap) / (bp - ap);
            return Color.Lerp(a, b, k);
        }
    }
}


