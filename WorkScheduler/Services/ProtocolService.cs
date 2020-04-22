using System;
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
        private static CyrNounCollection cyrNounCollection;
        private static CyrAdjectiveCollection cyrAdjectiveCollection;

        public ProtocolService(Context context)
        {
            Db = context;

            cyrNounCollection = new CyrNounCollection();
            cyrAdjectiveCollection = new CyrAdjectiveCollection();
        }

        public ProtocolViewModel GetProtocolOrCreate(int actionId)
        {
            var protocolId = Db.Protocols.FirstOrDefault(p => p.ActionId == actionId)?.Id;

            if (protocolId != null)
            {
                return GetProtocol((int)protocolId);
            }

            var action = Db.Actions.FirstOrDefault(a => a.Id == actionId);

            var lastNumber = Db.Protocols
                .Where(p => p.Action.Name.Trim().ToLower() == action.Name.Trim().ToLower()
                       && action.Date.Year == action.Date.Year
                       && p.Action.WorkSchedule.User.SchoolId == action.WorkSchedule.User.SchoolId)
                .OrderBy(p => p.Number)
                .Last().Number;

            var cyrPhrase = new CyrPhrase(cyrNounCollection, cyrAdjectiveCollection);
            CyrResult cyrResult = cyrPhrase.Decline(action.Name, GetConditionsEnum.Strict);

            var newProtocol = new Protocol
            {
                ActionId = actionId,
                Name = cyrResult.Genitive,
                Number = (Convert.ToInt32(lastNumber) + 1).ToString(),
            };

            Db.Protocols.Add(newProtocol);
            Db.SaveChanges();

            return GetProtocol(newProtocol.Id);
        }

        public ProtocolViewModel GetProtocol(int protocolId)
        {
            var protocol = Db.Protocols.Include(p => p.Action).FirstOrDefault(p => p.Id == protocolId);

            if (protocol == null)
            {
                return null;
            }

            return new ProtocolViewModel
            {
                Id = protocol.Id,
                Name = protocol.Name,
                Number = protocol.Number,
                Agenda = protocol.Agenda,
                Attended = protocol.Attended,
                Chairman = protocol.Chairman,
                CreatedAt = protocol.CreatedAt,
                Decided = protocol.Decided,
                Header = protocol.Header,
                Listen = protocol.Listen,
                Secretary = protocol.Secretary,
                Speaked = protocol.Speaked,
                Action = new ActionViewModel
                {
                    Id = protocol.Action.Id,
                    Name = protocol.Action.Name
                }
            };
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
    }
}
