## 高并发容易造成业务问题
1. 安装数据库更新方式 一般高并发导致业务问题分为两种：a.并发更新 b. 并发插入
2. 容易参数并发的代码
``` cs
var amount= select amount from t_user where id=@id

if(amount > 0)
{
    update t_user set amount=amount -1
} 

var hasCount= select count(id) from t_redpacket where userId=@userId
if(hasCount == 0)
{
    insert t_redpacket(userId,amount) values('123', 50)
}
```
> 一般防止并发 是保证很多次同样的请求，保证进入判断方法只有一次。在生成环境中，你觉得某个接口一个客户端同一次操作之后又一次请求，但是实际情况中，可能同一个操作产生多次同样的请求，而我们代码是多线程的，这样就有可能会导致同样的逻辑代码会并发执行。
解决方法：
1. 使用代码中的线程锁 实现, 让同样的逻辑判断同时只能有一个线程执行, 坏处：导致代码性能低, 接口的吞吐量下降, 这种方法只能在你的接口是 单点部署, 如采用的是集群部署的接口就不行了, 因为大量请求来的时候, 在不同服务器上运行的同样代码，我们的线程锁只能 对当前服务器当前接口进程有效。
2. 使用数据库锁(如：在查询语句上 加排它锁，更新时候加where 条件，WHERE NOT EXISTS SELECT等) --*如果是并发插入的话，还有一个邪道方法, 用数据库的唯一键约束放在并发插入*。
4. 第三方队列执行
5. 使用Redis的setnx 分布式锁
``` cs
//SETNX是 Redis分布式锁, 分布式多线程只会执行一次，第二次执行就不满足了，直到缓存时间结束
if((await _instance.GetDatabase(10).ExecuteAsync("SETNX", new  string[] { $"Cache:flag:{Id}", "1" })).ToString() == "1")
{
    //设置标识缓存时间
   await  _instance.GetDatabase(10).KeyExpireAsync($"Cache:flag:{Id}",  TimeSpan.FromMinutes(2));
   //业务
}
```