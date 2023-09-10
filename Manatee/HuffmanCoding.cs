
namespace Manatee
{
    public class HuffmanCoding
    {
        // лист узлов часота - символ
        private List<Node> nodeList = new();
        // храним кол-во различных символов во входном файле
        private byte sizeOfDictionary;


        // private string content;

        // путь к исходному файлу
        private string filePath;

        private string compressedFileName;


        //private StringBuilder content_ = new();

        public HuffmanCoding(string filePath)
        {
            //using (StreamReader input = new(filePath, true))
            //{
            //    content = input.ReadToEnd();
            //}




            this.filePath = filePath;
            compressedFileName = filePath + "_Compressed.manatee";

        }


        public string CompressFile()
        {
            // словарь для подсчета частот символов
            // Dictionary<char, int> dict = new();


            using (StreamReader input = new(filePath, true))
            {
                int inputSymbol;
                char symbol;
                int index;
                while ((inputSymbol = input.Read()) != -1)
                {

                    //dict[symbol] = dict.ContainsKey(symbol) ? dict[symbol] + 1 : 1;


                    symbol = (char)inputSymbol;

                    if ((index = nodeList.FindIndex(node => node.symbol == symbol)) != -1)
                    {
                        nodeList[index].frequency++;
                    }
                    else
                    {
                        nodeList.Add(new Node(symbol, 1));
                    }
                }
            }

            //foreach (char symbol in content)
            //{
            //    dict[symbol] = dict.ContainsKey(symbol) ? dict[symbol] + 1 : 1;
            //}


            // создание списка узлов частота - символ
            //foreach (KeyValuePair<char, int> item in dict)
            //{
            //    nodeList.Add(new Node(item.Key, item.Value));
            //}

            sizeOfDictionary = (byte)nodeList.Count;

            // быстрая сортировка узлов в порядке возрастания частоты
            nodeList.Sort((a, b) => a.frequency.CompareTo(b.frequency));


            BuildHuffmanTree();

            // передаем корень дерева Хаффмана, стэк под код символа, словарь - кодовую талицу
            recBuildCodeTable(nodeList[0], bitArray, codeTable);


            using (FileStream fOutput = new(compressedFileName, FileMode.Create, FileAccess.Write))
            {
                // запись служебной информации
                fOutput.WriteByte(sizeOfDictionary);
                WriteServicInfo(codeTable, fOutput);

                // запись сжатого текста в файл
                WriteContent(fOutput, codeTable);
            }


            return compressedFileName;
        }


        void BuildHuffmanTree()
        {
            // построение дерева Хаффмана
            while (nodeList.Count > 1)
            {
                // берутся два узла с минимальными частотами, их частоты складываются
                int frequency = nodeList[0].frequency + nodeList[1].frequency;

                // создается их родительский узел, помеченный как "не лист"
                nodeList.Add(new(null, frequency, nodeList[0], nodeList[1]));

                // использованные узлы удаляются из списка
                nodeList.RemoveRange(0, 2);

                // сортировка созданного узла
                InsertionSort(nodeList);
            }
        }




        // хранилище текущего пути из 0 и 1
        private List<bool> bitArray = new();

        // символ - код
        private Dictionary<char, List<bool>> codeTable = new();

        // создание таблицы символов с кодами
        private void recBuildCodeTable(Node node, List<bool> bitArray, Dictionary<char, List<bool>> codeTable)
        {
            // рекурсивный обход дерева Хаффмана 
            if (node.symbol == null)
            {
                // левый путь - единицы
                bitArray.Add(true);
                recBuildCodeTable(node.left, bitArray, codeTable);

                bitArray.RemoveAt(bitArray.Count - 1);

                // правый путь - нули
                bitArray.Add(false);
                recBuildCodeTable(node.right, bitArray, codeTable);

                bitArray.RemoveAt(bitArray.Count - 1);
            }
            // если достигли листа, сохраняем код его символа в таблице
            else
            {
                codeTable[(char)node.symbol] = new();

                foreach (bool bit in bitArray)
                {
                    codeTable[(char)node.symbol].Add(bit);
                }
            }
        }



        // функция побитовой записи сжатых символов исходного файла
        private void WriteContent(FileStream fOutput, Dictionary<char, List<bool>> codeTable)
        {
            using (StreamReader input = new(filePath, true))
            {
                int i = 8;
                byte bt = 0;
                int inputSymbol;
                while ((inputSymbol = input.Read()) != -1)
                {
                    foreach (bool bit in codeTable[(char)inputSymbol])
                    {
                        bt |= (byte)(Convert.ToByte(bit) << i - 1);
                        --i;

                        if (i == 0)
                        {
                            fOutput.WriteByte(bt);
                            i = 8;
                            bt = 0;
                        }
                    }
                }

                if (i != 8)
                {
                    fOutput.WriteByte(bt);
                }
            }


            //foreach (char symbol in content_)
            //{
            //    foreach (bool bit in codeTable[symbol])
            //    {
            //        bt |= (byte)(Convert.ToByte(bit) << i - 1);
            //        --i;

            //        if (i == 0)
            //        {
            //            fOutput.WriteByte(bt);
            //            i = 8;
            //            bt = 0;
            //        }
            //    }
            //}


        }



        // запись в файл символов в новой и старой кодировках
        private void WriteServicInfo(Dictionary<char, List<bool>> codeTable, FileStream fOutput)
        {
            byte bt;

            foreach (KeyValuePair<char, List<bool>> item in codeTable)
            {
                // запись символов в utf-8 в файл справа налево
                fOutput.WriteByte((byte)(item.Key));
                fOutput.WriteByte((byte)(item.Key >> 8));


                // запись кол-ва бит символа в сжатой кодировке
                fOutput.WriteByte((byte)item.Value.Count);



                // запись символа в сжатой кодировке слева направо
                bt = 0;
                int i = 8;
                foreach (bool bit in item.Value)
                {
                    // заполнение байта битами
                    bt |= (byte)(Convert.ToByte(bit) << i - 1);
                    --i;

                    // запись заполненного байта в файл
                    if (i == 0)
                    {
                        fOutput.WriteByte(bt);
                        i = 8;
                        bt = 0;
                    }
                }

                if (i != 8)
                {
                    fOutput.WriteByte(bt);
                }
            }
        }




        // сортировка вставками с учетом высоты
        private void InsertionSort(List<Node> nodeList)
        {
            int i = nodeList.Count - 1;

            for (int j = i - 1; j > 0 && nodeList[i].frequency <= nodeList[j].frequency; j--, i--)
            {
                if (nodeList[i].frequency == nodeList[j].frequency)
                {
                    if (nodeList[i].height >= nodeList[j].height)
                    {
                        break;
                    }
                }

                (nodeList[i], nodeList[j]) = (nodeList[j], nodeList[i]);
            }
        }
    }
}

