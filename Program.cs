using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Problem01
{
    class Program
    {
        static byte[] Data_Global = new byte[1000000000];
        static long Sum_Global = 0;
        static int G_index = 0;
        static int threadCount = 16;

        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try 
            {
                Data_Global = (byte[]) bf.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally
            {
                fs.Close();
            }

            return returnData;
        }
        static int sum(int a)
        {
            int sum_local = 0;
            if (Data_Global[a] % 2 == 0)
            {
                sum_local -= Data_Global[a];
            }
            else if (Data_Global[a] % 3 == 0)
            {
                sum_local += (Data_Global[a]*2);
            }
            else if (Data_Global[a] % 5 == 0)
            {
                sum_local += (Data_Global[a] / 2);
            }
            else if (Data_Global[a] %7 == 0)
            {
                sum_local += (Data_Global[a] / 3);
            }
            Data_Global[a] = 0;
            // a++;   
            return sum_local;
        }

        static void ThreadFx(object data) {
            int id = Convert.ToInt32(data);
            int allsum = 0;
            for (int j = id; j < 1000000000; j += threadCount){
                allsum += sum(j);
            }
            Sum_Global += allsum;
        }
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int i, y;

            /* Read data from file */
            Console.Write("Data read...");
            y = ReadData();
            if (y == 0)
            {
                Console.WriteLine("Complete.");
            }
            else
            {
                Console.WriteLine("Read Failed!");
            }

            /* Start */
            Console.Write("\n\nWorking...");
            sw.Start();
            // for (i = 0; i < 1000000000; i++){
            //     sum(i);
            // }
            Thread[] threads = new Thread[threadCount];
            for (i = 0; i < threadCount; i++) {
                threads[i] = new Thread (new ParameterizedThreadStart(ThreadFx));
                threads[i].Start(i);
            }
            for (i = 0; i < threadCount; i++) {
                threads[i].Join();
            }
            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}
