using System;
using System.Collections.Generic;
using System.Linq;
using Cyriller;
using Cyriller.Model;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models;
using WorkScheduler.ViewModels;
using WorkScheduler.ViewModels.Scheduler;

namespace WorkScheduler.Services
{
    public class ProtocolService
    {
        protected Context Db;
        private Logger Logger;

        public ProtocolService(Context context)
        {
            Db = context;
            Logger = Logger.GetInstance();

        }

        public List<ProtocolInfo> GetProtocolList(string userId, int year)
        {
            var result = Db.Protocols
                .Where(p => p.Action.WorkSchedule.UserId == userId && p.Action.Date.Year == year)
                .Select(p => new ProtocolInfo
                    {
                        Id = p.Id,
                        Number = p.Number,
                        ActionName = p.Action.Name,
                        ActionDate = p.Action.Date,
                        ActionId = p.ActionId,
                        CreatedAt = p.CreatedAt
                    }
                )
                .OrderBy(p => p.ActionName)
                .ThenBy(p => p.Number)
                .ToList();

            return result;
        }

        public List<ProtocolInfo> GetFullProtocolList(int schoolId, int year)
        {
            var result = Db.Protocols
                .Where(p => p.Action.WorkSchedule.User.SchoolId == schoolId && p.Action.Date.Year == year)
                .Select(p => new ProtocolInfo
                {
                    Id = p.Id,
                    Number = p.Number,
                    ActionName = p.Action.Name,
                    ActionDate = p.Action.Date,
                    ActionId = p.ActionId,
                    CreatedAt = p.CreatedAt,
                    ScheduleOwner = p.Action.WorkSchedule.User.LastName + " " + p.Action.WorkSchedule.User.FirstName[0] + ". " + p.Action.WorkSchedule.User.SurName[0] + "."
                }
                )
                .OrderBy(p => p.ActionName)
                .ThenBy(p => p.Number)
                .ToList();

            return result;
        }

        public void UpdateProtocol(ProtocolViewModel protocol)
        {
            var protocolNumber = 0;
            if (!int.TryParse(protocol.Number, out protocolNumber))
            {
                throw new Exception("Номером протокола может являться только число");
            }


            var foundProtocol = Db.Protocols.FirstOrDefault(p => p.Id == protocol.Id);


            foundProtocol.Name = protocol.Name;
            foundProtocol.Number = protocol.Number;
            foundProtocol.Agenda = protocol.Agenda;
            foundProtocol.Attended = protocol.Attended;
            foundProtocol.Chairman = protocol.Chairman;
            foundProtocol.Decided = protocol.Decided;
            foundProtocol.Listen = protocol.Listen;
            foundProtocol.Secretary = protocol.Secretary;
            foundProtocol.Speaked = protocol.Speaked;

            Db.SaveChanges();
        }

        public ProtocolViewModel GetProtocolOrCreate(int actionId)
        {
            var protocolId = Db.Protocols.FirstOrDefault(p => p.ActionId == actionId)?.Id;

            if (protocolId != null)
            {
                return GetProtocol((int)protocolId);
            }

            var action = Db.Actions.Include(a => a.WorkSchedule.User).FirstOrDefault(a => a.Id == actionId);

            var lastNumber = 0;

            var protocols = Db.Protocols
                .Where(p => p.Action.Name == action.Name
                       && p.Action.Date.Year == action.Date.Year
                       && p.Action.WorkSchedule.User.SchoolId == action.WorkSchedule.User.SchoolId)
                .ToList();

            if (protocols.Count() != 0)
            {
                var lNumber = protocols
                                .OrderBy(p => p.Number)
                                .LastOrDefault()
                                .Number;

                lastNumber = Convert.ToInt32(lNumber);
            }

            var protocolName = action.Name;

            try
            {
                var cyrNounCollection = new CyrNounCollection();
                var cyrAdjectiveCollection = new CyrAdjectiveCollection();

                var cyrPhrase = new CyrPhrase(cyrNounCollection, cyrAdjectiveCollection);
                CyrResult cyrResult = cyrPhrase.Decline(action.Name, GetConditionsEnum.Similar);

                protocolName = cyrResult.Genitive;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            
            var newProtocol = new Protocol
            {
                ActionId = actionId,
                Name = protocolName,
                Number = (lastNumber + 1).ToString(),
                CreatedAt = DateTime.Now
            };

            Db.Protocols.Add(newProtocol);
            Db.SaveChanges();

            return GetProtocol(newProtocol.Id);
        }

        public ProtocolViewModel GetProtocol(int protocolId, int? schoolId = null)
        {
            var protocol = Db.Protocols.Include(p => p.Action).FirstOrDefault(p => p.Id == protocolId);

            if (protocol == null)
            {
                return null;
            }

            var result = new ProtocolViewModel
            {
                Id = protocol.Id,
                Name = protocol.Name,
                Number = protocol.Number,
                Agenda = protocol.Agenda,
                Attended = protocol.Attended,
                Chairman = protocol.Chairman,
                CreatedAt = protocol.CreatedAt,
                Decided = protocol.Decided,
                Listen = protocol.Listen,
                Secretary = protocol.Secretary,
                Speaked = protocol.Speaked,
                Action = new ActionViewModel
                {
                    Id = protocol.Action.Id,
                    Name = protocol.Action.Name,
                    Date = protocol.Action.Date
                }
            };

            if (schoolId.HasValue)
            {
                var school = Db.Schools.FirstOrDefault(s => s.Id == schoolId);
                result.Header = school.DocumentHeaderHTML;
            }

            return result;
        }

        public void DeleteProtocol(int protocolId)
        {
            var protocol = Db.Protocols.FirstOrDefault(p => p.Id == protocolId);

            if (protocol == null)
            {
                throw new Exception("Протокол уже был удален. Обновите страницу");
            }

            Db.Protocols.Remove(protocol);
            Db.SaveChanges();
        }

        public bool ProtocolExists(int actionId)
        {
            var protocol = Db.Protocols.FirstOrDefault(p => p.ActionId == actionId);
            return protocol != null;
        }
    }
}
