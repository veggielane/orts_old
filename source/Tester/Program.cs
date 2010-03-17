using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orts.Core.Timing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Orts.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var messages = new StringWriter();


            using (var timer = new ObservableTimer(25))
            {
                timer.TimerMessages.Subscribe(m =>
                    {
                        Console.WriteLine("{0} - {1}".fmt(m.CurrentTime.ToStringSafe("hh:mm:ss.fff"), m.Message));
                        messages.WriteLine("{0} - {1}".fmt(m.CurrentTime.ToStringSafe("hh:mm:ss.fff"), m.Message));
                    });

                timer.Subscribe(d =>
                    {
                        Console.WriteLine("{0:hh:mm:ss.fff} - TICK".fmt(d));
                        messages.WriteLine("{0:hh:mm:ss.fff} - TICK".fmt(d));
                    });


                var cancelSource = new CancellationTokenSource();


                var finishTask = new Task(o =>
                    {
                        while (true)
                        {
                            Console.ReadKey(true);

                            if (((CancellationToken)o).IsCancellationRequested)
                                break;
                            timer.Stop();
                        }
                    }, cancelSource.Token);
                 
                finishTask.Start();

                timer.Start();

                cancelSource.Cancel();

                Console.WriteLine("Timer should have stopped.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);

            }
        }
    }
}
