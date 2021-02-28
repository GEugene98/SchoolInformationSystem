using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.ViewModels.Monitoring.Shared;
using WorkScheduler.ViewModels.Register;

namespace WorkScheduler.Services.Register
{
    public class RegisterService
    {
        protected Context Db;
        protected PlaningRecordService PlaningRecordService;
        protected GroupService GroupService;

        public RegisterService(Context context, PlaningRecordService planingRecordService, GroupService groupService)
        {
            Db = context;
            PlaningRecordService = planingRecordService;
            GroupService = groupService;
        }

        public IEnumerable<RegisterRow> GetRecords(int academicYearId, int associationId, int groupId, int schoolId)
        {
            var planingRecords = PlaningRecordService.GetRecords(academicYearId, associationId, groupId).Where(p => p.Date.HasValue).ToList();

            var aType = Db.Associations.FirstOrDefault(a => a.Id == associationId).Type;

            var students = Db.GroupStudents.Where(gs => gs.GroupId == groupId).Select(gs => gs.Student).ToList();

            var rows = students.Select(s => new RegisterRow
            {
                Student = new StudentViewModel
                {
                    Id = s.Id,
                    FullName = s.LastName + " " + s.FirstName[0] + ". " + s.SurName[0] + ". "
                },
                 
            })
            .ToArray();

            var registerRecords = Db.PlaningRecords
                .Where(p => p.AcademicYearId == academicYearId && p.AssociationId == associationId && p.GroupId == groupId && p.Date.HasValue)
                .OrderBy(p => p.Date)
                .SelectMany(p => p.RegisterRecords)
                .Include(rr => rr.PlaningRecord)
                .ToList()
                .GroupBy(r => r.StudentId);

            for (int i = 0; i < rows.Length; i++)
            {
                var regRecs = registerRecords.FirstOrDefault(g => g.Key == rows[i].Student.Id);

                var cells = new List<RegisterRecordViewModel>();

                foreach (var cell in planingRecords)
                {
                    var newCell = new RegisterRecordViewModel();
                    newCell.Date = cell.Date;

                    if (regRecs != null)
                    {
                        var foundRec = regRecs.FirstOrDefault(rr => rr.PlaningRecord.Date == cell.Date);
                        if (foundRec != null)
                        {
                            newCell.Id = foundRec.Id;
                            newCell.Content = foundRec.Content;
                            newCell.Date = foundRec.PlaningRecord.Date;
                        }
                    }

                    cells.Add(newCell);
                }

                rows[i].Cells = cells.OrderBy(c => c.Date).ToList();
            }

            return rows;
        }
    }
}
