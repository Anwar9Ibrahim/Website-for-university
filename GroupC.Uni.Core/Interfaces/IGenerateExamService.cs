using GroupC.Uni.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroupC.Uni.Core.Interfaces
{
    public interface IGenerateExamService
    {
        List<ExamQuestion> GenerateExam(int numOfQuestions, Guid courseId);
    }
}
