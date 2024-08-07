#region GLOBAL

using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using CONTROLLER;


var Zip = CompressionServices.Zip;
var Unzip = CompressionServices.Unzip;

var Encrypt = EncryptionServices.EncryptFile;
var Decrypt = EncryptionServices.DecryptFile;

var RND = new Random();
string BASE = AppDomain.CurrentDomain.BaseDirectory;
#endregion


#region STARTLINE

Ask("Press E for encrypt, D for decrypt!");

char CH = Console.ReadKey().KeyChar;

if      (CH == 'E' || CH == 'e')    {Console.Clear();   StartDialouge(true);    } //Action('e');}
else if (CH == 'D' || CH == 'd')    {Console.Clear();   StartDialouge(false);   } //Action('d');}
else                                {return;                                    }
#endregion

/* REPLACED BY 'StartDialouge'

void Action(char action)
{
    if (action == 'e')
    {
        StartDialouge(true);        
    }
    else if (action == 'd')
    {
        StartDialouge(false);
    }
}
*/

void StartDialouge(bool isEncrypting)
{
        Ask(isEncrypting ? "Input file(exe): " : "Input file(zip): ");
        string INPUT = Console.ReadLine();
        
        if (!File.Exists(INPUT)){return;}

// ========================================
        
        Ask("Output directory: ");
        string OUTPUT = Console.ReadLine();

        if (!Directory.Exists(OUTPUT)){return;}

// ========================================

        Ask("Enter your password: ");
        Console.ForegroundColor = ConsoleColor.Black;
        string PASS = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.White;

        if (PASS == null){return;}
        
// ========================================

        Ask("Enter your initialisation key: ");
        Console.ForegroundColor = ConsoleColor.Black;
        string IV = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.White;

        if (IV == null){return;}

// ========================================

    ManageActions(INPUT,OUTPUT,PASS, IV, isEncrypting);
}

void ManageActions(string inputP, string outputP, string key, string iv, bool isEncrypting)
{
#region Variables
        string FULLNAME = GetName(inputP);
        // full name of the input file (name + extension)
        string SHORTNAME = RemoveExtension(FULLNAME);
        // short name of the input file (name)
        
        string CACHE1PATH = CreateCache();
        // create the first cache dir

        string CACHE2PATH ="null";
        if (isEncrypting)
        {CACHE2PATH  = CreateCache();}
        // Create the second cache (if encrypting)

        string CACHEDFILE = Path.Combine(CACHE1PATH,isEncrypting ? FULLNAME : SHORTNAME+".exe");
        // get the path of file that is inside cache
#endregion

    if(isEncrypting)
        {
            Encrypt(inputP,CACHEDFILE,key,iv);
            string MYZIP = Path.Combine(CACHE2PATH, "archive.zip");
            Zip(CACHE1PATH,MYZIP);
            
            // ========================================

            string client = CreateCache();
            ExportCLIENT(client);
            FileShredder.ShredFile($"{client}/CLIENT/.source/archive.zip");
            
            File.Move(MYZIP,$"{client}/CLIENT/.source/archive.zip");
            Directory.Delete(CACHE2PATH, true);
            string myArg = "publish";
            
            var p = new Process();
            var pi = new ProcessStartInfo();
            pi.FileName = "dotnet.exe";
            pi.WorkingDirectory = $"{client}/CLIENT";
            pi.Arguments = myArg;
            
            p.StartInfo = pi;
            p.Start();
            p.WaitForExit();

            string startLine = $"{client}/CLIENT/bin/Release/net8.0/";
            File.Move($"{Directory.GetDirectories(startLine)[0]}/publish/CLIENT.exe",$"{outputP}/CLIENT.exe");
            Directory.Delete(client,true);

        }
        else
        {
            Unzip(inputP,CACHE1PATH);
            string MYEXE = Path.Combine(outputP, Directory.GetFiles(CACHE1PATH, "*.exe")[0]); // $"{SHORTNAME}.exe");
            Decrypt(CACHEDFILE,MYEXE,key,iv);
        }

    FileShredder.ShredFile(CACHEDFILE);
    Directory.Delete(CACHE1PATH,true);
}

void ExportCLIENT(string where)
{
    // Get the assembly and the resource name
    using(var  res = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetManifestResourceNames()[0]))
    {
        ZipFile.ExtractToDirectory(res,where);
    /*using (Stream st = new FileStream("arch.zip",FileMode.Create))
    {
        res.CopyTo(st);
    }*/
    }
}

string CreateCache()
{
    string TEMP = RND.Next(999999999).ToString();
    string PATH = Path.Combine(BASE,"Cache",TEMP);

    if (Directory.Exists(PATH))  {PATH = CreateCache();}
    else {Directory.CreateDirectory(PATH);}
    
    return PATH;
}

void Ask(string ques)
{
    Console.WriteLine(ques);
}

string GetName(string path)
{
    char[] NO = @"/\".ToCharArray();
    string WORK = path;
    
    WORK = RemoveLastSlash(WORK);

    WORK = WORK.Remove(0, WORK.LastIndexOfAny(NO)+1);

    return WORK;
}

string RemoveExtension(string path)
{
    char NO = '.';
    string WORK = path;

    RemoveLastSlash(WORK);

    WORK = WORK.Remove(WORK.LastIndexOf(NO), 4);

    return WORK;
}

string RemoveLastSlash(string path)
{
    char LAST = path[path.Length-1];
    char[] NO = @"/\".ToCharArray();
    string WORK = path;

    if(LAST == NO[0] || LAST == NO[1])
    {
        WORK = WORK.Remove(WORK.Length-1);

        char sLAST = WORK[WORK.Length-1];
        if(sLAST == NO[0] || sLAST == NO[1])
        {WORK = RemoveLastSlash(WORK);}
    }
    return WORK;
}
