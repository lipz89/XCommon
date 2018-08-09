using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace XCommon.Serializer
{
    /// <summary>
    /// 二进制序列化
    /// </summary>
    class BinarySerializer : BaseSerializer
    {
        /// <inheritdoc />
        public override string FileExtension { get; } = ".db";

        /// <inheritdoc />
        protected override T InnerDeserialize<T>(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                IFormatter serializer = new BinaryFormatter();
                var obj = serializer.Deserialize(stream);
                return (T)obj;
            }
        }

        /// <inheritdoc />
        protected override bool InnerSerialize<T>(T data, string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
                return true;
            }
        }
    }
}