//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using AssemblyGestore;
using AssemblyGestoreFile;
using ClientiLibrary;
using System.Configuration;
using System.Reflection;

//namespace Server
//{
//    class Server
//    {
//        static void Main(string[] args)
//        {
//            HttpChannel channel = new HttpChannel(8080);
//            ChannelServices.RegisterChannel(channel, false);

//            RemotingConfiguration.RegisterWellKnownServiceType(
//                typeof(GestoreClienti),
//                "GestoreClienti",
//                WellKnownObjectMode.Singleton);

//            RemotingConfiguration.RegisterWellKnownServiceType(
//                typeof(GestoreFileClienti),
//                "GestoreFileClienti",
//                WellKnownObjectMode.Singleton);

//            Console.WriteLine("Server avviato. Premi INVIO per terminare...");
//            Console.ReadLine();
//        }
//    }
//}


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

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpChannel channel = new HttpChannel(8080);
            ChannelServices.RegisterChannel(channel, false);

            string connectionDB = ConfigurationManager.AppSettings["DatabaseConnection"];
            string filePercorso = ConfigurationManager.AppSettings["FileConnection"];

            //Carico gli assembly dinamicamente
            Assembly assemblyGestore = Assembly.LoadFrom(@"C:\Users\d.dieleuterio\source\repos\ProgettoFramework\ClienteSoluzione\AssemlyGestore\obj\Debug\AssemlyGestore.dll");
            Assembly assemblyGestoreFile = Assembly.LoadFrom(@"C:\Users\d.dieleuterio\source\repos\ProgettoFramework\ClienteSoluzione\AssemblyGestoreFile\obj\Debug\AssemblyGestoreFile.dll");

            Type gestoreClientiType = assemblyGestore.GetType("AssemblyGestore.GestoreClienti");
            Type gestoreFileClientiType = assemblyGestoreFile.GetType("AssemblyGestoreFile.GestoreFileClienti");

            // Creo le istanze delle classi
            object gestoreClientiInstance = Activator.CreateInstance(gestoreClientiType, connectionDB);
            object gestoreFileClientiInstance = Activator.CreateInstance(gestoreFileClientiType, filePercorso);

            // Registro i servizi
            RemotingServices.Marshal((MarshalByRefObject)gestoreClientiInstance, "GestoreClienti");
            RemotingServices.Marshal((MarshalByRefObject)gestoreFileClientiInstance, "GestoreFileClienti");

            Console.WriteLine("Server avviato. Premi INVIO per terminare...");
            Console.ReadLine();
        }
    }
}