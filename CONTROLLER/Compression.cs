namespace CONTROLLER;
using System.IO.Compression;

public class CompressionServices
{
    public static void Zip(string sourceDirPath, string destFilePath)
    {
        ZipFile.CreateFromDirectory(sourceDirPath,destFilePath);
    }

    public static void Unzip(string sourceFilePath, string destDirPath)
    {
        ZipFile.ExtractToDirectory(sourceFilePath,destDirPath);
    }
}
