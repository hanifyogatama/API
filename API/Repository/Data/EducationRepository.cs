using API.Context;
using API.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace API.Repository.Data
{
    public class EducationRepository : GeneralRepository<MyContext, Education, int>
    {
        private readonly MyContext _context;
        public EducationRepository(MyContext myContext) : base(myContext)
        {
            this._context = myContext;  
        }

        public IEnumerable ShowDegree()
        {

            var degrees = new List<Degree>
            {
                new Degree
                {
                    name = "S1"
                },
                new Degree
                {
                    name = "S2"
                },
                new Degree
                {
                    name = "S3"
                },
                 new Degree
                {
                    name = "D-I"
                },
                  new Degree
                {
                    name = "D-II"
                },
                   new Degree
                {
                    name = "D-III"
                },
                    new Degree
                {
                    name = "D-IV"
                },
                new Degree
                {
                    name = "SMK"
                },
                new Degree
                {
                    name = "SMA"
                }
            };

            return degrees;
        }


        public class Degree
        {
            public string name { get; set; }
        }


    }
}
