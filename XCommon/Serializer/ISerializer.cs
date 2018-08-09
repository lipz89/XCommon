namespace XCommon.Serializer
{
    /// <summary>
    /// 序列化接口
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isRequire"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Deserialize<T>(string path, bool isRequire = false) where T : class;
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool Serialize<T>(T data, string fileName) where T : class;
        /// <summary>
        /// 文件扩展名
        /// </summary>
        string FileExtension { get; }
    }
}
