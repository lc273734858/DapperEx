using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FWW.Framework.DapperEx;
using FWW.Framework.DapperExTest.Entity;
using System.Diagnostics;
using FWW.Framework.DapperExTest;
using CreditCenter.Domain;

namespace FWW.Framework.DapperExTest
{
    class Program
    {
        //http://www.itdos.com/Dos/ORM/Index.html 使用教程
        //public static readonly DataContext Context = new DataContext("DosConnTest");//测试环境
        //public static readonly DataContext Context = new DataContext("DosConnLocal");//本地环境
        public static DataContext Context = null;//本地环境
        static void Main(string[] args)
        {
            try
            {
                Context = new DataContext("testDBMySql");
                //GenerateNewTestDbRows(20);
                //var count = Context.From<userinfoEntity>().Count();
                //Console.WriteLine(count);

                //Context.Update(new userinfoEntity() { UserName = "newusername" }, p => p.UserID == "userid");
                //QueryTestDB();
                //var entity = Context.From<BidRecord>(true).Where(BidRecord.F_BidID.In("JJ20160519150324312", "JJ20160519152045218", "JJ20160519153837828")).ToList();
                //Console.WriteLine("修改前：{0}",entity[0].ProductPrice);
                //Context.Update<BidRecord>(new BidRecord() { ProductPrice="10"}, p => p.BidID == "JJ20160519150324312","Status=Null");
                //Context.Excute<>()

                //var result = Context.From<BidRecord>(true).Where(p => p.BidID == "JJ20160519150324312").First();
                //Console.WriteLine("修改后：{0}",result.ProductPrice);
                //Context.Delete(new BidRecord() { BidID= "JJ20160519150324312" });
                //Context.Delete<BidRecordEntity>(p => p.BidID == "JJ20160519165038984");
                //Context.Insert(new BidRecordEntity() { BidID = "JJ20160519165038984", GameID = "b853436cee3c43dfb7221b6c1ffd47be", GameArea = "艾尔之光(月光宝盒)", GameService = "a1015c596cf240ab94866f0ae2f86b96", ProductType = "电信区", ProductID = "25c10a25519549d59834f7db3173e2a1", Status = 1, ProductPrice =101,UserID="1111",UserName="John.liu"});
                //Context.From<userinfoEntity>().Where(p=>p.UserID.Like("1111")).Page(30, 1);
                //var list = Context.From<BidRecordEntity>(true).LeftJoin<PositionTimeDetailEntity>((a, b) => a.BidID == b.BidID)
                //  .Where<PositionTimeDetailEntity>((a, b) => a.BidID == "JJ20160523100851278" && b.BidTimeID == "446c69b1-9c7f-4c69-a42e-55f26103bafc")
                //    .Select()
                //    .Where(new WhereClip("",))
                //    .First<PositionTimeDetailEntity>();
                //Console.WriteLine(Context.LastSqlExpress);
                //var where = new Where<BidRecordEntity>();
                //where.And(p => p.Active == true);
                //GenerateNewRows();
                //Context.From<BidRecordEntity>().Select(new Field[] { BidRecordEntity.F_BidID, BidRecordEntity.F_Commission.Sum().As("test1")}).GroupBy(p => p.BidID).ToList();
                //Console.WriteLine(Context.LastSqlExpress);
                //ExcuteSelect();

                //var from = Context.From<BidRecordEntity>(true).LeftJoin<PositionTimeDetailEntity>((a, b) => a.BidID == b.BidID)
                //    .Where<PositionTimeDetailEntity>((a, b) => a.BidID == "JJ20160523100851278" && b.BidTimeID == "446c69b1-9c7f-4c69-a42e-55f26103bafc");

                //var tb = Context.Procedure("test").AddInParameter("BidID", System.Data.DbType.String, "JJ20160519152045218").ToFirst<BidRecordEntity>();
                //Console.WriteLine(Context.LastSqlExpress);
                //var count = Context.From<BidRecordEntity>().Where(p => p.Status == 1).Count();
                //Console.WriteLine("联合查询：{0}", count);


                //var list = new List<ComparItem>();
                //list.Add(new ComparItem(new SelectTest(), "Select DapperEx"));
                //list.Add(new ComparItem(new SelectHartCodeTest(), "Select Dapper"));
                //list.Add(new ComparItem(new HardCodeTest(), "Select HardCode"));                
                //list.Add(new ComparItem(new SelectDosORM(), "DosORM"));

                //int excutetimes = 10;
                //while (Console.ReadLine().ToLower()!="exit")
                //{
                //    foreach (var item in list)
                //    {
                //        item.TestFactory.Excute(excutetimes);
                //        Console.WriteLine("{0}执行时间:{1}", item.Name, item.TestFactory.ExcuteTime);
                //    }
                //}
                //Context.From<TestTableEntity>().LeftJoin<>
                var user=Context.From<ChatMessage.Entity.chatmessageEntity>().Where(p => p.UserID.Like("0692")).First();
                Console.WriteLine(user.UserID);
                Console.WriteLine(Context.LastSqlExpress);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (Context != null)
                {
                    Console.WriteLine(Context.LastSqlExpress);
                }
            }
            Console.ReadLine();
        }

        static private void GenerateNewTestDbRows(int count)
        {
            var key=DateTime.Now.ToString("yyyyMMddHHMMss");
            for (int i = 0; i < count; i++)
            {
                var userinfo = new userinfoEntity()
                {
                    UserID = string.Concat(key, "-", i.ToString()),
                    UserName = string.Concat("testUser", i),
                    CreationDate = DateTime.Now,
                    tag = i
                };
                Context.Insert(userinfo);
                for (int j = 0; j < 2; j++)
                {
                    Context.Insert(new userinfoexEntity()
                    {
                        UserID = userinfo.UserID,
                        CreationDate = DateTime.Now,
                        ID = Guid.NewGuid().ToString()
                        ,tag=userinfo.tag
                    });
                }
                Console.WriteLine("{0}秒-{1}",DateTime.Now .Second,i);
            }
        }
        static private void TestDBDelete(string userid)
        {
            //"20161212151217-1018"
            var entity = Context.From<userinfoEntity>().Where(p => p.UserID == userid).First();
            Console.WriteLine(entity.ToString());
            Context.Delete(entity);
            Console.WriteLine(Context.LastSqlExpress);            
        }
        static private void QueryTestDB()
        {
            var entity=Context.From<userinfoEntity>().RightJoin<userinfoexEntity>((left, b) => left.UserID==b.UserID).Where<userinfoexEntity>((a,b)=>a.UserName== "testUser513").First();
            Console.WriteLine(entity.ToString());
            Console.WriteLine(Context.LastSqlExpress);
        }
        static private void GenerateNewRows()
        {
            for (int i = 0; i < 50000; i++)
            {
                Context.Insert(new TestTableEntity()
                {
                    Name = "test" + i,
                    Name1 = "test" + i,
                    Name2 = "test" + i,
                    Name3 = "test" + i,
                    Name4 = "test" + i,
                    Name5 = "test" + i
                });
                Console.WriteLine(Context.LastSqlExpress);
            }
        }
        static private void ExcuteSelect()
        {
            Stopwatch stop = new Stopwatch();
            stop.Start();
            var list = Context.From<TestTableEntity>().Top(5000).Where(p => p.ID > 5000).ToList();
            stop.Stop();
            Console.WriteLine(stop.ElapsedMilliseconds);
        }
    }
}
