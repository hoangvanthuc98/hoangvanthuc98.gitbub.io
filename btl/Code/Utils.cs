using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace btl.Code
{
    public static class Utils
    {
        public static UploadFileClass GetUploadFile(string MediaPath, string strFileName, bool AddDatePath)
        {
            UploadFileClass tempUploadClass = new UploadFileClass();
            System.Text.RegularExpressions.Match matchResults;
            string strAdditionFolder = (AddDatePath ? String.Format(DateTime.Now.ToString("yyyy\\\\MM\\\\")) : "");
            var strSaveFile = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + Guid.NewGuid() + "." + strFileName.Split('.')[1];
            string strSaveFolder = MediaPath + strAdditionFolder;
            //Check folder exist
            try
            {
                if (System.IO.Directory.Exists(strSaveFolder) == false)
                {
                    //Create Directory
                    System.IO.Directory.CreateDirectory(strSaveFolder);
                }
                if (System.IO.File.Exists(strSaveFolder + strSaveFile))
                {
                    while (System.IO.File.Exists(strSaveFolder + strSaveFile))
                    {
                        matchResults = System.Text.RegularExpressions.Regex.Match(strSaveFile, "(?<FileName>.*?)(?:\\((?<AutoNumber>\\d*?)\\))?\\.(?<FileType>\\w*?)(?!.)");
                        if (matchResults.Success)
                        {
                            if (matchResults.Groups["AutoNumber"].Value == string.Empty)
                            {
                                strSaveFile = matchResults.Groups["FileName"].Value + "(1)." + matchResults.Groups["FileType"].Value;
                            }
                            else
                            {
                                strSaveFile = matchResults.Groups["FileName"].Value + "(" + (int.Parse(matchResults.Groups["AutoNumber"].Value) + 1).ToString() + ")." + matchResults.Groups["FileType"].Value;
                            }
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            tempUploadClass.Virtualpath = strAdditionFolder.Replace("\\", "/") + strSaveFile;
            tempUploadClass.FileName = strSaveFile;
            tempUploadClass.Fullpath = strSaveFolder + strSaveFile;
            tempUploadClass.FolderPath = strAdditionFolder.Replace("\\", "/");
            tempUploadClass.StrPathTemp = strSaveFolder;
            return tempUploadClass;
        }
    }

    public class UploadFileClass
    {
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private string fullpath;

        public string Fullpath
        {
            get { return fullpath; }
            set { fullpath = value; }
        }
        private string virtualpath;

        public string Virtualpath
        {
            get { return virtualpath; }
            set { virtualpath = value; }
        }

        private string folderPath;

        public string FolderPath
        {
            get { return folderPath; }
            set { folderPath = value; }
        }

        private string strPathTemp;

        public string StrPathTemp
        {
            get { return strPathTemp; }
            set { strPathTemp = value; }
        }
        public string pathThumb { get; set; }
    }

}