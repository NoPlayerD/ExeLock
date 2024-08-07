using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using CONTROLLER;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;


Random RND = new Random();
string BASE = AppDomain.CurrentDomain.BaseDirectory;


Console.WriteLine("Enter your key: ");
var key = Console.ReadLine();

Console.WriteLine("Enter your iv (initialisation vector): ");
var iv = Console.ReadLine();


var cache = CreateCache();
Directory.CreateDirectory(cache);

try{RUN(DecryptedFile(ExtractedFile()));}
catch(Exception ex){Console.WriteLine(ex.Message.ToString());Console.ReadLine();};




string ExtractedFile()
{
    using(var  res = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetManifestResourceNames()[0]))
    {
    ZipFile.ExtractToDirectory(res,cache);
    }

    string myfile =Directory.GetFiles(cache)[0];
    return myfile;
}

string DecryptedFile(string file)
{
    string myFile = Path.Combine(cache,$"{Path.GetFileNameWithoutExtension(file)} [ue]{Path.GetExtension(file)}");
    EncryptionServices.DecryptFile(file, myFile, key, iv);
    File.Delete(file);

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
    
    Console.WriteLine("Running '{0}'",Path.GetFileName(pi.FileName));

    p.Start();
    p.WaitForExit();
}

foreach (string my in Directory.GetFiles(cache,"",searchOption: System.IO.SearchOption.AllDirectories))
{
    FileShredder.ShredFile(my);
    Console.WriteLine($"'{my}' file shredded");
}

Directory.Delete(cache,true);