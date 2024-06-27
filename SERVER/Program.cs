#region GLOBAL

using System.ComponentModel;
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

        string MYFULLNAME = GetName(INPUT);
        string MYNAME = RemoveExtension(MYFULLNAME);
        
        string CACHE = CreateCache();
        Directory.CreateDirectory(CACHE);
        string CACHED = Path.Combine(CACHE,isEncrypting ? MYFULLNAME : MYNAME+".exe");

        if(isEncrypting)
        {
            Encrypt(INPUT,CACHED,PASS,IV);
            string MYZIP = Path.Combine(OUTPUT, $"{MYNAME}.zip");
            Zip(CACHE,MYZIP);
        }
        else
        {
            Unzip(INPUT,CACHE);
            string MYEXE = Path.Combine(OUTPUT, $"{MYNAME}.exe");
            Decrypt(CACHED,MYEXE,PASS,IV);
        }

        File.Delete(CACHED);
        Directory.Delete(CACHE,true);

}

string CreateCache()
{
    string TEMP = RND.Next(999999999).ToString();
    string PATH = Path.Combine(BASE,"Cache",TEMP);

    if (Directory.Exists(PATH))  {PATH = CreateCache();}

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