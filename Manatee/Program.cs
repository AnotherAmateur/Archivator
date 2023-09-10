using Manatee;
using System.IO.Compression;




string filePath = "E:/Projects/Manatee/inputUtf8.txt";
string compressedFileName = "E:/Projects/Manatee/inputUtf8.txt_Compressed.manatee";



Console.WriteLine("Кодировать - 1\n Декодирвоать - 2");
int choice = Console.Read();


switch (choice)
{
    case '1':
        HuffmanCoding file1 = new(filePath);
        Console.WriteLine(file1.CompressFile()); 
        break;

    case '2':
        HuffmanDecoding file2 = new(compressedFileName);
        Console.WriteLine(" Decompressed to " + file2.DecompressFile());
        break;

    default:
        Console.WriteLine("Неверный ввод");
        break;
}






//string filePath1 = "E:/Projects/Manatee/inputUtf8.txt_Compressed.manatee";
//string filePath2 = "E:/Projects/Manatee/autoInputZiped2.bb";

//using FileStream inf = new FileStream(filePath1, FileMode.Open);
//using FileStream outf = File.Create(filePath2);

//using GZipStream compressionStream = new GZipStream(outf, CompressionMode.Compress);
//inf.CopyTo(compressionStream);



//using FileStream sourceStream = new FileStream(filePath2, FileMode.OpenOrCreate);
//using FileStream targetStream = File.Create("E:/Projects/Manatee/autoUnZippped2.txt");

//using GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress);
//await decompressionStream.CopyToAsync(targetStream);