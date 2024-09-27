using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOM.Common.Utils
{
    public class MovingAverageFilter
    {
        private Queue<Vector3> buffer;
        private Vector3 sum = Vector3.zero;
        private int bufferSize;

        public MovingAverageFilter(int bufferSize)
        {
            this.bufferSize = bufferSize;
            this.buffer = new Queue<Vector3>(bufferSize);
        }

        public Vector3 Process(Vector3 newPoint)
        {
            if (buffer.Count >= bufferSize)
            {
                Vector3 oldPoint = buffer.Dequeue();
                sum -= oldPoint;
            }

            buffer.Enqueue(newPoint);
            sum += newPoint;
            return sum / buffer.Count;
        }
        public void Clear()
        {
            buffer.Clear();
            sum = Vector3.zero;
        }
    }
}
