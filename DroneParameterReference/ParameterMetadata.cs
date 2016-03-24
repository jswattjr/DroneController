using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DroneParameterReference
{
    public class ParameterMetadata
    {
        static Logger logger = LogManager.GetLogger("database");
        static Dictionary<String, ParameterMetadataEntry> parameterMetadata = null;
        static Object lockObj = new object();
        static String paramsFilename = "ParameterMetaDataBackup.xml";

        public ParameterMetadata()
        {
            if (null == parameterMetadata)
            {
                lock (lockObj)
                {
                    if (null == parameterMetadata)
                    {
                        loadData();
                    }
                }
            }
        }

        public ParameterMetadataEntry fetchMetadata(String key)
        {
            if (null == parameterMetadata)
            {
                return null;
            }
            if (parameterMetadata.ContainsKey(key))
            {
                return parameterMetadata[key];
            }
            return null;
        }

        public int numEntries()
        {
            return parameterMetadata.Keys.Count;
        }

        static private void loadData()
        {
            // source file
            string metadata = GetResourceTextFile(paramsFilename);

            // destination static object
            parameterMetadata = new Dictionary<string, ParameterMetadataEntry>();

            // Create an XmlReader
            using (XmlReader reader = XmlReader.Create(new StringReader(metadata)))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);

                XmlNodeList arducopter2 = doc.DocumentElement.SelectNodes("/Params/ArduCopter2");
                if (arducopter2.Count >= 1)
                {
                    XmlNodeList arduParams = arducopter2[0].ChildNodes;
                    foreach (XmlNode parameter in arduParams)
                    {
                        ParameterMetadataEntry entry = new ParameterMetadataEntry();
                        entry.key = parameter.Name;
                        entry.DisplayName = safeGet(parameter, "DisplayName");
                        entry.Description = safeGet(parameter, "Description");
                        entry.Increment = safeGet(parameter, "Increment");
                        entry.Units = safeGet(parameter, "Units");
                        entry.User = safeGet(parameter, "User");
                        String range = safeGet(parameter, "Range");
                        if ((null != range)&&(range.Length > 0))
                        {
                            String[] ranges = range.Split(' ');
                            if (ranges.Length > 2)
                            {
                                entry.Lower = ranges[0];
                                entry.Upper = ranges[1];
                            }
                        }
                        parameterMetadata.Add(entry.key, entry);
                    }
                }

                logger.Debug("Loaded {0} parameters from file {1}", parameterMetadata.Keys.Count, paramsFilename);
            }
        }

        static private String safeGet(XmlNode node, String field)
        {
            XmlNode property = node.SelectSingleNode(field);
            String result = "";
            if (null != property)
            {
                result = property.InnerText;
            }
            return result;
        }

        static private string GetResourceTextFile(string filename)
        {
            string result = string.Empty;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DroneParameterReference." + filename))
            {
                if (null == stream)
                {
                    logger.Error("Unable to load Parameter Resource File {0}, parameter information may not be available.", filename);
                }
                using (StreamReader sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

    }
}
