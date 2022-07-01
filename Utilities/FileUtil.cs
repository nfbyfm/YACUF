using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using YACUF.Utilities;

namespace YACUF.Utilities
{
    /// <summary>
    /// utility-class with functions for handling files and file-/folder-paths
    /// </summary>
    public static class FileUtil
    {
        #region xml-Funktionen

        /// <summary>
        /// saves (serializes) object as xml-file
        /// </summary>
        /// <param name="fileName">(absolute) filepath</param>
        /// <param name="xmlObject">object for serialization</param>
        /// <param name="exception">exception triggered if saveing failed</param>
        /// <returns>true if saving was successful</returns>
        public static bool TrySaveToXMLFile(string fileName, object xmlObject, out Exception? exception)
        {
            bool success = false;
            exception = null;

            try
            {
                if (xmlObject != null)
                {
                    if (fileName.IsValidString())
                    {
                        //create folder if it doesn't exist yet
                        if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                        }

                        XmlSerializer xs = new XmlSerializer(xmlObject.GetType());

                        TextWriter txtWriter = new StreamWriter(fileName);

                        xs.Serialize(txtWriter, xmlObject);

                        txtWriter.Close();
                        success = true;
                    }
                    else
                    {
                        throw new ArgumentException("Filename is not a valid string.");
                    }
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return success;
        }

        /// <summary>
        /// tries to load object from xml-file
        /// </summary>
        /// <typeparam name="T">expected class / type the object to read from file</typeparam>
        /// <param name="fileName">(absolute) filepath</param>
        /// <param name="readObject">parsed obejct</param>
        /// <param name="readException">exception triggered if reading from file failed</param>
        /// <returns>true if reading from file succeeded</returns>
        public static bool TryLoadFromXMLFile<T>(string fileName, out T readObject, out Exception? readException)
        {
            bool success = false;
            readObject = default(T);
            readException = null;

            try
            {
                if (File.Exists(fileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    using (Stream reader = new FileStream(fileName, FileMode.Open))
                    {
                        // deserialize file
                        readObject = (T)serializer.Deserialize(reader);
                    }

                    success = true;
                }
                else
                {
                    throw new FileNotFoundException("File '" + fileName + "' not found.");
                }
            }
            catch (Exception ex)
            {
                readException = ex;
            }

            return success;
        }



        #endregion


        #region filepath-functions
        /// <summary>
        /// creates absolute filepath combined with the folder of the current application
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>combined filepath</returns>
        public static string GetFullFileName(string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), fileName);
        }

        /// <summary>
        /// combines subfolder and filename relative to current application
        /// </summary>
        /// <param name="subfolderPath">subfolderpath</param>
        /// <param name="fileName">filename</param>
        /// <param name="createFolder">create folder if non-existant</param>
        /// <returns>combined filepath</returns>
        public static string CombineFileName(string subfolderPath, string fileName, bool createFolder)
        {
            string folderPath = CombineFolderPath(subfolderPath, createFolder);

            return Path.Combine(folderPath, fileName);
        }

        /// <summary>
        /// combines folderpath with the one of the current application
        /// </summary>
        /// <param name="subfolderPath">subfolder</param>
        /// <param name="createFolder">creates folder if non-existant</param>
        /// <returns>combined folderpath</returns>
        public static string CombineFolderPath(string subfolderPath, bool createFolder)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), subfolderPath);

            if (createFolder && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }


        /// <summary>
        /// generates shortened filepath (filename and extension only)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetShortFileName(string fileName)
        {
            string result;

            try
            {
                result = Path.GetFileName(fileName);
            }
            catch
            {
                result = fileName;
            }

            return result;
        }

        /// <summary>
        /// checks if given folderpath is valid
        /// </summary>
        /// <param name="directoryPath">path to check</param>
        /// <param name="checkDirectoryExists">check if folder actually exists or not</param>
        /// <returns>true if path is valid </returns>
        public static bool IsValidFolderPath(string directoryPath, bool checkDirectoryExists = false)
        {
            bool result = false;

            if (checkDirectoryExists)
            {
                try
                {
                    result = Directory.Exists(directoryPath);
                }
                catch
                {
                    result = false;
                }
            }
            else
            {
                try
                {
                    Path.GetFullPath(directoryPath);
                    result = Path.IsPathRooted(directoryPath);
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        #endregion

    }
}
