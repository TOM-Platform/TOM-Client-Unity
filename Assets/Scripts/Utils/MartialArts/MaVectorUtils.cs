using UnityEngine;

namespace MartialArts
{
    public static class MaVectorUtils
    {
        public static MaVector convertToMaVector(MaVector maVector, Vector3 vector)
        {
            maVector.X = vector.x;
            maVector.Y = vector.y;
            maVector.Z = vector.z;
            return maVector;
        }
    }
}
