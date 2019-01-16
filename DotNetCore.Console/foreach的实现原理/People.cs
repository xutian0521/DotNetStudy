using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetCore.Console_.foreach的实现原理
{
    public class People : IEnumerable
    {
        public Person[] _people;
        public People(Person[] pArray)
        {
            _people= new Person[pArray.Length];
            for (int i = 0; i < pArray.Length; i++)
            {
                _people[i]= pArray[i];
            }
        }
        public IEnumerator GetEnumerator()
        {
            return new PeopleEnum(_people);
        }
    }

}