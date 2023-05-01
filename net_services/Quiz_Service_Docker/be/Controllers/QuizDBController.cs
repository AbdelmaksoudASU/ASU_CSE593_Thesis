using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz_Service.Data;
using Quiz_Service.Models;
using System.Collections.Generic;

namespace Quiz_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizDBController : ControllerBase
    {
        private readonly QuizContext _context;

        public QuizDBController(QuizContext context)
        {
            _context = context;
        }
        [HttpPost("programs")]
        public ActionResult<string> SaveToDB(List<Program_Matching_Criteria> programs)
        {
            foreach (var program in programs)
            {
                program.ProgramId = Guid.NewGuid().ToString();
                program.Fix_Sum();
                _context.QuizDB.Add(program);
            }    
            _context.SaveChanges();
            return Ok("success");
        }
        [HttpPost("program")]
        public ActionResult<string> SaveToDB(Program_Matching_Criteria program, string ProgramName)
        {
            program.ProgramId = Guid.NewGuid().ToString();
            program.Fix_Sum();
            _context.QuizDB.Add(program);
            _context.SaveChanges();
            return Ok("success");
        }

        [HttpPatch("program")]
        public ActionResult<string> UpdateDB(Dictionary<string, double> program, string ProgramName)
        {
            Program_Matching_Criteria programScores = _context.QuizDB.First(p => p.ProgramName == ProgramName);
            programScores.Update_Program(program, ProgramName);
            _context.SaveChanges();
            return Ok("success");
        }
        [HttpDelete("program")]
        public ActionResult<string> DeleteDB(string ProgramName)
        {
            Program_Matching_Criteria programScores = _context.QuizDB.First(p => p.ProgramName == ProgramName);
            _context.QuizDB.Remove(programScores);
            _context.SaveChanges();
            return Ok("success");
        }
        [HttpGet("program")]
        public ActionResult<Program_Matching_Criteria> GetDB(string ProgramName)
        {
            Program_Matching_Criteria programScores = _context.QuizDB.First(p => p.ProgramName == ProgramName);
            return Ok(programScores);
        }
        [HttpPost("quizresult")]
        public IEnumerable<String> Get_Quiz_Results(Program_Matching_Criteria Student_Scores)
        {
            //IEnumerable<String, double> Top_Five_Vals = new Dictionary<String, double>();
            List<String> Top_Five = new List<String>();
            Dictionary<String, double> Student_Program_Scores = new Dictionary<String, double>();
            List<Program_Matching_Criteria> programScores = new List<Program_Matching_Criteria>();
            programScores = _context.QuizDB.ToList();
            foreach (var program in programScores)
            {
                Student_Program_Scores.Add(program.ProgramName, program.Calculate_Score(Student_Scores));
            }
            var Top_Five_Vals = (from entry in Student_Program_Scores orderby entry.Value descending select entry.Key);
            var Top_Five_Keys = Top_Five_Vals.Take(5);
            Top_Five = Top_Five_Keys.ToList();
            return Top_Five;
        }

       
    }
}
