namespace CONTROLLER;

using System;
using System.IO;
using System.Security.Cryptography;

public static class FileShredder
{
    public static void ShredFile(string filePath, int passes = 3)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }

        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
        {
            byte[] buffer = new byte[4096];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            for (int i = 0; i < passes; i++)
            {
                fileStream.Position = 0;
                while (fileStream.Position < fileStream.Length)
                {
                    rng.GetBytes(buffer);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
            }

            fileStream.SetLength(0);
        }

        File.Delete(filePath);
    }
}
