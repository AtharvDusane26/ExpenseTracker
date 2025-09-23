using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ExpenseTracker.DataManagement.Serialization
{
    public class DataManager
    {
        public void Save<T>(T serializableObject, string fileName, DataContractSerializerSettings settings = null)
        {
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T), settings);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, null))
                    {
                        xmlWriter.Formatting = Formatting.Indented;
                        serializer.WriteObject(xmlWriter, serializableObject);
                        xmlWriter.Flush();
                        memoryStream.Position = 0;
                        using (StreamReader sr = new StreamReader(memoryStream))
                        {
                            if (File.Exists(fileName))
                                File.Delete(fileName);
                            using (StreamWriter sw = new StreamWriter(fileName))
                            {
                                sw.Write(sr.ReadToEnd());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public T Read<T>(string fileName, DataContractSerializerSettings settings = null)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T), settings);
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
                    {
                        T retVal = (T)serializer.ReadObject(fileStream);
                        return retVal;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return default;
        }
    }
}
