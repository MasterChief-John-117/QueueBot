using System;
using System.Diagnostics;

    namespace QueueBot
    {
        class Program
        {
            static bool run = true;
            static void Main(string[] args)
            {
                    MyBot bot = new MyBot();
            }



            public static void restart()
            {
                Process p = null;
                try
                {
                    string targetDir;
                    targetDir = string.Format(@"C:\Users\Galen\Desktop\QueueBot v1.0 RELEASE");
                    p = new Process();
                    p.StartInfo.WorkingDirectory = targetDir;
                    p.StartInfo.FileName = "run.bat";

                    p.StartInfo.Arguments = string.Format("C-Sharp Console application");
                    p.StartInfo.CreateNoWindow = false;
                    p.Start();
                    p.WaitForExit();
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Exception Occurred :{0},{1}",
                        ex.Message, ex.StackTrace.ToString());
                }
            }


        }
    }
