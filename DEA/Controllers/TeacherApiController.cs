using DEA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DEA
{
    public class TeacherApiController : ApiController
    {
        private DEA_DBEntities db = new DEA_DBEntities();

        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        public string ViewSubjects(string TeacherId)
        {
            var subjects = db.AspNetSubjects.Where(x => x.TeacherID == TeacherId).Select(x => new { x.SubjectName, x.AspNetClass.ClassName }).ToList();

            var javaScriptSerializer = new
            System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(subjects);

            return jsonString;
        }


        /////////////////////////////////////////////////////////////////////////////


        //[AcceptVerbs("GET", "POST")]
        //[HttpGet]
        //public string GetAllDiary(string TeacherId)
        //{
        //    var subjects = db.AspNetSubjects.Where(x => x.TeacherID == TeacherId).Select(x => new { x.SubjectName, x.AspNetClass.ClassName }).ToList();

        //    var javaScriptSerializer = new
        //    System.Web.Script.Serialization.JavaScriptSerializer();
        //    string jsonString = javaScriptSerializer.Serialize(subjects);

        //    return jsonString;
        //}
    }
}
