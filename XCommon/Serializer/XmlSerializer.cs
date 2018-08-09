using System.IO;

namespace XCommon.Serializer
{
    /// <summary>
    /// XML–Ú¡–ªØ¿‡
    /// </summary>
    class XmlSerializer : BaseSerializer
    {
        /// <inheritdoc />
        protected sealed override T InnerDeserialize<T>(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                var obj = serializer.Deserialize(stream);
                return (T)obj;
            }
        }

        /// <inheritdoc />
        protected sealed override bool InnerSerialize<T>(T data, string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(data.GetType());
                serializer.Serialize(stream, data);
                stream.Flush();
                return true;
            }
        }

        /// <inheritdoc />
        public override string FileExtension { get; } = ".config";
    }
}