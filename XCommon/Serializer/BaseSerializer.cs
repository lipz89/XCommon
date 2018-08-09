using System.IO;

namespace XCommon.Serializer
{
    /// <summary>
    /// ���л�����
    /// </summary>
    abstract class BaseSerializer : ISerializer
    {
        /// <inheritdoc />
        public T Deserialize<T>(string filePath, bool isRequire = false) where T : class
        {
            if (!File.Exists(filePath))
            {
                if (isRequire)
                {
                    throw new System.Exception("�ļ�[" + filePath + "]�����ڡ�");
                }
                return default(T);
            }

            return InnerDeserialize<T>(filePath);
        }

        /// <summary>
        /// �����л�
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected abstract T InnerDeserialize<T>(string path);

        /// <inheritdoc />
        public bool Serialize<T>(T data, string filePath) where T : class
        {
            if (data == null)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }

            CheckFile(filePath);
            return InnerSerialize(data, filePath);
        }

        /// <summary>
        /// ���л�
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected abstract bool InnerSerialize<T>(T data, string fileName) where T : class;

        /// <inheritdoc />
        public abstract string FileExtension { get; }

        private void CheckFile(string fileName)
        {
            var fi = new FileInfo(fileName);
            var dir = fi.Directory;
            if (!dir.Exists)
            {
                dir.Create();
            }
        }
    }
}