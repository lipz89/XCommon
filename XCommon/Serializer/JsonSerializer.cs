using System.IO;

namespace XCommon.Serializer
{
    /// <summary>
    /// JSON序列化类
    /// </summary>
    class JsonSerializer : BaseSerializer
    {
        /// <inheritdoc />
        public override string FileExtension { get; } = ".json";

        /// <inheritdoc />
        protected sealed override T InnerDeserialize<T>(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                //构建Json.net的读取流  
                JsonReader reader = new JsonTextReader(sr);
                //对读取出的Json.net的reader流进行反序列化，并装载到模型中
                return GetJsonSerializer().Deserialize<T>(reader);
            }
        }

        /// <inheritdoc />
        protected sealed override bool InnerSerialize<T>(T data, string fileName)
        {
            using (StreamWriter sr = new StreamWriter(fileName))
            {
                //构建Json.net的读取流  
                JsonWriter write = new JsonTextWriter(sr);
                //对读取出的Json.net的reader流进行反序列化，并装载到模型中
                GetJsonSerializer().Serialize(write, data);
                return true;
            }
        }
        private Newtonsoft.Json.JsonSerializer GetJsonSerializer()
        {
            return new Newtonsoft.Json.JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
        }
    }
}