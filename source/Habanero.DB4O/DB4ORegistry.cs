using Db4objects.Db4o;
using Habanero.Base;

namespace Habanero.DB4O
{
    public static class DB4ORegistry
    {
        private static string _server;
        private static int _port;
        public static IObjectContainer DB { get; set; }
        public static IObjectServer DB4OServer { get; set; }

        public static void CreateDB4OObjectContainer(string fileName)
        {
            Db4oFactory.Configure().ObjectClass(typeof(BusinessObjectDTO)).CascadeOnUpdate(true);
            DB = Db4oFactory.OpenFile(fileName);
        }

        public static void CreateDB4OServerConfiguration(string server, int port, string fileName)
        {
            Db4oFactory.Configure().ObjectClass(typeof(BusinessObjectDTO)).CascadeOnUpdate(true);
            _server = server;
            _port = port;
            DB4OServer = Db4oFactory.OpenServer(fileName,port);
        }        
        
        public static void OpenDB4OClient(string userName,string passWord)
        {
            DB = Db4oFactory.OpenClient(_server,_port,userName,passWord);
        }
    }
}
