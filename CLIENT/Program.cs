using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using Microsoft.VisualBasic;

Random RND = new Random();
string BASE = AppDomain.CurrentDomain.BaseDirectory;

var cache = CreateCache();
Directory.CreateDirectory(cache);

RUN(ExtractedFile(ExportedZip()));



string ExportedZip()
{
    string myfile =$"{cache}/cache.zip";
    
    using(var  res = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetManifestResourceNames()[0]))
    {
    using (Stream st = new FileStream(myfile ,FileMode.Create))
    {
        res.CopyTo(st);
    }
    }

    return myfile;
}

string ExtractedFile(string zip)
{
    ZipFile.ExtractToDirectory(zip,cache);
    File.Delete(zip);
    string myFile = Directory.GetFiles(cache)[0];

    return myFile;
}

string CreateCache()
{
    string TEMP = RND.Next(999999999).ToString();
    string PATH = Path.Combine(BASE,$"cache (id-{TEMP})");

    if (Directory.Exists(PATH))  {PATH = CreateCache();}

    return PATH;
}

void RUN (string what)
{
    var pi = new ProcessStartInfo();
    pi.FileName = what;
    var p = new Process();
    p.StartInfo = pi;
    p.Start();
    p.WaitForExit();
}

Directory.Delete(cache,true);

