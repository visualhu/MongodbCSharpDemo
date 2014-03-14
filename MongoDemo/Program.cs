using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                const string connStr = @"mongodb://127.0.0.1:27017";
                const string dbName = "CentaLog";
                //MongoServerSettings settings=new MongoServerSettings(ConnectionMode.Automatic,new TimeSpan(0,30,0),new MongoCredentialsStore())
                MongoServer server = MongoServer.Create(connStr);
                MongoDatabase db = server.GetDatabase(dbName);

                Insert(db);
                Query(db);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }

        static void Insert(MongoDatabase database)
        {
            if(database!=null)
            {
                var log = new Log
                {
                    CityCode="010",
                    AppDomain="CoreService",
                    SysName="SalesBlogService",
                    FunName = "GetPostByPostID",
                    Thread=new Random().Next(1024),
                    Level="INFO",
                    RequestTime=DateTime.Now,
                    ResponseTime=DateTime.Now,
                    ResponseCode="200",
                    Logger="MongoLog4NetAppender",
                    XmlContent="<root><content>source:127.0.0.1,port:20717,event:Hello mongodb</content></root>",
                    Message="MongoDB Driver for CSharp",
                    Exception=null,
                    SouceIp="127.0.0.1",
                    Machine=Environment.MachineName,
                    CreateTime=DateTime.Now
                };

                MongoCollection collection = database.GetCollection("log-" + DateTime.Now.ToString("yyyyMM"));
                int i=100;
                while(i>0)
                {
                    collection.Insert<Log>(log);
                    i--;
                }
                
            }
        }
        static void Update(MongoDatabase database)
        {
            var query=new QueryDocument{{"CityCode","010"}};
            var update = new UpdateDocument { { "$set", new QueryDocument { { "CityCode", "022" } } } };

            var col = database.GetCollection("log" + DateTime.Now.ToString("yyyyMM"));
            col.Update(query, update);
        }
        static void Delete(MongoDatabase database)
        {
            var query = new QueryDocument { { "CityCode", "022" } };
            var col = database.GetCollection("log" + DateTime.Now.ToString("yyyyMM"));
            col.Remove(query);
        }

        static void Query(MongoDatabase database)
        {
            var col = database.GetCollection("log-" + DateTime.Now.ToString("yyyyMM"));
            var result = col.FindAll();
            foreach(var item in result)
            {
                Console.WriteLine(item.ToJson());
            }
        }
    }

    class Log
    {
        public string CityCode { get; set; }
        public string AppDomain { get; set; }
        public string SysName { get; set; }
        public string FunName { get; set; }
        public int Thread { get; set; }
        public string Level { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ResponseTime { get; set; }
        public string ResponseCode { get; set; }
        public string Logger{ get; set; }
        public string XmlContent { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string SouceIp { get; set; }
        public string Machine { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
