using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using AssemblyGestore;
using AssemblyGestoreFile;
using ClientiLibrary;
using System.Configuration;
using System.Reflection;
using static System.Net.WebRequestMethods;
using System.Runtime.ConstrainedExecution;
using System.Management.Instrumentation;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //Viene creato l'oggetto HttpChannel sulla porta 8080.Questo oggetto è un canale di comunicazione che utilizza il protocollo HTTP per trasmettere i messaggi tra il server e i client.
            HttpChannel channel = new HttpChannel(8080);
            //Si registra il canale creato, in modo che il sistema possa utilizzarlo per le comunicazioni.
            ChannelServices.RegisterChannel(channel, false);

            string connectionDB = ConfigurationManager.AppSettings["DatabaseConnection"];
            string filePercorso = ConfigurationManager.AppSettings["FileConnection"];

            // Carico gli assembly dinamicamente
            Assembly assemblyGestore = Assembly.LoadFrom(@"C:\Users\d.dieleuterio\source\repos\ProgettoFramework\ClienteSoluzione\AssemlyGestore\obj\Debug\AssemlyGestore.dll");
            Assembly assemblyGestoreFile = Assembly.LoadFrom(@"C:\Users\d.dieleuterio\source\repos\ProgettoFramework\ClienteSoluzione\AssemblyGestoreFile\obj\Debug\AssemblyGestoreFile.dll");

            //Si ottengono i tipi delle classi GestoreClienti e GestoreFileClienti dai due assembly caricati.
            Type gestoreClientiType = assemblyGestore.GetType("AssemblyGestore.GestoreClienti");
            Type gestoreFileClientiType = assemblyGestoreFile.GetType("AssemblyGestoreFile.GestoreFileClienti");


            Console.WriteLine("File path: " + filePercorso);

            // Creo le istanze delle classi
            //Si creano le istanze delle classi GestoreClienti e GestoreFileClienti utilizzando il costruttore che accetta la stringa di connessione al database e il percorso del file come argomenti, rispettivamente.
            object gestoreClientiInstance = Activator.CreateInstance(gestoreClientiType, connectionDB);
            //object gestoreFileClientiInstance = Activator.CreateInstance(gestoreFileClientiType, filePercorso);
            GestoreFileClienti gestoreFileClienti = new GestoreFileClienti(filePercorso);

            // Registraro l'oggetto gestoreClientiInstance come oggetto remoto, in modo che i client possano accedervi da remoto tramite .NET Remoting. Il primo argomento del metodo è l'oggetto che deve essere registrato come oggetto remoto, che viene eseguito tramite l'implementazione della classe MarshalByRefObject. Il secondo argomento è il nome dell'oggetto remoto, che viene utilizzato dai client per accedere all'oggetto remoto registrato//Si registrano i servizi per le istanze delle classi create, in modo che possano essere utilizzati dai client attraverso il.NET Remoting.
            RemotingServices.Marshal((MarshalByRefObject)gestoreClientiInstance, "GestoreClienti");
            //RemotingServices.Marshal((MarshalByRefObject)gestoreFileClientiInstance, "GestoreFileClienti");
            RemotingServices.Marshal(gestoreFileClienti, "GestoreFileClienti");

            //Console.ReadLine() è utilizzato per impedire alla console di chiudersi immediatamente dopo l'avvio del server
            Console.WriteLine("Server avviato. Premi INVIO per terminare...");
            Console.ReadLine();
        }
    }
}

//namespace Server
//{
//    class Server
//    {
//        static void Main(string[] args)
//        {
//            HttpChannel channel = new HttpChannel(8080);
//            ChannelServices.RegisterChannel(channel, false);

//            string connectionDB = ConfigurationManager.AppSettings["DatabaseConnection"];
//            GestoreClienti gestoreClienti = new GestoreClienti(connectionDB);
//            RemotingServices.Marshal(gestoreClienti, "GestoreClienti");

//            string filePercorso = ConfigurationManager.AppSettings["FileConnection"];
//            GestoreFileClienti gestoreFileClienti = new GestoreFileClienti(filePercorso);
//            RemotingServices.Marshal(gestoreFileClienti, "GestoreFileClienti");

//            Console.WriteLine("Server avviato. Premi INVIO per terminare...");
//            Console.ReadLine();
//        }
//    }
//}