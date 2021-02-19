﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using WorkScheduler.Models.Register;
using WorkScheduler.ViewModels.Register;

namespace WorkScheduler.Services.Register
{
    public class PlaningRecordService
    {
        protected readonly string RootPath;
        protected Context Db;

        public PlaningRecordService(Context context, IConfiguration configuration)
        {
            Db = context;
            RootPath = configuration.GetValue<string>("UploadPath");
        }

        public void ParseExcelAndWriteToDB(IFormFile file, ImportPlaning importModel)
        {
            var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            var transactionDirectory = new DirectoryInfo(Path.Combine(RootPath, "Tmp", Guid.NewGuid().ToString()));

            if (!transactionDirectory.Exists)
            {
                transactionDirectory.Create();
            }

            string fullPhysicalPath = Path.Combine(transactionDirectory.FullName, Path.GetFileName(fileContent.FileName.Trim('"')));

            using (FileStream stream = new FileStream(fullPhysicalPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                file.CopyTo(stream);
            }

            using (var excelPack = new ExcelPackage())
            {
                using (var stream = File.OpenRead(fullPhysicalPath))
                {
                    excelPack.Load(stream);
                }

                var ws = excelPack.Workbook.Worksheets.FirstOrDefault();

                var dateCells = ws.Cells[importModel.DateRange];
                var nameCells = ws.Cells[importModel.NameRange];
                var hoursCells = ws.Cells[importModel.HoursRange];
                var commentCells = ws.Cells[importModel.CommentRange];

                var countOfRows = nameCells.Count();
                if (!(dateCells.Count() == countOfRows && hoursCells.Count() == countOfRows && commentCells.Count() == countOfRows))
                {
                    throw new Exception("Количество строк по всем полям должно быть одинаковым");
                }

                var dates = new List<DateTime?>();
                var names = new List<string>();
                var hoursList = new List<int?>();
                var comments = new List<string>();

                foreach (var cell in dateCells)
                {
                    if (cell.Value == null)
                    {
                        dates.Add(null);
                        continue;
                    }

                    long dateNum;
                    bool result = long.TryParse(cell.Value.ToString(), out dateNum);
                    if (result)
                    {
                        dates.Add(DateTime.FromOADate(dateNum));
                    }
                    else
                    {
                        dates.Add(null);
                    }
                }

                foreach (var cell in nameCells)
                {
                    if (cell.Value == null)
                    {
                        names.Add(null);
                        continue;
                    }

                    names.Add(cell.Value.ToString());
                }

                foreach (var cell in hoursCells)
                {
                    if (cell.Value == null)
                    {
                        hoursList.Add(null);
                        continue;
                    }

                    hoursList.Add(Convert.ToInt32(cell.Value));
                }

                foreach (var cell in commentCells)
                {
                    if (cell.Value == null)
                    {
                        comments.Add(null);
                        continue;
                    }

                    comments.Add(cell.Value.ToString());
                }

                var recordPlanings = new List<PlaningRecord>();
                for (int i = 0; i < countOfRows; i++)
                {
                    var newRecord = new PlaningRecord
                    {
                        AcademicYearId = importModel.AcademicYearId,
                        AssociationId = importModel.AssociationId,
                        GroupId = importModel.GroupId,
                        Name = names[i],
                        Date = dates[i],
                        Hours = hoursList[i],
                        Comment = comments[i]
                    };
                    recordPlanings.Add(newRecord);
                }

                Db.PlaningRecords.AddRange(recordPlanings);
                Db.SaveChanges();
            }

            transactionDirectory.Delete(true);
        }

        public IEnumerable<PlaningRecordViewModel> GetRecords(int academicYearId, int associationId, int groupId)
        {
            return Db.PlaningRecords
                .Where(p => p.AcademicYearId == academicYearId && p.AssociationId == associationId && p.GroupId == groupId)
                .Select(p => new PlaningRecordViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Date = p.Date,
                    Hours = p.Hours,
                    Comment = p.Comment
                });
        }
    }
}
