using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PythonRouge.game
{
    public class MonsterUpdateEventArgs : EventArgs
    {
        public Vector2 playerPos;
        public Engine engine;
    }
    public class PlayerMoveEventArgs : EventArgs
    {
        public Vector2 playerPos;
        public Engine engine;
    }
    public static class Generics
    {
        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}