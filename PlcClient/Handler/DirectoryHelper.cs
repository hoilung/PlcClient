using System.IO;

namespace PlcClient.Handler
{
    public static class DirectoryHelper
    {
        /// <summary>
        /// 递归复制目录及其所有子目录和文件
        /// </summary>
        /// <param name="sourceDir">源目录路径</param>
        /// <param name="targetDir">目标目录路径</param>
        public static void CopyDirectory(string sourceDir, string targetDir)
        {
            // 检查源目录是否存在
            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"源目录不存在: {sourceDir}");
            }

            // 如果目标目录不存在，则创建
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // 获取源目录中的所有文件，并复制到目标目录
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);
                string targetFilePath = Path.Combine(targetDir, fileName);

                // 复制文件，如果目标文件已存在则覆盖
                File.Copy(filePath, targetFilePath, true);
            }

            // 递归复制所有子目录
            foreach (string subDirPath in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDirPath);
                string targetSubDirPath = Path.Combine(targetDir, subDirName);

                CopyDirectory(subDirPath, targetSubDirPath);
            }
        }
    }
}
