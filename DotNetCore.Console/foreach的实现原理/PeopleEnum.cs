using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetCore.Console_.foreach的实现原理
{
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
}