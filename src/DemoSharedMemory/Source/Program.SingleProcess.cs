﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Security;
using Standard.IPC.SharedMemory;

namespace SharedMemoryDemo
{
    partial class Program
    {
        [SecuritySafeCritical]
        internal static void RunSingleProcess(int serverCount, int clientCount, int elements = 100000, int bufferSize = 1048576, int count = 50)
        {
            int clientWaitCount = 0;
            int serverWaitCount = 0;
            long lastTick = 0;
            int writeCount = 0;
            int readCount = 0;

            int size = sizeof(byte) * bufferSize;

            Console.WriteLine("Node buffer size: {0}", size);
            Console.WriteLine("Number of nodes: {0}", count);
            Console.WriteLine("Number of readers: {0}", clientCount);
            Console.WriteLine("Number of writers: {0}", serverCount);
            Console.WriteLine("Number of elements to process: {0}", elements);

            int dataListCount = 256;
            // Generate random data to be written
            Random random = new Random();
            byte[][] dataList = new byte[dataListCount][];
            for (var j = 0; j < dataListCount; j++)
            {
                var data = new byte[size];
                random.NextBytes(data);
                dataList[j] = data;
            }

            Console.WriteLine("Populated random data.");

            long bytesWritten = 0;
            long bytesRead = 0;
            string name = Guid.NewGuid().ToString();
            var server = new CircularBuffer(name, count, size);
            
            Stopwatch sw = Stopwatch.StartNew();

            Action clientAction = () =>
            {
                byte[] testData = new byte[size];

                var client = new CircularBuffer(name);

                Stopwatch clientTime = new Stopwatch();
                clientTime.Start();
                long startTick = 0;
                long stopTick = 0;

                for (; ; )
                {
                    startTick = clientTime.ElapsedTicks;
                    int amount = client.Read(testData, 100);
                    bytesRead += amount;

                    if (amount == 0)
                        Interlocked.Increment(ref clientWaitCount);
                    else
                        Interlocked.Increment(ref readCount);

                    stopTick = clientTime.ElapsedTicks;

                    if (writeCount > elements && writeCount - readCount == 0)
                        break;
                }
            };

            for (int c = 0; c < clientCount; c++)
            {
#if NETFX
                Task c1 = Task.Factory.StartNew(clientAction);
#else
                ThreadPool.QueueUserWorkItem((o) => { clientAction(); });
#endif
            }

            bool wait = true;
            int index = 0;
            Action serverWrite = () =>
            {
                int serverIndex = Interlocked.Increment(ref index);

                var writer = (serverIndex == 1 ? server : new CircularBuffer(name));
                bool done = false;
                TimeSpan doneTime = TimeSpan.MinValue;
                for (; ; )
                {
                    if (writeCount <= elements)
                    {
                        int amount = writer.Write(dataList[random.Next(0, dataListCount)], 100);
                        bytesWritten += amount;
                        if (amount == 0)
                            Interlocked.Increment(ref serverWaitCount);
                        else
                            Interlocked.Increment(ref writeCount);
                    }
                    else
                    {
                        if (!done && serverIndex == 1)
                        {
                            doneTime = sw.Elapsed;
                            done = true;
                        }
                    }

                    if (serverIndex == 1 && sw.ElapsedTicks - lastTick > 1000000)
                    {
                        Console.WriteLine("Write: {0}, Read: {1}, Diff: {5}, Wait(cli/svr): {3}/{2}, {4}MB/s", 
                            writeCount, readCount, serverWaitCount, clientWaitCount, 
                            (int)((((bytesWritten + bytesRead) / 1048576.0) / sw.ElapsedMilliseconds) * 1000), 
                            writeCount - readCount);

                        lastTick = sw.ElapsedTicks;
                        if (writeCount > elements && writeCount - readCount == 0)
                        {
                            Console.WriteLine("Total Time: " + doneTime);
                            wait = false;
                            break;
                        }
                    }
                }
            };

            for (int s = 0; s < serverCount; s++)
            {
#if NETFX
                Task s1 = Task.Factory.StartNew(serverWrite);
#else
                ThreadPool.QueueUserWorkItem((o) => { serverWrite(); });
#endif
            }

            while (wait)
            {
                Thread.Sleep(100);
            }
        }
    }
}