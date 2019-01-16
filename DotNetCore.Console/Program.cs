using System;
using DotNetCore.Console_.foreach的实现原理;

namespace DotNetCore.Console_
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ForeachTest();
            Console.ReadKey();
        }

        static void ForeachTest()
        {
            Person[] peopleArray=new Person[3]
            {
            new Person("John", "Smith"),  
            new Person("Jim", "Johnson"),  
            new Person("Sue", "Rabon"),  
            };
            People peopleList=new People(peopleArray);

            foreach (Person p in peopleList)
            {
                Console.WriteLine(p.firstName + " " + p.lastName);  
            }

        }
    }
}
