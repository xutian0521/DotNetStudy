#### C#中foreach的实现原理

1. 首先对象要实现IEnumerable 接口(IEnumerable 意思是可枚举的)，实现GetEnumerator 方法，返回一个(枚举器)迭代器
``` cs
public class People : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        return new PeopleEnum(_people);
    }
}

```
2. 迭代器 实现 IEnumerator 接口(IEnumerator 枚举器), 实现2分方法 一个属性。Current 属性(当前迭代对象) MoveNext() 迭代下一个对象的方法 Reset() 重置枚举器
``` cs
public class PeopleEnum : IEnumerator
{
    public Person[] _people;
    int _position = -1;
    public PeopleEnum(Person[] list)
    {
        _people= list;
    }
    public object Current { get { return _people[_position]; }}


    public bool MoveNext()
    {
        _position++;
        return _people.Length > _position;
    }

    public void Reset()
    {
        _position = -1;
    }
}
```
3. 反编译C# 代码foreach底层实现大概是这样的

```cs
IEnumerator People = p.GetEnumerator();
while (p1.MoveNext())
{
    Person str=(Person)p1.Current;
    Console.WriteLine(str);
}
```