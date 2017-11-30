# DapperEx

app.config或者web.config 中配置数据库连接 ConnectionStrings
```sql
  <connectionStrings>
    <add name="DosConnLocal" providerName="默认不填为MSSql" connectionString="server=.;database=BidService;uid=sa;pwd=11111;"/>
    <add name="DosConnTest" providerName="默认不填为MSSql" connectionString="server=192.168.130.19;database=BidService;uid=bidserconn;pwd=1111;"/>
    <add name="mysqlTest" providerName="mysql" connectionString="Server=192.168.130.112;Database=CreditCenter;User=root;Password=//5173@#;Port=3306
;"/>
    <add name="testDBMySql" providerName="mysql" connectionString="Server=localhost;Database=TESTDB;User=root;Password=111111;Port=8066;"/>
    <!--<add name="testDBMySql" providerName="mysql" connectionString="Server=192.168.158.130;Database=testdb2;User=root;Password=11111;Port=3306;"/>-->
  </connectionStrings>
```
创建全局对象
```csharp
    public class DB
    {
        public static readonly DataContext Instance = new DataContext("chatrecord");
    }
```

或者独立创建
```csharp
    Context = new DataContext("testDBMySql");
```
## Insert
**普通插入  Insert(object entity)  如果有自增ID，插入之后实体中的自增ID将会被赋值为新数据的ID**
```csharp
var userinfo = new userinfoEntity()
{
	UserID = string.Concat(key, "-", i.ToString()),
	UserName = string.Concat("testUser", i),
	CreationDate = DateTime.Now,
	tag = 1
};
Context.Insert(userinfo);
Console.WriteLine(userinfo.AutoID);
```
**自增ID不返回插入  InsertNoReturnAutoID  插入数据之后如果自增ID不想返回，节约性能，可以使用此方法**

## select

**等于**
```csharp
var result = Context.From<BidRecord>(true).Where(p => p.BidID == "JJ20160519150324312").First();
```

**包含**
```csharp
var entity = Context.From<BidRecord>(true).Where(p=>p.BidID.In("JJ20160519150324312", "JJ20160519152045218", "JJ20160519153837828")).ToList();
```

**join  请尽量避免使用join查询，为未来分库分表打下基础**
```csharp
var list = Context.From<BidRecordEntity>(true).LeftJoin<PositionTimeDetailEntity>((a, b) => a.BidID == b.BidID)
                  .Where<PositionTimeDetailEntity>((a, b) => a.BidID == "JJ20160523100851278" && b.BidTimeID == "446c69b1-9c7f-4c69-a42e-55f26103bafc")
                    .Select()
                    .Where(new WhereClip("",))
                    .First<PositionTimeDetailEntity>();
```

**Group By**

```csharp
Context.From<BidRecordEntity>().Select(new Field[] { BidRecordEntity.F_BidID, BidRecordEntity.F_Commission.Sum().As("test1")}).GroupBy(p => p.BidID).ToList();
```

**Like**

```csharp
Context.From<ChatMessage.Entity.chatmessageEntity>().Where(p => p.UserID.Like("0692")).First();
```

## Delete
**根据主键删除**
```csharp
Context.Delete(new userinfoEntity() { UserID = "111" });
```

**根据条件删除**
```csharp
Context.Delete<userinfoEntity>(p=>p.UserID=="");
```

## update
**根据主键更新**
```csharp
Context.UpdateByPrimaryKey(new userinfoEntity() { UserID = "", UserName = "new username" });
```

**根据条件更新**
```csharp
Context.Update(new userinfoEntity() { UserName = "newusername" }, p => p.UserID == "userid");
```

## Procedure
```csharp
var tb = Context.Procedure("test").AddInParameter("BidID", System.Data.DbType.String, "JJ20160519152045218").ToFirst<BidRecordEntity>();
```

## 原生Dapper语法
```csharp
Context.Excute<userinfoEntity>("select * from userinfo where userid=@userid", new { userid = "1111" });
```
