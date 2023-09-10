using System.Text;


namespace Manatee
{
    public class HuffmanDecoding
    {
        string filePath;
        string decompressedFileName;

        public HuffmanDecoding(string filePath)
        {
            this.filePath = filePath;
            decompressedFileName = filePath.Substring(0, filePath.LastIndexOf('_')) + "_Decompressed.txt";
        }

        public string DecompressFile()
        {
            // чтение сжатого файла
            using (FileStream fInput = new(filePath, FileMode.Open, FileAccess.Read))
            {
                Node2 huffmanTree = new();
                StringBuilder decodedContent = new();


                // читаем весь файл в массив байтов
                byte[] bytes = new byte[fInput.Length];
                fInput.Read(bytes, 0, bytes.Length);

                int index = 0;

                // восстанавливаем дерево хаффмана
                char symbol;
                int symbolsCount = bytes[index++];
                Node2 temp = huffmanTree;
                for (int i = 0; i < symbolsCount; i++)
                {
                    temp = huffmanTree;

                    symbol = BitConverter.ToChar(bytes, index);

                    index += 2;

                    int symbolBitsCount = bytes[index++];
                    for (int k = symbolBitsCount, c = 7; k > 0; k--, c--)
                    {
                        byte bit = (byte)(bytes[index] >> 7);

                        if (k == 1)
                        {
                            temp = temp.Add(symbol, bit);
                            break;
                        }
                        else
                        {
                            temp = temp.Add(null, bit);
                        }

                        bytes[index] = (byte)(bytes[index] << 1);

                        if (c == 0)
                        {
                            ++index;
                            c = 8;
                        }
                    }

                    index++;
                }


                // декодирование содержания исходного файла
                int r;
                temp = huffmanTree;
                while (index < bytes.Length)
                {
                    r = 8;

                    while (r > 0)
                    {
                        byte bit = (byte)(bytes[index] >> 7);

                        temp = (bit == 1 ? temp.left : temp.right);
                        if (temp.symbol != null)
                        {
                            decodedContent.Append(temp.symbol);
                            temp = huffmanTree;
                        }

                        --r;

                        bytes[index] = (byte)(bytes[index] << 1);
                    }

                    ++index;
                }

                // запись декодированной информации в файл
                using (StreamWriter fOutput = new(decompressedFileName, false, Encoding.UTF8))
                {
                    fOutput.Write(decodedContent);
                }

                return decompressedFileName;
            }

        }

    }








}
